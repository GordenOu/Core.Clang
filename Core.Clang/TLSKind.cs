namespace Core.Clang
{
    /// <summary>
    /// Describes the "thread-local storage (TLS) kind" of the declaration referred to by a cursor.
    /// </summary>
    [EnumMapping(typeof(CXTLSKind), Prefix = "CXTLS_")]
    public enum TLSKind
    {
        /// <summary>
        /// Not a TLS variable.
        /// </summary>
        None = 0,

        /// <summary>
        /// TLS with a dynamic initializer.
        /// </summary>
        Dynamic,

        /// <summary>
        /// TLS with a known-constant initializer.
        /// </summary>
        Static
    }
}
