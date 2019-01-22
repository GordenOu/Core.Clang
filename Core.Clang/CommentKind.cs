namespace Core.Clang
{
    [EnumMapping(typeof(CXCommentKind), Prefix = "CXComment_")]
    public enum CommentKind
    {
        Null,
        Text,
        InlineCommand,
        HTMLStartTag,
        HTMLEndTag,
        Paragraph,
        BlockCommand,
        ParamCommand,
        TParamCommand,
        VerbatimBlockCommand,
        VerbatimBlockLine,
        VerbatimLine,
        FullComment,
    }
}
