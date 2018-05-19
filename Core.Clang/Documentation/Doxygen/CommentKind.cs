namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Describes the type of the comment AST node (\c CXComment).  A comment node can be
    /// considered block content(e.g., paragraph), inline content (plain text) or neither (the root
    /// AST node).
    /// </summary>
    [EnumMapping(
        typeof(CXCommentKind),
        Prefix = "CXComment_",
        Excluded = new object[] { CXCommentKind.CXComment_Null })]
    public enum CommentKind
    {
        /// <summary>
        /// Plain text. Inline content.
        /// </summary>
        Text = 1,

        /// <summary>
        /// A command with word-like arguments that is considered inline content.
        /// </summary>
        /// <example>\c command</example>
        InlineCommand = 2,

        /// <summary>
        /// HTML start tag with attributes (name-value pairs). Considered inline content.
        /// </summary>
        /// <example>
        /// &lt;br&gt; &lt;br /&gt; &lt;a href="http://example.org/"&gt;
        /// </example>
        HTMLStartTag = 3,

        /// <summary>
        /// HTML end tag. Considered inline content.
        /// </summary>
        /// <example>
        /// &lt;/a&gt;
        /// </example>
        HTMLEndTag = 4,

        /// <summary>
        /// A paragraph, contains inline comment. The paragraph itself is block content.
        /// </summary>
        Paragraph = 5,

        /// <summary>
        /// A command that has zero or more word-like arguments (number of word-like arguments
        /// depends on command name) and a paragraph as an argument. Block command is block
        /// content.
        /// <para>Paragraph argument is also a child of the block command.</para>
        /// <para>For example: \brief has 0 word-like arguments and a paragraph argument.</para>
        /// </summary>
        /// <remarks>
        /// AST nodes of special kinds that parser knows about (e. g., \param command) have their
        /// own node kinds.
        /// </remarks>
        BlockCommand = 6,

        /// <summary>
        /// A \param or \arg command that describes the function parameter (name, passing
        /// direction, description).
        /// </summary>
        /// <example>\param [in] ParamName description</example>
        ParamCommand = 7,

        /// <summary>
        /// A \tparam command that describes a template parameter (name and description).
        /// </summary>
        /// <example>\tparam T description.</example>
        TParamCommand = 8,

        /// <summary>
        /// A verbatim block command (e. g., preformatted code). Verbatim block has an opening and
        /// a closing command and contains multiple lines of text (<see cref="VerbatimBlockLine"/>
        /// child nodes).
        /// </summary>
        /// <example>
        /// \verbatim
        /// aaa
        /// \endverbatim
        /// </example>
        VerbatimBlockCommand = 9,

        /// <summary>
        /// A line of text that is contained within a <see cref="VerbatimBlockCommand"/> node.
        /// </summary>
        VerbatimBlockLine = 10,

        /// <summary>
        /// A verbatim line command.  Verbatim line has an opening command, a single line of text
        /// (up to the newline after the opening command) and has no closing command.
        /// </summary>
        VerbatimLine = 11,

        /// <summary>
        /// A full comment attached to a declaration, contains block content.
        /// </summary>
        FullComment = 12
    }
}
