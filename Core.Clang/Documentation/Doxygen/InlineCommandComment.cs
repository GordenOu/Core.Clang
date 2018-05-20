using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A command with word-like arguments that is considered inline content.
    /// </summary>
    public sealed class InlineCommandComment : InlineContentComment
    {
        internal InlineCommandComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.InlineCommand);
        }

        /// <summary>
        /// Gets the name of the inline command.
        /// </summary>
        /// <returns>The name of the inline command.</returns>
        public string GetCommandName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_InlineCommandComment_getCommandName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the most appropriate rendering mode, chosen on command semantics in Doxygen.
        /// </summary>
        /// <returns>
        /// The most appropriate rendering mode, chosen on command semantics in Doxygen.
        /// </returns>
        public CommentInlineCommandRenderKind GetRenderKind()
        {
            ThrowIfDisposed();

            var kind = NativeMethods.clang_InlineCommandComment_getRenderKind(Struct);
            return (CommentInlineCommandRenderKind)kind;
        }

        /// <summary>
        /// Gets the number of word-like arguments.
        /// </summary>
        /// <returns>The number of word-like arguments.</returns>
        public uint GetNumArgs()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_InlineCommandComment_getNumArgs(Struct);
        }

        /// <summary>
        /// Gets the text of the specified word-like argument.
        /// </summary>
        /// <param name="index">Argument index (zero-based).</param>
        /// <returns>The text of the specified word-like argument.</returns>
        public string GetArgText(uint index)
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_InlineCommandComment_getArgText(Struct, index);
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
