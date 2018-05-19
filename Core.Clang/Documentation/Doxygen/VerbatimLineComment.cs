using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A verbatim line command. Verbatim line has an opening command, a single line of text (up to
    /// the newline after the opening command) and has no closing command.
    /// </summary>
    public sealed class VerbatimLineComment : BlockCommandComment
    {
        internal VerbatimLineComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.VerbatimLine);
        }

        /// <summary>
        /// Gets the text contained in the AST node.
        /// </summary>
        /// <returns>The text contained in the AST node.</returns>
        public string GetText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_VerbatimLineComment_getText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
