using Core.Clang.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Describes the kind of error that occurred (if any) in a call to
    /// <see cref="TranslationUnit.TrySave(string)"/>.
    /// </summary>
    [EnumMapping(typeof(CXSaveError), Prefix = "CXSaveError_")]
    public enum TranslationUnitSaveError
    {
        /// <summary>
        /// Indicates that no error occurred while saving a translation unit.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that an unknown error occurred while attempting to save the file.
        /// </summary>
        /// <remarks>
        /// This error typically indicates that file I/O failed when attempting to write the file.
        /// </remarks>
        Unknown = 1,

        /// <summary>
        /// Indicates that errors during translation prevented this attempt to save the translation
        /// unit.
        /// </summary>
        TranslationErrors = 2,

        /// <summary>
        /// Errors that prevent the translation unit from being saved can be extracted using
        /// <see cref="DiagnosticSet.FromTranslationUnit(TranslationUnit)"/>.
        /// </summary>
        InvalidTU = 3
    }
}
