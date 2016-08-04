namespace Core.Clang
{
    /// <summary>
    /// Represents the C++ access control level to a base class for a cursor with kind
    /// <see cref="CursorKind.CXXBaseSpecifier"/>.
    /// </summary>
    [EnumMapping(typeof(CX_CXXAccessSpecifier), Prefix = "CX_CXX")]
    public enum CXXAccessSpecifier
    {
        /// <summary>
        /// Invalid access specifier.
        /// </summary>
        InvalidAccessSpecifier,

        /// <summary>
        /// "public".
        /// </summary>
        Public,

        /// <summary>
        /// "protected".
        /// </summary>
        Protected,

        /// <summary>
        /// "private".
        /// </summary>
        Private
    }
}
