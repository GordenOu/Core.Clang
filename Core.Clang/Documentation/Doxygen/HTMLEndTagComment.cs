using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A closing HTML tag.
    /// </summary>
    public sealed class HTMLEndTagComment: HTMLTagComment
    {
        internal HTMLEndTagComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.HTMLEndTag);
        }
    }
}
