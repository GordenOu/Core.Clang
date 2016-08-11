namespace Core.Clang
{
    /// <summary>
    /// Represents the storage classes as declared in the source. <see cref="Invalid"/> was added
    /// for the case that the passed cursor in not a declaration.
    /// </summary>
    [EnumMapping(typeof(CX_StorageClass),
        Prefix = "CX_SC_",
        Excluded = new object[]
        {
            CX_StorageClass.CX_SC_PrivateExtern,
            CX_StorageClass.CX_SC_OpenCLWorkGroupLocal
        })]
    public enum StorageClass
    {
        /// <summary>
        /// The passed cursor in not a declaration.
        /// </summary>
        Invalid,

        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// "extern".
        /// </summary>
        Extern,

        /// <summary>
        /// "static".
        /// </summary>
        Static,

        /// <summary>
        /// "auto".
        /// </summary>
        Auto = 6,

        /// <summary>
        /// "register".
        /// </summary>
        Register
    }
}
