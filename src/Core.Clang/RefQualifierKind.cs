namespace Core.Clang
{
    /// <summary>
    /// The ref-qualifier kind of a function or method.
    /// </summary>
    [EnumMapping(typeof(CXRefQualifierKind), Prefix = "CXRefQualifier_")]
    public enum RefQualifierKind
    {
        /// <summary>
        /// No ref-qualifier was provided.
        /// </summary>
        None = 0,

        /// <summary>
        /// An lvalue ref-qualifier was provided (<c>&amp;</c>).
        /// </summary>
        LValue,

        /// <summary>
        /// An rvalue ref-qualifier was provided (<c>&amp;&amp;</c>).
        /// </summary>
        RValue
    }
}
