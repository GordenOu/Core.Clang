using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// A single paragraph that contains inline content.
    /// </summary>
    public sealed class ParamCommandComment : BlockContentComment
    {
        internal ParamCommandComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.ParamCommand);
        }

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        /// <returns>The parameter name.</returns>
        public string GetParamName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_ParamCommandComment_getParamName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the zero-based parameter index in function prototype.
        /// </summary>
        /// <returns>
        /// The zero-based parameter index in function prototype, or null if the parameter that
        /// this AST node represents was not found in the function prototype.
        /// </returns>
        public uint? GetParamIndex()
        {
            ThrowIfDisposed();

            if (NativeMethods.clang_ParamCommandComment_isParamIndexValid(Struct) == 0)
            {
                uint index = NativeMethods.clang_ParamCommandComment_getParamIndex(Struct);
                return index;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// True if parameter passing direction was specified explicitly in the comment.
        /// </summary>
        /// <returns>
        /// true if parameter passing direction was specified explicitly in the comment.
        /// </returns>
        public bool IsDirectionExplicit()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_ParamCommandComment_isDirectionExplicit(Struct) != 0;
        }

        /// <summary>
        /// Gets the parameter passing direction.
        /// </summary>
        /// <returns>The parameter passing direction.</returns>
        public CommentParamPassDirection GetDirection()
        {
            ThrowIfDisposed();

            var direction = NativeMethods.clang_ParamCommandComment_getDirection(Struct);
            return (CommentParamPassDirection)direction;
        }
    }
}
