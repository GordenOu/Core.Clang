using System;
using System.Diagnostics;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A single translation unit, which resides in an index.
    /// </summary>
    public unsafe class TranslationUnit : IDisposable
    {
        internal CXTranslationUnitImpl* Ptr { get; }

        /// <summary>
        /// The <see cref="Index"/> associated with the <see cref="TranslationUnit"/>.
        /// </summary>
        public Index Index { get; }

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
        /// Destroys the specified <see cref="TranslationUnit"/> object.
        /// </summary>
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
        /// Destroys the specified <see cref="TranslationUnit"/> object.
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
        /// Gets a source file within the translation unit.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>
        /// The <see cref="SourceFile"/> object for the named file in the translation unit, or null
        /// if the file was not a part of this translation unit.
        /// </returns>
        public SourceFile GetSourceFile(string fileName)
        {
            Requires.NotNullOrEmpty(fileName, nameof(fileName));
            ThrowIfDisposed();

            using (var cString = new CString(fileName))
            {
                var ptr = NativeMethods.clang_getFile(Ptr, cString.Ptr);
                return ptr == null ? null : new SourceFile(ptr, this);
            }
        }
    }
}
