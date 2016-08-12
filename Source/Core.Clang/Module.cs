using System.ComponentModel;
using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Describes a module or submodule.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed unsafe class Module
    {
        internal CXModuleImpl* Ptr { get; }

        internal TranslationUnit TranslationUnit { get; }

        internal Module(CXModuleImpl* ptr, TranslationUnit translationUnit)
        {
            Debug.Assert(ptr != null);
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            Ptr = ptr;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Gets the module file where the provided module object came from.
        /// </summary>
        /// <returns>The module file where the provided module object came from.</returns>
        public SourceFile GetASTFile()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_Module_getASTFile(Ptr);
            return new SourceFile(ptr, TranslationUnit);
        }

        /// <summary>
        /// Gets the parent of a sub-module or null if the module is top-level.
        /// </summary>
        /// <returns>The parent of the module, of null if the module is top-level.</returns>
        public Module GetParant()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_Module_getParent(Ptr);
            return ptr == null ? null : new Module(ptr, TranslationUnit);
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <returns>The name of the module.</returns>
        public string GetName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_Module_getName(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the full name of the module.
        /// </summary>
        /// <returns>The full name of the module.</returns>
        public string GetFullName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_Module_getFullName(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Determines whether the module is a system module.
        /// </summary>
        /// <returns>true if the module is a system module.</returns>
        public bool IsSystem()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Module_isSystem(Ptr) != 0;
        }

        /// <summary>
        /// Gets the number of top level headers associated with this module.
        /// </summary>
        /// <returns>The number of top level headers associated with this module.</returns>
        public uint GetNumTopLevelHeaders()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Module_getNumTopLevelHeaders(TranslationUnit.Ptr, Ptr);
        }

        /// <summary>
        /// Gets the specified top level header associated with the module.
        /// </summary>
        /// <param name="index">Top level header index (zero-based).</param>
        /// <returns>The specified top level header associated with the module.</returns>
        public SourceFile GetTopLevelHeader(uint index)
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_Module_getTopLevelHeader(
                TranslationUnit.Ptr,
                Ptr,
                index);
            return ptr == null ? null : new SourceFile(ptr, TranslationUnit);
        }
    }
}
