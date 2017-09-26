namespace Core.Clang
{
    /// <summary>
    /// The kind of the evaluation result of a cursor.
    /// </summary>
    [EnumMapping(typeof(CXEvalResultKind),
        Prefix = "CXEval_",
        Excluded = new object[]
        {
            CXEvalResultKind.CXEval_ObjCStrLiteral,
            CXEvalResultKind.CXEval_CFStr
        })]
    public enum EvaluationResultKind
    {
        /// <summary>
        /// Integer.
        /// </summary>
        Int = 1,

        /// <summary>
        /// Floating point number.
        /// </summary>
        Float = 2,

        /// <summary>
        /// String literal.
        /// </summary>
        StringLiteral = 4,

        /// <summary>
        /// Other types with evaluated value returned.
        /// </summary>
        Other = 6,

        /// <summary>
        /// Unexposed result types.
        /// </summary>
        Unexposed = 0
    }
}
