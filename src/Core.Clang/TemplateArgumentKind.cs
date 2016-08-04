namespace Core.Clang
{
    /// <summary>
    /// The kind of template argument.
    /// </summary>
    [EnumMapping(typeof(CXTemplateArgumentKind), Prefix = "CXTemplateArgumentKind_")]
    public enum TemplateArgumentKind
    {
        /// <summary>
        /// Represents an empty template argument, e.g., one that has not been deduced.
        /// </summary>
        Null,

        /// <summary>
        /// The template argument is a type.
        /// </summary>
        Type,

        /// <summary>
        /// The template argument is a declaration that was provided for a pointer, reference, or
        /// pointer to member non-type template parameter.
        /// </summary>
        Declaration,

        /// <summary>
        /// The template argument is a null pointer or null pointer to member that was provided for
        /// a non-type template parameter.
        /// </summary>
        NullPtr,

        /// <summary>
        /// The template argument is an integral value.
        /// </summary>
        Integral,

        /// <summary>
        /// The template argument is a template name that was provided for a template template
        /// parameter.
        /// </summary>
        Template,

        /// <summary>
        /// The template argument is a pack expansion of a template name that was provided for a
        /// template template parameter.
        /// </summary>
        TemplateExpansion,

        /// <summary>
        /// The template argument is an expression, and it has not been resolved to one of the
        /// other forms yet, either because it's dependent or because it is represented as a
        /// non-canonical template argument (for instance, in a TemplateSpecializationType). Also
        /// used to represent a non-dependent __uuidof expression (a Microsoft extension).
        /// </summary>
        Expression,

        /// <summary>
        /// The template argument is actually a parameter pack.
        /// </summary>
        Pack,

        /// <summary>
        /// Indicates an error case, preventing the kind from being deduced.
        /// </summary>
        Invalid
    }
}
