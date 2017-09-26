using System;
using System.Collections.Generic;
using Core.Diagnostics;
using Core.Linq;

namespace Core.Clang
{
    /// <summary>
    /// An "index" that consists of a set of translation units that would typically be linked
    /// together into an executable or library.
    /// </summary>
    public sealed unsafe class Index : IDisposable
    {
        internal CXIndexImpl* Ptr { get; }

        /// <summary>
        /// Provides a shared context for creating translation units.
        /// </summary>
        /// <param name="excludeDeclarationsFromPCH">
        /// When true, allows enumeration of "local" declarations (when loading any new
        /// translation units). A "local" declaration is one that belongs in the translation unit
        /// itself and not in a precompiled header that was used by the translation unit. If false,
        /// all declarations will be enumerated.
        /// </param>
        /// <param name="displayDiagnostics">
        /// true to print diagnostics to standard error.
        /// </param>
        public Index(bool excludeDeclarationsFromPCH, bool displayDiagnostics)
        {
            Ptr = NativeMethods.clang_createIndex(
                Convert.ToInt32(excludeDeclarationsFromPCH),
                Convert.ToInt32(displayDiagnostics));
        }

        private bool disposed;

        /// <summary>
        /// Destroys the index.
        /// </summary>
        /// <remarks>
        /// The index must not be destroyed until all of the translation units created within that
        /// index have been destroyed.
        /// </remarks>
        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.clang_disposeIndex(Ptr);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroys the index.
        /// </summary>
        ~Index()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(Index).Name);
            }
        }

        /// <summary>
        /// Sets general options associated with the <see cref="Index"/>.
        /// </summary>
        /// <param name="options">
        /// A bitmask of options, a bitwise OR of <see cref="GlobalOptions"/>.
        /// </param>
        public void SetGlobalOptions(GlobalOptions options)
        {
            ThrowIfDisposed();

            NativeMethods.clang_CXIndex_setGlobalOptions(Ptr, (uint)options);
        }

        /// <summary>
        /// Gets the general options associated with an <see cref="Index"/>.
        /// </summary>
        /// <returns>
        /// A bitmask of options, a bitwise OR of <see cref="GlobalOptions"/> flags that are
        /// associated with the <see cref="Index"/> object.
        /// </returns>
        public GlobalOptions GetGlobalOptions()
        {
            ThrowIfDisposed();

            return (GlobalOptions)NativeMethods.clang_CXIndex_getGlobalOptions(Ptr);
        }

        /// <summary>
        /// Create a translation unit from an AST file (-emit-ast).
        /// </summary>
        /// <param name="astFileName">The path to the AST file.</param>
        /// <returns>
        /// The translation unit from the AST file.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="astFileName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="astFileName"/> is empty.
        /// </exception>
        /// <exception cref="ErrorCodeException">
        /// The error code returned by libclang is not <see cref="ErrorCode.Success"/>.
        /// </exception>
        public TranslationUnit CreateTranslationUnit(string astFileName)
        {
            Requires.NotNullOrEmpty(astFileName, nameof(astFileName));
            ThrowIfDisposed();

            using (var cString = new CString(astFileName))
            {
                CXTranslationUnitImpl* ptr;
                NativeMethods.clang_createTranslationUnit2(
                    Ptr,
                    cString.Ptr,
                    &ptr).Check();
                return new TranslationUnit(ptr, this);
            }
        }

        /// <summary>
        /// Parse the given source file and the translation unit corresponding to that file.
        /// </summary>
        /// <param name="sourceFileName">
        /// The name of the source file to load, or null if the source file is included in
        /// <paramref name="commandLineArgs"/>.
        /// </param>
        /// <param name="commandLineArgs">
        /// The command-line arguments that would be passed to the clang executable if it were
        /// being invoked out-of-process. These command-line options will be parsed and will affect
        /// how the translation unit is parsed. Note that the following options are ignored: '-c',
        /// '-emit-ast', '-fsyntax-only' (which is the default), and '-o &lt;output file&gt;'.
        /// </param>
        /// <param name="unsavedFiles">
        /// The files that have not yet been saved to disk but may be required for parsing,
        /// including the contents of those files.
        /// </param>
        /// <param name="options">
        /// A bitmask of options that affects how the translation unit is managed but not its
        /// compilation. This should be a bitwise OR of the
        /// <see cref="TranslationUnitCreationOptions"/> flags.
        /// </param>
        /// <returns>
        /// The created <see cref="TranslationUnit"/>, describing the parsed code and containing
        /// any diagnostics produced by the compiler.
        /// </returns>
        /// <remarks>
        /// This method is the main entry point for the Clang C API, providing the ability to
        /// parse a source file into a translation unit that can then be queried by other functions
        /// in the API. This routine accepts a set of command-line arguments so that the
        /// compilation can be configured in the same way that the compiler is configured on the
        /// command line.
        /// </remarks>
        /// <exception cref="ErrorCodeException">
        /// The error code returned by libclang is not <see cref="ErrorCode.Success"/>.
        /// </exception>
        public TranslationUnit ParseTranslationUnit(
            string sourceFileName,
            IEnumerable<string> commandLineArgs,
            IEnumerable<UnsavedFile> unsavedFiles = null,
            TranslationUnitCreationOptions options = TranslationUnitCreationOptions.None)
        {
            ThrowIfDisposed();

            using (var cString = new CString(sourceFileName))
            using (var args = new CStrings(commandLineArgs))
            using (var files = new CXUnsavedFiles(unsavedFiles))
            {
                var argsPtr = stackalloc sbyte*[args.Count];
                args.Apply((arg, i) => argsPtr[i] = arg.Ptr);
                var filesPtr = stackalloc CXUnsavedFile[files.Count];
                files.Apply((file, i) => filesPtr[i] = files[i]);

                CXTranslationUnitImpl* ptr;
                NativeMethods.clang_parseTranslationUnit2(
                    Ptr,
                    cString.Ptr,
                    argsPtr,
                    args.Count,
                    filesPtr,
                    (uint)files.Count,
                    (uint)options,
                    &ptr).Check();
                return new TranslationUnit(ptr, this);
            }
        }

        /// <summary>
        /// Same as <see cref="ParseTranslationUnit"/> but requires a full command line for
        /// <paramref name="commandLineArgs"/> including args[0]. This is useful if the standard
        /// library paths are relative to the binary.
        /// </summary>
        /// <param name="sourceFileName">
        /// The name of the source file to load, or null if the source file is included in
        /// <paramref name="commandLineArgs"/>.
        /// </param>
        /// <param name="commandLineArgs">
        /// The command-line arguments that would be passed to the clang executable if it were
        /// being invoked out-of-process. These command-line options will be parsed and will affect
        /// how the translation unit is parsed. Note that the following options are ignored: '-c',
        /// '-emit-ast', '-fsyntax-only' (which is the default), and '-o &lt;output file&gt;'.
        /// </param>
        /// <param name="unsavedFiles">
        /// The files that have not yet been saved to disk but may be required for parsing,
        /// including the contents of those files.
        /// </param>
        /// <param name="options">
        /// A bitmask of options that affects how the translation unit is managed but not its
        /// compilation. This should be a bitwise OR of the
        /// <see cref="TranslationUnitCreationOptions"/> flags.
        /// </param>
        /// <returns>
        /// The created <see cref="TranslationUnit"/>, describing the parsed code and containing
        /// any diagnostics produced by the compiler.
        /// </returns>
        /// <exception cref="ErrorCodeException">
        /// The error code returned by libclang is not <see cref="ErrorCode.Success"/>.
        /// </exception>
        public TranslationUnit ParseTranslationUnitFullArgv(
            string sourceFileName,
            IEnumerable<string> commandLineArgs,
            IEnumerable<UnsavedFile> unsavedFiles = null,
            TranslationUnitCreationOptions options = TranslationUnitCreationOptions.None)
        {
            ThrowIfDisposed();

            using (var cString = new CString(sourceFileName))
            using (var args = new CStrings(commandLineArgs))
            using (var files = new CXUnsavedFiles(unsavedFiles))
            {
                var argsPtr = stackalloc sbyte*[args.Count];
                args.Apply((arg, i) => argsPtr[i] = arg.Ptr);
                var filesPtr = stackalloc CXUnsavedFile[files.Count];
                files.Apply((file, i) => filesPtr[i] = files[i]);

                CXTranslationUnitImpl* ptr;
                NativeMethods.clang_parseTranslationUnit2FullArgv(
                    Ptr,
                    cString.Ptr,
                    argsPtr,
                    args.Count,
                    filesPtr,
                    (uint)files.Count,
                    (uint)options,
                    &ptr).Check();
                return new TranslationUnit(ptr, this);
            }
        }
    }
}
