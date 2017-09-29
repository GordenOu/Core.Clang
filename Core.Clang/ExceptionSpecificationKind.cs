namespace Core.Clang
{
    /// <summary>
    /// Describes the exception specification of a cursor.
    /// </summary>
    [EnumMapping(typeof(CXCursor_ExceptionSpecificationKind),
        Prefix = "CXCursor_ExceptionSpecificationKind_")]
    public enum ExceptionSpecificationKind
    {
        /// <summary>
        /// The cursor has no exception specification.
        /// </summary>
        None,

        /// <summary>
        /// The cursor has exception specification throw().
        /// </summary>
        DynamicNone,

        /// <summary>
        /// The cursor has exception specification throw(T1, T2).
        /// </summary>
        Dynamic,

        /// <summary>
        /// The cursor has exception specification throw(...).
        /// </summary>
        MSAny,

        /// <summary>
        /// The cursor has exception specification basic noexcept.
        /// </summary>
        BasicNoexcept,

        /// <summary>
        /// The cursor has exception specification computed noexcept.
        /// </summary>
        ComputedNoexcept,

        /// <summary>
        /// The exception specification has not yet been evaluated.
        /// </summary>
        Unevaluated,

        /// <summary>
        /// The exception specification has not yet been instantiated.
        /// </summary>
        Uninstantiated,

        /// <summary>
        /// The exception specification has not been parsed yet.
        /// </summary>
        Unparsed
    }
}
