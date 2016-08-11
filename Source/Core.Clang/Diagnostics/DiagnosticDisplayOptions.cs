using System;

namespace Core.Clang.Diagnostics
{
    /// <summary>
    /// Options to control the display of diagnostics.
    /// </summary>
    /// <remarks>
    /// The values in this enum are meant to be combined to customize the behavior of
    /// <see cref="Diagnostic.ToString(DiagnosticDisplayOptions)"/>.
    /// </remarks>
    [EnumMapping(typeof(CXDiagnosticDisplayOptions), Prefix = "CXDiagnostic_")]
    [Flags]
    public enum DiagnosticDisplayOptions : uint
    {
        /// <summary>
        /// Display the source-location information where the diagnostic was located.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When set, diagnostics will be prefixed by the file, line, and (optionally) column to
        /// which the diagnostic refers. For example,
        /// </para>
        /// <code>
        /// test.c:28: warning: extra tokens at end of #endif directive
        /// </code>
        /// <para>
        /// This option corresponds to the clang flag -fshow-source-location.
        /// </para>
        /// </remarks>
        DisplaySourceLocation = 0x01,

        /// <summary>
        /// If displaying the source-location information of the diagnostic, also include the
        /// column number.
        /// </summary>
        /// <remarks>
        /// This option corresponds to the clang flag -fshow-column.
        /// </remarks>
        DisplayColumn = 0x02,

        /// <summary>
        /// If displaying the source-location information of the diagnostic, also include
        /// information about source ranges in a machine-parsable format.
        /// </summary>
        /// <remarks>
        /// This option corresponds to the clang flag -fdiagnostics-print-source-range-info.
        /// </remarks>
        DisplaySourceRanges = 0x04,

        /// <summary>
        /// Display the option name associated with this diagnostic, if any.
        /// </summary>
        /// <remarks>
        /// The option name displayed (e.g., -Wconversion) will be placed in brackets after the
        /// diagnostic text. This option corresponds to the clang flag -fdiagnostics-show-option.
        /// </remarks>
        DisplayOption = 0x08,

        /// <summary>
        /// Display the category number associated with this diagnostic, if any.
        /// </summary>
        /// <remarks>The category number is displayed within brackets after the diagnostic text.
        /// This option corresponds to the clang flag -fdiagnostics-show-category = id.
        /// </remarks>
        DisplayCategoryId = 0x10,

        /// <summary>
        /// Display the category name associated with this diagnostic, if any.
        /// </summary>
        /// <remarks>
        /// The category name is displayed within brackets after the diagnostic text. This option
        /// corresponds to the clang flag -fdiagnostics-show-category = name.
        /// </remarks>
        DisplayCategoryName = 0x20
    }
}
