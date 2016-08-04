namespace Core.Clang
{
    /// <summary>
    /// Lists the possible error codes for <see cref="TypeInfo.TryGetSizeOf(out long)"/>,
    /// <see cref="TypeInfo.TryGetAlignOf(out long)"/>,
    /// <see cref="TypeInfo.TryGetOffsetOf(string, out long)"/> and
    /// <see cref="Cursor.TryGetOffsetOfField(out long)"/>.
    /// </summary>
    [EnumMapping(typeof(CXTypeLayoutError), Prefix = "CXTypeLayoutError_")]
    public enum TypeLayoutError
    {
        /// <summary>
        /// Type is of kind <see cref="TypeKind.Invalid"/>.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// The type is an incomplete Type.
        /// </summary>
        Incomplete = -2,

        /// <summary>
        /// The type is a dependent Type.
        /// </summary>
        Dependent = -3,

        /// <summary>
        /// The type is not a constant size type.
        /// </summary>
        NotConstantSize = -4,

        /// <summary>
        /// The Field name is not valid for this record.
        /// </summary>
        InvalidFieldName = -5
    }
}
