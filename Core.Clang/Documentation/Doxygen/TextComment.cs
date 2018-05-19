using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Plain text. Inline content.
    /// </summary>
    public sealed class TextComment : InlineContentComment
    {
        internal TextComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.Text);
        }

        /// <summary>
        /// A <see cref="CommentKind.Paragraph"/> node is considered whitespace if it contains only
        /// <see cref="CommentKind.Text"/> nodes that are empty or whitespace.
        /// </summary>
        /// <returns>true if the Comment is whitespace.</returns>
        /// <remarks>
        /// Other AST nodes (except <see cref="CommentKind.Paragraph"/> and
        /// <see cref="CommentKind.Text"/>) are never considered whitespace.
        /// </remarks>
        bool IsWhiteSpace()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Comment_isWhitespace(Struct) != 0;
        }

        /// <summary>
        /// Gets the text contained in the AST node.
        /// </summary>
        /// <returns>Text contained in the AST node.</returns>
        public string GetText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_TextComment_getText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
