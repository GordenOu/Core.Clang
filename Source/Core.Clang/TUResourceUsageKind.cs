namespace Core.Clang
{
    /// <summary>
    /// Categorizes how memory is being used by a translation unit.
    /// </summary>
    [EnumMapping(typeof(CXTUResourceUsageKind), Prefix = "CXTUResourceUsage_")]
    public enum TUResourceUsageKind
    {
        /// <summary>
        /// ASTContext: expressions, declarations, and types.
        /// </summary>
        AST = 1,

        /// <summary>
        /// ASTContext: identifiers.
        /// </summary>
        Identifiers = 2,

        /// <summary>
        /// ASTContext: selectors.
        /// </summary>
        Selectors = 3,

        /// <summary>
        /// Code completion: cached global results.
        /// </summary>
        GlobalCompletionResults = 4,

        /// <summary>
        /// SourceManager: content cache allocator.
        /// </summary>
        SourceManagerContentCache = 5,

        /// <summary>
        /// ASTContext: side tables.
        /// </summary>
        AST_SideTables = 6,

        /// <summary>
        /// SourceManager: malloc'ed memory buffers.
        /// </summary>
        SourceManager_Membuffer_Malloc = 7,

        /// <summary>
        /// SourceManager: mmap'ed memory buffers.
        /// </summary>
        SourceManager_Membuffer_MMap = 8,

        /// <summary>
        /// ExternalASTSource: malloc'ed memory buffers.
        /// </summary>
        ExternalASTSource_Membuffer_Malloc = 9,

        /// <summary>
        /// ExternalASTSource: mmap'ed memory buffers.
        /// </summary>
        ExternalASTSource_Membuffer_MMap = 10,

        /// <summary>
        /// Preprocessor: malloc'ed memory.
        /// </summary>
        Preprocessor = 11,

        /// <summary>
        /// Preprocessor: PreprocessingRecord.
        /// </summary>
        PreprocessingRecord = 12,

        /// <summary>
        /// SourceManager: data structures and tables.
        /// </summary>
        SourceManager_DataStructures = 13,

        /// <summary>
        /// Preprocessor: header search tables.
        /// </summary>
        Preprocessor_HeaderSearch = 14
    }
}
