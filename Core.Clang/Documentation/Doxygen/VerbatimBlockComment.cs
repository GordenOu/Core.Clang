using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A verbatim block command (e. g., preformatted code). Verbatim block has an opening and a
    /// closing command and contains multiple lines of text (<see cref="VerbatimBlockLineComment"/>
    /// nodes).
    /// </summary>
    public sealed class VerbatimBlockComment : BlockCommandComment
    {
        internal VerbatimBlockComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.VerbatimBlockCommand);
        }

        internal override void Accept(CommentVisitor visitor)
        {
            ThrowIfDisposed();

            visitor.Visit(this);
        }
    }
}
