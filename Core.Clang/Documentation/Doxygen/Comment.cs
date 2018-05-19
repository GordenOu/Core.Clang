using System;
using System.Diagnostics;
using Core.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A parsed comment.
    /// </summary>
    public abstract class Comment
    {
        internal CXComment Struct { get; }

        internal TranslationUnit TranslationUnit { get; }

        internal Comment(CXComment cxComment, TranslationUnit translationUnit)
        {
            Struct = cxComment;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Given a cursor that represents a documentable entity (e.g., declaration), returns the
        /// associated parsed comment as a <see cref="CommentKind.FullComment"/> AST node.
        /// </summary>
        /// <param name="cursor">
        /// A cursor that represents a documentable entity (e.g., declaration), or null if no AST
        /// node is constructed at the requested location because there is no text or a syntax
        /// error.
        /// </param>
        /// <returns>The associated parsed comment.</returns>
        public static Comment FromCursor(Cursor cursor)
        {
            Requires.NotNull(cursor, nameof(cursor));
            cursor.ThrowIfDisposed();

            var cxComment = NativeMethods.clang_Cursor_getParsedComment(cursor.Struct);
            var kind = NativeMethods.clang_Comment_getKind(cxComment);
            if (kind == CXCommentKind.CXComment_FullComment)
            {
                return new FullComment(cxComment, cursor.TranslationUnit);
            }
            else
            {
                Debug.Assert(kind == CXCommentKind.CXComment_Null);
                return null;
            }
        }

        /// <summary>
        /// Gets the type of the AST node.
        /// </summary>
        /// <returns>The type of the AST node.</returns>
        public CommentKind GetKind()
        {
            ThrowIfDisposed();

            return (CommentKind)NativeMethods.clang_Comment_getKind(Struct);
        }

        /// <summary>
        /// Gets the number of children of the AST node.
        /// </summary>
        /// <returns>The number of children of the AST node.</returns>
        public uint GetNumChildren()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Comment_getNumChildren(Struct);
        }

        /// <summary>
        /// Gets the specified child of the AST node.
        /// </summary>
        /// <param name="index">Child index (zero-based).</param>
        /// <returns>The specified child of the AST node.</returns>
        public Comment GetChild(uint index)
        {
            var cxComment = NativeMethods.clang_Comment_getChild(Struct, index);
            var kind = NativeMethods.clang_Comment_getKind(cxComment);
            switch (kind)
            {
                case CXCommentKind.CXComment_Text:
                    return new TextComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_InlineCommand:
                    return new InlineCommandComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_HTMLStartTag:
                    return new HTMLStartTagComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_HTMLEndTag:
                    return new HTMLEndTagComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_Paragraph:
                    return new ParagraphComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_BlockCommand:
                    return new BlockCommandComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_ParamCommand:
                    return new ParamCommandComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_TParamCommand:
                    return new TParamCommandComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_VerbatimBlockCommand:
                    return new VerbatimBlockComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_VerbatimBlockLine:
                    return new VerbatimBlockLineComment(cxComment, TranslationUnit);
                case CXCommentKind.CXComment_VerbatimLine:
                    return new VerbatimLineComment(cxComment, TranslationUnit);
                default:
                    Debug.Fail("Unreachable.");
                    throw new NotSupportedException(kind.ToString()); ;
            }
        }
    }
}
