using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Core.Diagnostics;
using Core.Linq;

namespace Core.Clang
{
    /// <summary>
    /// A single translation unit, which resides in an index.
    /// </summary>
    public unsafe class TranslationUnit : IDisposable
    {
        internal CXTranslationUnitImpl* Ptr { get; }

        internal Index Index { get; }

        internal TranslationUnit(CXTranslationUnitImpl* ptr, Index index)
        {
            Debug.Assert(ptr != null);
            Debug.Assert(index != null);
            index.ThrowIfDisposed();

            Ptr = ptr;
            Index = index;
        }

        private bool disposed;

        /// <summary>
        /// Destroys the <see cref="TranslationUnit"/> object.
        /// </summary>
        /// <remarks>
        /// This method invalidates the associated <see cref="Diagnostics.DiagnosticSet"/>.
        /// </remarks>
        [Unstable(version: "3.8.1", seealso: new[]
        {
            "https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CIndex.cpp"
        })]
        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.clang_disposeTranslationUnit(Ptr);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroys the <see cref="TranslationUnit"/> object.
        /// </summary>
        ~TranslationUnit()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(TranslationUnit).Name);
            }
            Index.ThrowIfDisposed();
        }

        /// <summary>
        /// Gets the set of flags that is suitable for parsing a translation unit that is being
        /// edited.
        /// </summary>
        /// <returns></returns>
        public static TranslationUnitCreationOptions GetDefaultEditingOptions()
        {
            uint options = NativeMethods.clang_defaultEditingTranslationUnitOptions();
            return (TranslationUnitCreationOptions)options;
        }

        /// <summary>
        /// Gets a source file within the translation unit.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>
        /// The <see cref="SourceFile"/> object for the named file in the translation unit, or null
        /// if the file was not a part of this translation unit.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="fileName"/> is empty.
        /// </exception>
        public SourceFile GetFile(string fileName)
        {
            Requires.NotNullOrEmpty(fileName, nameof(fileName));
            ThrowIfDisposed();

            using (var cString = new CString(fileName))
            {
                var ptr = NativeMethods.clang_getFile(Ptr, cString.Ptr);
                return ptr == null ? null : new SourceFile(ptr, this);
            }
        }

        /// <summary>
        /// Gets all ranges that were skipped by the preprocessor.
        /// </summary>
        /// <remarks>
        /// The preprocessor will skip lines when they are surrounded by an if/ifdef/ifndef
        /// directive whose condition does not evaluate to true.
        /// </remarks>
        /// <returns>All ranges that were skipped by the preprocessor.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file"/> is null.
        /// </exception>
        public SourceRange[] GetSkippedRanges(SourceFile file)
        {
            Requires.NotNull(file, nameof(file));
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_getSkippedRanges(Ptr, file.Ptr);
            if (ptr == null)
            {
                return Array.Empty<SourceRange>();
            }
            else
            {
                try
                {
                    var ranges = new SourceRange[ptr->count];
                    ranges.SetValues(i => SourceRange.Create(ptr->ranges[i], this));
                    return ranges;
                }
                finally
                {
                    NativeMethods.clang_disposeSourceRangeList(ptr);
                }
            }
        }

        /// <summary>
        /// Gets the original translation unit source file name.
        /// </summary>
        /// <returns>The original translation unit source file name.</returns>
        public string GetSpelling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getTranslationUnitSpelling(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Saves a translation unit into a serialized representation of that translation unit on
        /// disk.
        /// </summary>
        /// <param name="fileName">The file to which the translation unit will be saved.</param>
        /// <returns>
        /// <see cref="TranslationUnitSaveError.None"/> if the translation unit was saved
        /// successfully, otherwise a problem occurred.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="fileName"/> is empty.
        /// </exception>
        public TranslationUnitSaveError TrySave(string fileName)
        {
            Requires.NotNullOrEmpty(fileName, nameof(fileName));
            ThrowIfDisposed();

            using (var cString = new CString(fileName))
            {
                return (TranslationUnitSaveError)NativeMethods.clang_saveTranslationUnit(
                    Ptr,
                    cString.Ptr,
                    (uint)CXSaveTranslationUnit_Flags.CXSaveTranslationUnit_None);
            }
        }

        /// <summary>
        /// Reparse the source files that produced this translation unit.
        /// </summary>
        /// <param name="unsavedFiles">
        /// The files that have not yet been saved to disk but may be required for parsing,
        /// including the contents of those files.
        /// </param>
        /// <param name="translationUnit">
        /// The translation unit whose contents were re-parsed.
        /// </param>
        /// <returns>
        /// <see cref="ErrorCode.Success"/> if the sources could be reparsed. Another error code
        /// will be returned if reparsing was impossible, such that the translation unit is
        /// invalid. In such cases, the only valid call for <paramref name="translationUnit"/> is
        /// <see cref="Dispose"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This routine can be used to re-parse the source files that originally created the
        /// given translation unit, for example because those source files have changed (either on
        /// disk or as passed via <paramref name="unsavedFiles"/>). The source code will be
        /// reparsed with the same command-line options as it was originally parsed.
        /// </para>
        /// <para> 
        /// Reparsing a translation unit invalidates all cursors and source locations that refer
        /// into that translation unit. This makes reparsing a translation unit semantically
        /// equivalent to destroying the translation unit and then creating a new translation unit
        /// with the same command-line arguments. However, it may be more efficient to reparse a
        /// translation unit using this routine.
        /// </para>
        /// </remarks>
        public ErrorCode TryReparse(
            IEnumerable<UnsavedFile> unsavedFiles,
            out TranslationUnit translationUnit)
        {
            ThrowIfDisposed();

            var files = new CXUnsavedFiles(unsavedFiles);
            var filesPtr = stackalloc CXUnsavedFile[files.Count];
            files.Apply((file, i) => filesPtr[i] = files[i]);
            var errorCode = (ErrorCode)NativeMethods.clang_reparseTranslationUnit(
                Ptr,
                (uint)files.Count,
                filesPtr,
                NativeMethods.clang_defaultReparseOptions(Ptr));
            if (errorCode != ErrorCode.Success)
            {
                translationUnit = null;
                Dispose();
            }
            else
            {
                translationUnit = new TranslationUnit(Ptr, Index);
                disposed = true;
            }
            return errorCode;
        }

        /// <summary>
        /// Gets the memory usage of a translation unit.
        /// </summary>
        /// <returns>The memory usage of a translation unit.</returns>
        public TUResourceUsageEntry[] GetResourceUsage()
        {
            ThrowIfDisposed();

            var cxTUResourceUsage = NativeMethods.clang_getCXTUResourceUsage(Ptr);
            try
            {
                var entries = new TUResourceUsageEntry[cxTUResourceUsage.numEntries];
                entries.SetValues(
                    i => new TUResourceUsageEntry(
                        (TUResourceUsageKind)cxTUResourceUsage.entries[i].kind,
                        cxTUResourceUsage.entries[i].amount));
                return entries;
            }
            finally
            {
                NativeMethods.clang_disposeCXTUResourceUsage(cxTUResourceUsage);
            }
        }

        /// <summary>
        /// Gets the cursor that represents the translation unit.
        /// </summary>
        /// <returns>The cursor that represents the translation unit.</returns>
        /// <remarks>
        /// The translation unit cursor can be used to start traversing the various declarations
        /// within the translation unit.
        /// </remarks>
        public Cursor GetCursor()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getTranslationUnitCursor(Ptr);
            return Cursor.Create(cxCursor, this);
        }

        /// <summary>
        /// Maps a source location to the cursor that describes the entity at that location in the
        /// source code.
        /// </summary>
        /// <param name="location">The source location to be mapped.</param>
        /// <returns>
        /// A cursor representing the entity at the given source location, or null if no such
        /// entity can be found.
        /// </returns>
        /// <remarks>
        /// <see cref="GetCursor(SourceLocation)"/> maps an arbitrary source location within a
        /// translation unit down to the most specific cursor that describes the entity at that
        /// location. For example, given an expression <c>x + y</c>, invoking
        /// <see cref="GetCursor(SourceLocation)"/> with a source location pointing to "x" will
        /// return the cursor for "x"; similarly for "y". If the cursor points anywhere between "x"
        /// or "y" (e.g., on the + or the whitespace around it),
        /// <see cref="GetCursor(SourceLocation)"/> will return a cursor referring to the "+"
        /// expression.
        /// </remarks>
        public Cursor GetCursor(SourceLocation location)
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCursor(Ptr, location.Struct);
            return Cursor.Create(cxCursor, this);
        }

        /// <summary>
        /// Tokenizes the source code described by the given range into raw lexical tokens.
        /// </summary>
        /// <param name="range">
        /// The source range in which text should be tokenized. All of the tokens produced by
        /// tokenization will fall within this source range.
        /// </param>
        /// <returns>
        /// The array of tokens that occur within the given source range.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="range"/> is null.
        /// </exception>
        public Token[] Tokenize(SourceRange range)
        {
            Requires.NotNull(range, nameof(range));
            ThrowIfDisposed();

            CXToken* cxTokens;
            uint numTokens;
            NativeMethods.clang_tokenize(Ptr, range.Struct, &cxTokens, &numTokens);
            try
            {
                var tokens = new Token[numTokens];
                for (int i = 0; i < tokens.Length; i++)
                {
                    tokens[i] = new Token(cxTokens[i], this);
                }
                return tokens;
            }
            finally
            {
                if (cxTokens != null)
                {
                    NativeMethods.clang_disposeTokens(Ptr, cxTokens, numTokens);
                }
            }
        }

        /// <summary>
        /// Annotates the given set of tokens by providing cursors for each token that can be
        /// mapped to a specific entity within the abstract syntax tree.
        /// </summary>
        /// <param name="tokens">The set of tokens to annotate.</param>
        /// <returns>
        /// An array of cursors, whose contents will be replaced with the cursors corresponding to
        /// each token.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tokens"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="tokens"/> has null items.
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Cursor[] AnnotateTokens(IEnumerable<Token> tokens)
        {
            Requires.NotNull(tokens, nameof(tokens));
            var array = tokens.ToArray();
            Requires.NotNullItems(array, nameof(tokens));
            ThrowIfDisposed();

            int numTokens = array.Length;
            var cxTokens = stackalloc CXToken[numTokens];
            array.Apply((token, i) => cxTokens[i] = token.Struct);
            var cxCursors = stackalloc CXCursor[numTokens];
            NativeMethods.clang_annotateTokens(Ptr, cxTokens, (uint)numTokens, cxCursors);
            var cursors = new Cursor[numTokens];
            cursors.SetValues(i => Cursor.Create(cxCursors[i], this));
            return cursors;
        }
    }
}
