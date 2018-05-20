using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A single paragraph that contains inline content.
    /// </summary>
    public sealed class ParagraphComment: BlockContentComment
    {
        internal ParagraphComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.Paragraph);
        }

        internal override void Accept(CommentVisitor visitor)
        {
            visitor.Visit(this);
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
        public bool IsWhiteSpace()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Comment_isWhitespace(Struct) != 0;
        }
    }
}
