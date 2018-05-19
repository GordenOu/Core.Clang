using System;
using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A single translation unit, which resides in an index.
    /// </summary>
    public unsafe class TargetInfo : IDisposable
    {
        internal CXTargetInfoImpl* Ptr { get; }

        internal TranslationUnit TranslationUnit { get; }

        internal TargetInfo(CXTargetInfoImpl* ptr, TranslationUnit translationUnit)
        {
            Debug.Assert(ptr != null);
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            Ptr = ptr;
            TranslationUnit = translationUnit;
        }

        private bool disposed;

        /// <summary>
        /// Destroys the <see cref="TargetInfo"/> object.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.clang_TargetInfo_dispose(Ptr);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroys the <see cref="TargetInfo"/> object.
        /// </summary>
        ~TargetInfo()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(TargetInfo).Name);
            }
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Gets the normalized target triple as a string.
        /// </summary>
        /// <returns>Empty string in case of any error.</returns>
        public string GetTriple()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_TargetInfo_getTriple(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Get the pointer width of the target in bits.
        /// </summary>
        /// <returns>-1 in case of error.</returns>
        public int GetPointerWidth()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_TargetInfo_getPointerWidth(Ptr);
        }
    }
}
