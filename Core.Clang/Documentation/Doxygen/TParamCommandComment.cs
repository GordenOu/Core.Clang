using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Doxygen \tparam command, describes a template parameter.
    /// </summary>
    public sealed class TParamCommandComment : BlockCommandComment
    {
        internal TParamCommandComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.TParamCommand);
        }

        /// <summary>
        /// Gets the template parameter name.
        /// </summary>
        /// <returns>The template parameter name.</returns>
        public string GetParamName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_TParamCommandComment_getParamName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the zero-based nesting depth of this parameter in the template parameter list.
        /// </summary>
        /// <returns>
        /// The zero-based nesting depth of this parameter in the template parameter list, or null
        /// if the parameter that this AST node represents was not found in the template parameter
        /// list.
        /// </returns>
        public uint? GetDepth()
        {
            ThrowIfDisposed();

            if (NativeMethods.clang_TParamCommandComment_isParamPositionValid(Struct) != 0)
            {
                uint depth = NativeMethods.clang_TParamCommandComment_getDepth(Struct);
                return depth;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the zero-based parameter index in the template parameter list at a given nesting
        /// depth.
        /// </summary>
        /// <returns>
        /// The zero-based parameter index in the template parameter list at a given nesting
        /// depth, or null if the parameter that this AST node represents was not found in the
        /// template parameter list.
        /// </returns>
        public uint? GetIndex(uint depth)
        {
            ThrowIfDisposed();

            if (NativeMethods.clang_TParamCommandComment_isParamPositionValid(Struct) == 0)
            {
                uint index = NativeMethods.clang_TParamCommandComment_getIndex(Struct, depth);
                return index;
            }
            else
            {
                return null;
            }
        }

        internal override void Accept(CommentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
