using System;
using System.Diagnostics;
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
            file.ThrowIfDisposed();
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
    }
}
