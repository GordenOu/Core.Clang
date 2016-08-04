namespace Core.Clang
{
    /// <summary>
    /// Describes the visibility of the entity referred to by a cursor.
    /// </summary>
    [EnumMapping(typeof(CXVisibilityKind), Prefix = "CXVisibility_")]
    public enum VisibilityKind
    {
        /// <summary>
        /// This value indicates that no visibility information is available for a provided
        /// <see cref="Cursor"/>.
        /// </summary>
        Invalid,

        /// <summary>
        /// Symbol not seen by the linker.
        /// </summary>
        Hidden,

        /// <summary>
        /// Symbol seen by the linker but resolves to a symbol inside this object.
        /// </summary>
        Protected,

        /// <summary>
        /// Symbol seen by the linker and acts like a normal symbol.
        /// </summary>
        Default
    }
}
