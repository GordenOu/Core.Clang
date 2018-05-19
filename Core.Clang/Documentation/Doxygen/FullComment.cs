using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A full comment attached to a declaration, contains block content.
    /// </summary>
    public sealed class FullComment : Comment
    {
        internal FullComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.FullComment);
        }

        /// <summary>
        /// Converts the given full parsed comment to an HTML fragment.
        /// </summary>
        /// <returns>
        /// String containing an HTML fragment.
        /// </returns>
        public string GetAsHTML()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_FullComment_getAsHTML(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Converts the given full parsed comment to an XML document.
        /// </summary>
        /// <returns>
        /// String containing an XML document.
        /// </returns>
        public string GetAsXML()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_FullComment_getAsXML(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
