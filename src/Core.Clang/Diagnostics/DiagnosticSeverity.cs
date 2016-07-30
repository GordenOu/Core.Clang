namespace Core.Clang.Diagnostics
{
    /// <summary>
    /// Describes the severity of a particular diagnostic.
    /// </summary>
    [EnumMapping(typeof(CXDiagnosticSeverity), Prefix = "CXDiagnostic_")]
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// A diagnostic that has been suppressed, e.g., by a command-line option.
        /// </summary>
        Ignored = 0,

        /// <summary>
        /// This diagnostic is a note that should be attached to the previous (non-note) diagnostic.
        /// </summary>
        Note = 1,

        /// <summary>
        /// This diagnostic indicates suspicious code that may not be wrong.
        /// </summary>
        Warning = 2,

        /// <summary>
        /// This diagnostic indicates that the code is ill-formed.
        /// </summary>
        Error = 3,

        /// <summary>
        /// This diagnostic indicates that the code is ill-formed such that future parser recovery
        /// is unlikely to produce useful results.
        /// </summary>
        Fatal = 4
    }
}
