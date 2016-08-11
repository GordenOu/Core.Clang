using System;

namespace Core.Clang
{
    /// <summary>
    /// Flags that control the creation of translation units.
    /// </summary>
    [EnumMapping(typeof(CXTranslationUnit_Flags),
        Prefix = "CXTranslationUnit_",
        Excluded = new object[]
        {
            CXTranslationUnit_Flags.CXTranslationUnit_CXXChainedPCH,
            CXTranslationUnit_Flags.CXTranslationUnit_IncludeBriefCommentsInCodeCompletion
        })]
    [Flags]
    public enum TranslationUnitCreationOptions
    {
        /// <summary>
        /// Used to indicate that no special translation-unit options are needed.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Used to indicate that the parser should construct a "detailed" preprocessing record,
        /// including all macro definitions and instantiations.
        /// </summary>
        /// <remarks>
        /// Constructing a detailed preprocessing record requires more memory and time to parse,
        /// since the information contained in the record is usually not retained. However, it can
        /// be useful for applications that require more detailed information about the behavior
        /// of the preprocessor.
        /// </remarks>
        DetailedPreprocessingRecord = 0x01,

        /// <summary>
        /// Used to indicate that the translation unit is incomplete.
        /// </summary>
        /// <remarks>
        /// When a translation unit is considered "incomplete", semantic analysis that is typically
        /// performed at the end of the translation unit will be suppressed. For example, this
        /// suppresses the completion of tentative declarations in C and of instantiation of
        /// implicitly-instantiation function templates in C++. This option is typically used when
        /// parsing a header with the intent of producing a precompiled header.
        /// </remarks>
        Incomplete = 0x02,

        /// <summary>
        /// Used to indicate that the translation unit should be built with an implicit precompiled
        /// header for the preamble.
        /// </summary>
        /// <remarks>
        /// An implicit precompiled header is used as an optimization when a particular translation
        /// unit is likely to be reparsed many times when the sources aren't changing that often.
        /// In this case, an implicit precompiled header will be built containing all of the
        /// initial includes at the top of the main file (what we refer to as the "preamble" of the
        /// file). In subsequent parses, if the preamble or the files in it have not changed,
        /// <see cref="TranslationUnit.TryReparse"/> will re-use the implicit precompiled header to
        /// improve parsing performance.
        /// </remarks>
        PrecompiledPreamble = 0x04,

        /// <summary>
        /// Used to indicate that the translation unit should cache some code-completion results
        /// with each reparse of the source file.
        /// </summary>
        /// <remarks>
        /// Caching of code-completion results is a performance optimization that introduces some
        /// overhead to reparsing but improves the performance of code-completion operations.
        /// </remarks>
        CacheCompletionResults = 0x08,

        /// <summary>
        /// Used to indicate that the translation unit will be serialized with
        /// <see cref="TranslationUnit.TrySave(string)"/>
        /// </summary>
        /// <remarks>
        /// This option is typically used when parsing a header with the intent of producing a
        /// precompiled header.
        /// </remarks>
        ForSerialization = 0x10,

        /// <summary>
        /// Used to indicate that function/method bodies should be skipped while parsing.
        /// </summary>
        /// <remarks>
        /// This option can be used to search for declarations/definitions while ignoring the
        /// usages.
        /// </remarks>
        SkipFunctionBodies = 0x40,

        /// <summary>
        /// Used to indicate that the precompiled preamble should be created on the first parse.
        /// Otherwise it will be created on the first reparse. This trades runtime on the first
        /// parse (serializing the preamble takes time) for reduced runtime on the second parse
        /// (can now reuse the preamble).
        /// </summary>
        CreatePreambleOnFirstParse = 0x100
    }
}
