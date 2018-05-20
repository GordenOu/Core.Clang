using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A line of text contained in a verbatim block.
    /// </summary>
    public sealed class VerbatimBlockLineComment : Comment
    {
        internal VerbatimBlockLineComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.VerbatimBlockLine);
        }

        /// <summary>
        /// Gets the text contained in the AST node.
        /// </summary>
        /// <returns>The text contained in the AST node.</returns>
        public string GetText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_VerbatimBlockLineComment_getText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        internal override void Accept(CommentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
