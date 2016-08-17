using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Describes a single preprocessing token.
    /// </summary>
    public sealed unsafe class Token
    {
        internal CXToken Struct { get; }

        internal TranslationUnit TranslationUnit { get; }

        internal Token(CXToken cxToken, TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            Struct = cxToken;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Gets the kind of the token.
        /// </summary>
        /// <returns>The kind of the token.</returns>
        public TokenKind GetKind()
        {
            ThrowIfDisposed();

            return (TokenKind)NativeMethods.clang_getTokenKind(Struct);
        }

        /// <summary>
        /// Gets the spelling of the token.
        /// </summary>
        /// <returns>The spelling of the token.</returns>
        /// <remarks>
        /// The spelling of a token is the textual representation of that token, e.g., the text of
        /// an identifier or keyword.
        /// </remarks>
        public string GetSpelling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getTokenSpelling(TranslationUnit.Ptr, Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the source location of the token.
        /// </summary>
        /// <returns>The source location of the token.</returns>
        public SourceLocation GetLocation()
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getTokenLocation(
                TranslationUnit.Ptr,
                Struct);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets a source range that covers the token.
        /// </summary>
        /// <returns>A source range that covers the token.</returns>
        public SourceRange GetExtent()
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_getTokenExtent(
                TranslationUnit.Ptr,
                Struct);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }
    }
}
