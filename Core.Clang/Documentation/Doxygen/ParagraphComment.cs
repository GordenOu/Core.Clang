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
    }
}
