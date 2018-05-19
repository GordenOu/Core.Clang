using System;
using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Abstract class for opening and closing HTML tags. HTML tags are always treated as inline
    /// content (regardless HTML semantics).
    /// </summary>
    public abstract class HTMLTagComment : InlineContentComment
    {
        internal HTMLTagComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            switch (GetKind())
            {
                case CommentKind.HTMLStartTag:
                case CommentKind.HTMLEndTag:
                    break;
                default:
                    Debug.Fail("Unreachable.");
                    throw new ArgumentException(nameof(cxComment));
            }
        }

        /// <summary>
        /// Gets the HTML tag name.
        /// </summary>
        /// <returns>The HTML tag name.</returns>
        public string GetTagName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_HTMLTagComment_getTagName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Converts an HTML tag AST node to string.
        /// </summary>
        /// <returns>String containing an HTML tag.</returns>
        public string GetAsString()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_HTMLTagComment_getAsString(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
