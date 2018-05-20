using System;
using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Inline content (contained within a block).
    /// </summary>
    public abstract class InlineContentComment: Comment
    {
        internal InlineContentComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            switch (GetKind())
            {
                case CommentKind.Text:
                case CommentKind.InlineCommand:
                case CommentKind.HTMLStartTag:
                case CommentKind.HTMLEndTag:
                    break;
                default:
                    Debug.Fail("Unreachable.");
                    throw new ArgumentException(nameof(cxComment));
            }
        }

        /// <summary>
        /// Returns true if the Comment is inline content and has a newline immediately following
        /// it in the comment text. Newlines between paragraphs do not count.
        /// </summary>
        /// <returns>
        /// true if the Comment is inline content and has a newline immediately following it in the
        /// comment text.</returns>
        public bool HasTrailingNewLine()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_InlineContentComment_hasTrailingNewline(Struct) != 0;
        }
    }
}
