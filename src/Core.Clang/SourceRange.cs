using System;
using System.Diagnostics;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Identifies a half-open character range in the source code.
    /// </summary>
    /// <remarks>
    /// Use <see cref="GetStart"/> and <see cref="GetEnd"/> to retrieve the starting and end
    /// locations from a source range, respectively.
    /// </remarks>
    public sealed unsafe class SourceRange : IEquatable<SourceRange>
    {
        internal CXSourceRange Struct { get; }

        internal TranslationUnit TranslationUnit { get; }

        private SourceRange(CXSourceRange cxSourceRange, TranslationUnit translationUnit)
        {
            Struct = cxSourceRange;
            TranslationUnit = translationUnit;
        }

        internal static SourceRange Create(
            CXSourceRange cxSourceRange,
            TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            return NativeMethods.clang_Range_isNull(cxSourceRange) != 0
                ? null
                : new SourceRange(cxSourceRange, translationUnit);
        }

        /// <summary>
        /// Creates a source range given the beginning and ending source locations.
        /// </summary>
        /// <param name="begin">The beginning source location.</param>
        /// <param name="end">The ending source location.</param>
        /// <returns>The source range, or null if the range is invalid.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="begin"/> or <paramref name="end"/> is null.
        /// </exception>
        public static SourceRange Create(SourceLocation begin, SourceLocation end)
        {
            Requires.NotNull(begin, nameof(begin));
            Requires.NotNull(end, nameof(end));
            begin.ThrowIfDisposed();
            end.ThrowIfDisposed();

            if (!begin.SourceFile.Equals(end.SourceFile))
            {
                return null;
            }
            else
            {
                var cxSourceRange = NativeMethods.clang_getRange(begin.Struct, end.Struct);
                return Create(cxSourceRange, begin.SourceFile.TranslationUnit);
            }
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Determine whether the current instance is equal to another <see cref="SourceRange"/>
        /// object.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SourceRange"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// true if the current instance and the <paramref name="other"/> parameter represents the
        /// same source range.
        /// </returns>
        public bool Equals(SourceRange other)
        {
            ThrowIfDisposed();
            other?.ThrowIfDisposed();

            return other != null
                && NativeMethods.clang_equalRanges(Struct, other.Struct) != 0;
        }

        /// <summary>
        /// Gets a source location representing the first character within a source range.
        /// </summary>
        /// <returns>
        /// The source location representing the first character within a source range, or null if
        /// the range is invalid.
        /// </returns>
        public SourceLocation GetStart()
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getRangeStart(Struct);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets a source location representing the last character within a source range.
        /// </summary>
        /// <returns>
        /// The source location representing the last character within a source range, or null if
        /// the range is invalid.
        /// </returns>
        public SourceLocation GetEnd()
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getRangeEnd(Struct);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }
    }
}
