using System;

namespace Core.Clang
{
    /// <summary>
    /// See <see cref="Cursor.GetReferenceNameRange(NameReferenceFlags, uint)"/>.
    /// </summary>
    [EnumMapping(typeof(CXNameRefFlags), Prefix = "CXNameRange_")]
    [Flags]
    public enum NameReferenceFlags
    {
        /// <summary>
        /// Include the nested-name-specifier, e.g. Foo:: in x.Foo::y, in the range.
        /// </summary>
        WantQualifier = 0x1,

        /// <summary>
        /// Include the explicit template arguments, e.g. &lt;int&gt; in x.f&lt;int&gt;, in the
        /// range.
        /// </summary>
        WantTemplateArgs = 0x2,

        /// <summary>
        /// If the name is non-contiguous, return the full spanning range.
        /// </summary>
        /// <remarks>
        /// Non-contiguous names occur in C++ when using an operator:
        /// <code>
        /// return some_vector[1];
        /// </code>
        /// </remarks>
        WantSinglePiece = 0x4
    }
}
