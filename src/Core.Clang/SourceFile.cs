using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A particular source file that is part of a translation unit.
    /// </summary>
    public sealed unsafe class SourceFile
    {
        internal CXFileImpl* Ptr { get; }

        /// <summary>
        /// The <see cref="TranslationUnit"/> associated with the <see cref="SourceFile"/>.
        /// </summary>
        public TranslationUnit TranslationUnit { get; }

        internal SourceFile(CXFileImpl* ptr, TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            Debug.Assert(ptr != null);
            translationUnit.ThrowIfDisposed();

            Ptr = ptr;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Gets the complete file and path name of the given file.
        /// </summary>
        /// <returns>The complete file and path name.</returns>
        public string GetName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getFileName(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Determines whether the given header is guarded against multiple inclusions, either with
        /// the conventional #ifndef/#define/#endif macro guards or with #pragma once.
        /// </summary>
        /// <returns></returns>
        public bool IsMultipleIncludeGuarded()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isFileMultipleIncludeGuarded(TranslationUnit.Ptr, Ptr) != 0;
        }
    }
}
