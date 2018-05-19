using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A command that has zero or more word-like arguments (number of word-like arguments depends
    /// on command name) and a paragraph as an argument (e. g., \brief).
    /// </summary>
    public class BlockCommandComment: BlockContentComment
    {
        internal BlockCommandComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            switch (GetKind())
            {
                case CommentKind.BlockCommand:
                case CommentKind.ParamCommand:
                case CommentKind.TParamCommand:
                case CommentKind.VerbatimBlockCommand:
                case CommentKind.VerbatimLine:
                    break;
                default:
                    Debug.Fail("Unreachable.");
                    break;
            }
        }

        /// <summary>
        /// Gets the name of the block command.
        /// </summary>
        /// <returns>The name of the block command.</returns>
        public string GetCommandName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_BlockCommandComment_getCommandName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the number of word-like arguments.
        /// </summary>
        /// <returns>The number of word-like arguments.</returns>
        public uint GetNumArgs()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_BlockCommandComment_getNumArgs(Struct);
        }

        /// <summary>
        /// Gets the text of the specified word-like argument.
        /// </summary>
        /// <param name="index">Argument index (zero-based).</param>
        /// <returns>The text of the specified word-like argument.</returns>
        public string GetArgText(uint index)
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_BlockCommandComment_getArgText(Struct, index);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the paragraph argument of the block command.
        /// </summary>
        /// <returns>The paragraph argument of the block command.</returns>
        public ParagraphComment GetParagraph()
        {
            ThrowIfDisposed();

            var cxComment = NativeMethods.clang_BlockCommandComment_getParagraph(Struct);
            return new ParagraphComment(cxComment, TranslationUnit);
        }
    }
}
