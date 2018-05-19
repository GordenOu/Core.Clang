using System;
using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Block content (contains inline content).
    /// </summary>
    public abstract class BlockContentComment: Comment
    {
        internal BlockContentComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            switch (GetKind())
            {
                case CommentKind.Paragraph:
                case CommentKind.BlockCommand:
                case CommentKind.ParamCommand:
                case CommentKind.TParamCommand:
                case CommentKind.VerbatimBlockCommand:
                case CommentKind.VerbatimLine:
                    break;
                default:
                    Debug.Fail("Unreachable.");
                    throw new ArgumentException(nameof(cxComment));
            }
        }
    }
}
