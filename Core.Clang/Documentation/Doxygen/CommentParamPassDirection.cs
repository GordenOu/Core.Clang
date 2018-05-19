namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Describes parameter passing direction for \param or \arg command.
    /// </summary>
    [EnumMapping(typeof(CXCommentParamPassDirection), Prefix = "CXCommentParamPassDirection_")]
    public enum CommentParamPassDirection
    {
        /// <summary>
        /// The parameter is an input parameter.
        /// </summary>
        In,

        /// <summary>
        /// The parameter is an output parameter.
        /// </summary>
        Out,

        /// <summary>
        /// The parameter is an input and output parameter.
        /// </summary>
        InOut
    }
}
