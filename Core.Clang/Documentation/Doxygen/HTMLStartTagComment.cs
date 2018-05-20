using System.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// An opening HTML tag with attributes.
    /// </summary>
    public sealed class HTMLStartTagComment : HTMLTagComment
    {
        internal HTMLStartTagComment(CXComment cxComment, TranslationUnit translationUnit)
            : base(cxComment, translationUnit)
        {
            Debug.Assert(GetKind() == CommentKind.HTMLStartTag);
        }

        /// <summary>
        /// True if tag is self-closing (for example, &lt;br /&gt;).
        /// </summary>
        /// <returns>true if tag is self-closing.</returns>
        public bool IsSelfClosing()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_HTMLStartTagComment_isSelfClosing(Struct) != 0;
        }

        /// <summary>
        /// Gets the number of attributes (name-value pairs) attached to the start tag.
        /// </summary>
        /// <returns>
        /// The number of attributes (name-value pairs) attached to the start tag.
        /// </returns>
        public uint GetNumAttrs()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_HTMLStartTag_getNumAttrs(Struct);
        }

        /// <summary>
        /// Gets the name of the specified attribute.
        /// </summary>
        /// <param name="index">Attribute index (zero-based).</param>
        /// <returns>The name of the specified attribute.</returns>
        public string GetAttrName(uint index)
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_HTMLStartTag_getAttrName(Struct, index);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the value of the specified attribute.
        /// </summary>
        /// <param name="index">Attribute index (zero-based).</param>
        /// <returns>The value of the specified attribute.</returns>
        public string GetAttrValue(uint index)
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_HTMLStartTag_getAttrValue(Struct, index);
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
