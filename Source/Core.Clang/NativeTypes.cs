using System;
using System.Runtime.InteropServices;

namespace Core.Clang
{
    internal enum CXErrorCode
    {
        CXError_Success = 0,
        CXError_Failure = 1,
        CXError_Crashed = 2,
        CXError_InvalidArguments = 3,
        CXError_ASTReadError = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXString
    {
        public void* data;
        public uint private_flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXStringSet
    {
        public CXString* Strings;
        public uint Count;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXVirtualFileOverlayImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXModuleMapDescriptorImpl { }

    internal struct CXIndexImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXTranslationUnitImpl { }

    internal struct CXClientDataImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXUnsavedFile
    {
        public sbyte* Filename;
        public sbyte* Contents;
        public ulong Length;
    }

    internal enum CXAvailabilityKind
    {
        CXAvailability_Available,
        CXAvailability_Deprecated,
        CXAvailability_NotAvailable,
        CXAvailability_NotAccessible
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXVersion
    {
        public int Major;
        public int Minor;
        public int Subminor;
    }

    internal enum CXGlobalOptFlags
    {
        CXGlobalOpt_None = 0x0,
        CXGlobalOpt_ThreadBackgroundPriorityForIndexing = 0x1,
        CXGlobalOpt_ThreadBackgroundPriorityForEditing = 0x2,
        CXGlobalOpt_ThreadBackgroundPriorityForAll =
            CXGlobalOpt_ThreadBackgroundPriorityForIndexing |
            CXGlobalOpt_ThreadBackgroundPriorityForEditing
    }

    internal struct CXFileImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXFileUniqueID
    {
        public fixed ulong data[3];
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXSourceLocation
    {
        public fixed ulong ptr_data[2];
        public uint int_data;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXSourceRange
    {
        public fixed ulong ptr_data[2];
        public uint begin_int_data;
        public uint end_int_data;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXSourceRangeList
    {
        public uint count;
        public CXSourceRange* ranges;
    }

    internal enum CXDiagnosticSeverity
    {
        CXDiagnostic_Ignored = 0,
        CXDiagnostic_Note = 1,
        CXDiagnostic_Warning = 2,
        CXDiagnostic_Error = 3,
        CXDiagnostic_Fatal = 4
    }

    internal struct CXDiagnosticImpl { }

    internal struct CXDiagnosticSetImpl { }

    internal enum CXLoadDiag_Error
    {
        CXLoadDiag_None = 0,
        CXLoadDiag_Unknown = 1,
        CXLoadDiag_CannotLoad = 2,
        CXLoadDiag_InvalidFile = 3
    }

    internal enum CXDiagnosticDisplayOptions
    {
        CXDiagnostic_DisplaySourceLocation = 0x01,
        CXDiagnostic_DisplayColumn = 0x02,
        CXDiagnostic_DisplaySourceRanges = 0x04,
        CXDiagnostic_DisplayOption = 0x08,
        CXDiagnostic_DisplayCategoryId = 0x10,
        CXDiagnostic_DisplayCategoryName = 0x20
    }

    internal enum CXTranslationUnit_Flags
    {
        CXTranslationUnit_None = 0x0,
        CXTranslationUnit_DetailedPreprocessingRecord = 0x01,
        CXTranslationUnit_Incomplete = 0x02,
        CXTranslationUnit_PrecompiledPreamble = 0x04,
        CXTranslationUnit_CacheCompletionResults = 0x08,
        CXTranslationUnit_ForSerialization = 0x10,
        CXTranslationUnit_CXXChainedPCH = 0x20,
        CXTranslationUnit_SkipFunctionBodies = 0x40,
        CXTranslationUnit_IncludeBriefCommentsInCodeCompletion = 0x80,
        CXTranslationUnit_CreatePreambleOnFirstParse = 0x100,
        CXTranslationUnit_KeepGoing = 0x200
    }

    internal enum CXSaveTranslationUnit_Flags
    {
        CXSaveTranslationUnit_None = 0x0
    }

    internal enum CXSaveError
    {
        CXSaveError_None = 0,
        CXSaveError_Unknown = 1,
        CXSaveError_TranslationErrors = 2,
        CXSaveError_InvalidTU = 3
    }

    internal enum CXReparse_Flags
    {
        CXReparse_None = 0x0
    }

    internal enum CXTUResourceUsageKind
    {
        CXTUResourceUsage_AST = 1,
        CXTUResourceUsage_Identifiers = 2,
        CXTUResourceUsage_Selectors = 3,
        CXTUResourceUsage_GlobalCompletionResults = 4,
        CXTUResourceUsage_SourceManagerContentCache = 5,
        CXTUResourceUsage_AST_SideTables = 6,
        CXTUResourceUsage_SourceManager_Membuffer_Malloc = 7,
        CXTUResourceUsage_SourceManager_Membuffer_MMap = 8,
        CXTUResourceUsage_ExternalASTSource_Membuffer_Malloc = 9,
        CXTUResourceUsage_ExternalASTSource_Membuffer_MMap = 10,
        CXTUResourceUsage_Preprocessor = 11,
        CXTUResourceUsage_PreprocessingRecord = 12,
        CXTUResourceUsage_SourceManager_DataStructures = 13,
        CXTUResourceUsage_Preprocessor_HeaderSearch = 14,
        CXTUResourceUsage_MEMORY_IN_BYTES_BEGIN = CXTUResourceUsage_AST,
        CXTUResourceUsage_MEMORY_IN_BYTES_END = CXTUResourceUsage_Preprocessor_HeaderSearch,
        CXTUResourceUsage_First = CXTUResourceUsage_AST,
        CXTUResourceUsage_Last = CXTUResourceUsage_Preprocessor_HeaderSearch
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXTUResourceUsageEntry
    {
        public CXTUResourceUsageKind kind;
        public ulong amount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXTUResourceUsage
    {
        public void* data;
        public uint numEntries;
        public CXTUResourceUsageEntry* entries;
    }

    internal enum CXCursorKind
    {
        CXCursor_UnexposedDecl = 1,
        CXCursor_StructDecl = 2,
        CXCursor_UnionDecl = 3,
        CXCursor_ClassDecl = 4,
        CXCursor_EnumDecl = 5,
        CXCursor_FieldDecl = 6,
        CXCursor_EnumConstantDecl = 7,
        CXCursor_FunctionDecl = 8,
        CXCursor_VarDecl = 9,
        CXCursor_ParmDecl = 10,
        CXCursor_ObjCInterfaceDecl = 11,
        CXCursor_ObjCCategoryDecl = 12,
        CXCursor_ObjCProtocolDecl = 13,
        CXCursor_ObjCPropertyDecl = 14,
        CXCursor_ObjCIvarDecl = 15,
        CXCursor_ObjCInstanceMethodDecl = 16,
        CXCursor_ObjCClassMethodDecl = 17,
        CXCursor_ObjCImplementationDecl = 18,
        CXCursor_ObjCCategoryImplDecl = 19,
        CXCursor_TypedefDecl = 20,
        CXCursor_CXXMethod = 21,
        CXCursor_Namespace = 22,
        CXCursor_LinkageSpec = 23,
        CXCursor_Constructor = 24,
        CXCursor_Destructor = 25,
        CXCursor_ConversionFunction = 26,
        CXCursor_TemplateTypeParameter = 27,
        CXCursor_NonTypeTemplateParameter = 28,
        CXCursor_TemplateTemplateParameter = 29,
        CXCursor_FunctionTemplate = 30,
        CXCursor_ClassTemplate = 31,
        CXCursor_ClassTemplatePartialSpecialization = 32,
        CXCursor_NamespaceAlias = 33,
        CXCursor_UsingDirective = 34,
        CXCursor_UsingDeclaration = 35,
        CXCursor_TypeAliasDecl = 36,
        CXCursor_ObjCSynthesizeDecl = 37,
        CXCursor_ObjCDynamicDecl = 38,
        CXCursor_CXXAccessSpecifier = 39,
        CXCursor_FirstDecl = CXCursor_UnexposedDecl,
        CXCursor_LastDecl = CXCursor_CXXAccessSpecifier,
        CXCursor_FirstRef = 40,
        CXCursor_ObjCSuperClassRef = 40,
        CXCursor_ObjCProtocolRef = 41,
        CXCursor_ObjCClassRef = 42,
        CXCursor_TypeRef = 43,
        CXCursor_CXXBaseSpecifier = 44,
        CXCursor_TemplateRef = 45,
        CXCursor_NamespaceRef = 46,
        CXCursor_MemberRef = 47,
        CXCursor_LabelRef = 48,
        CXCursor_OverloadedDeclRef = 49,
        CXCursor_VariableRef = 50,
        CXCursor_LastRef = CXCursor_VariableRef,
        CXCursor_FirstInvalid = 70,
        CXCursor_InvalidFile = 70,
        CXCursor_NoDeclFound = 71,
        CXCursor_NotImplemented = 72,
        CXCursor_InvalidCode = 73,
        CXCursor_LastInvalid = CXCursor_InvalidCode,
        CXCursor_FirstExpr = 100,
        CXCursor_UnexposedExpr = 100,
        CXCursor_DeclRefExpr = 101,
        CXCursor_MemberRefExpr = 102,
        CXCursor_CallExpr = 103,
        CXCursor_ObjCMessageExpr = 104,
        CXCursor_BlockExpr = 105,
        CXCursor_IntegerLiteral = 106,
        CXCursor_FloatingLiteral = 107,
        CXCursor_ImaginaryLiteral = 108,
        CXCursor_StringLiteral = 109,
        CXCursor_CharacterLiteral = 110,
        CXCursor_ParenExpr = 111,
        CXCursor_UnaryOperator = 112,
        CXCursor_ArraySubscriptExpr = 113,
        CXCursor_BinaryOperator = 114,
        CXCursor_CompoundAssignOperator = 115,
        CXCursor_ConditionalOperator = 116,
        CXCursor_CStyleCastExpr = 117,
        CXCursor_CompoundLiteralExpr = 118,
        CXCursor_InitListExpr = 119,
        CXCursor_AddrLabelExpr = 120,
        CXCursor_StmtExpr = 121,
        CXCursor_GenericSelectionExpr = 122,
        CXCursor_GNUNullExpr = 123,
        CXCursor_CXXStaticCastExpr = 124,
        CXCursor_CXXDynamicCastExpr = 125,
        CXCursor_CXXReinterpretCastExpr = 126,
        CXCursor_CXXConstCastExpr = 127,
        CXCursor_CXXFunctionalCastExpr = 128,
        CXCursor_CXXTypeidExpr = 129,
        CXCursor_CXXBoolLiteralExpr = 130,
        CXCursor_CXXNullPtrLiteralExpr = 131,
        CXCursor_CXXThisExpr = 132,
        CXCursor_CXXThrowExpr = 133,
        CXCursor_CXXNewExpr = 134,
        CXCursor_CXXDeleteExpr = 135,
        CXCursor_UnaryExpr = 136,
        CXCursor_ObjCStringLiteral = 137,
        CXCursor_ObjCEncodeExpr = 138,
        CXCursor_ObjCSelectorExpr = 139,
        CXCursor_ObjCProtocolExpr = 140,
        CXCursor_ObjCBridgedCastExpr = 141,
        CXCursor_PackExpansionExpr = 142,
        CXCursor_SizeOfPackExpr = 143,
        CXCursor_LambdaExpr = 144,
        CXCursor_ObjCBoolLiteralExpr = 145,
        CXCursor_ObjCSelfExpr = 146,
        CXCursor_OMPArraySectionExpr = 147,
        CXCursor_ObjCAvailabilityCheckExpr = 148,
        CXCursor_LastExpr = CXCursor_ObjCAvailabilityCheckExpr,
        CXCursor_FirstStmt = 200,
        CXCursor_UnexposedStmt = 200,
        CXCursor_LabelStmt = 201,
        CXCursor_CompoundStmt = 202,
        CXCursor_CaseStmt = 203,
        CXCursor_DefaultStmt = 204,
        CXCursor_IfStmt = 205,
        CXCursor_SwitchStmt = 206,
        CXCursor_WhileStmt = 207,
        CXCursor_DoStmt = 208,
        CXCursor_ForStmt = 209,
        CXCursor_GotoStmt = 210,
        CXCursor_IndirectGotoStmt = 211,
        CXCursor_ContinueStmt = 212,
        CXCursor_BreakStmt = 213,
        CXCursor_ReturnStmt = 214,
        CXCursor_GCCAsmStmt = 215,
        CXCursor_AsmStmt = CXCursor_GCCAsmStmt,
        CXCursor_ObjCAtTryStmt = 216,
        CXCursor_ObjCAtCatchStmt = 217,
        CXCursor_ObjCAtFinallyStmt = 218,
        CXCursor_ObjCAtThrowStmt = 219,
        CXCursor_ObjCAtSynchronizedStmt = 220,
        CXCursor_ObjCAutoreleasePoolStmt = 221,
        CXCursor_ObjCForCollectionStmt = 222,
        CXCursor_CXXCatchStmt = 223,
        CXCursor_CXXTryStmt = 224,
        CXCursor_CXXForRangeStmt = 225,
        CXCursor_SEHTryStmt = 226,
        CXCursor_SEHExceptStmt = 227,
        CXCursor_SEHFinallyStmt = 228,
        CXCursor_MSAsmStmt = 229,
        CXCursor_NullStmt = 230,
        CXCursor_DeclStmt = 231,
        CXCursor_OMPParallelDirective = 232,
        CXCursor_OMPSimdDirective = 233,
        CXCursor_OMPForDirective = 234,
        CXCursor_OMPSectionsDirective = 235,
        CXCursor_OMPSectionDirective = 236,
        CXCursor_OMPSingleDirective = 237,
        CXCursor_OMPParallelForDirective = 238,
        CXCursor_OMPParallelSectionsDirective = 239,
        CXCursor_OMPTaskDirective = 240,
        CXCursor_OMPMasterDirective = 241,
        CXCursor_OMPCriticalDirective = 242,
        CXCursor_OMPTaskyieldDirective = 243,
        CXCursor_OMPBarrierDirective = 244,
        CXCursor_OMPTaskwaitDirective = 245,
        CXCursor_OMPFlushDirective = 246,
        CXCursor_SEHLeaveStmt = 247,
        CXCursor_OMPOrderedDirective = 248,
        CXCursor_OMPAtomicDirective = 249,
        CXCursor_OMPForSimdDirective = 250,
        CXCursor_OMPParallelForSimdDirective = 251,
        CXCursor_OMPTargetDirective = 252,
        CXCursor_OMPTeamsDirective = 253,
        CXCursor_OMPTaskgroupDirective = 254,
        CXCursor_OMPCancellationPointDirective = 255,
        CXCursor_OMPCancelDirective = 256,
        CXCursor_OMPTargetDataDirective = 257,
        CXCursor_OMPTaskLoopDirective = 258,
        CXCursor_OMPTaskLoopSimdDirective = 259,
        CXCursor_OMPDistributeDirective = 260,
        CXCursor_OMPTargetEnterDataDirective = 261,
        CXCursor_OMPTargetExitDataDirective = 262,
        CXCursor_OMPTargetParallelDirective = 263,
        CXCursor_OMPTargetParallelForDirective = 264,
        CXCursor_OMPTargetUpdateDirective = 265,
        CXCursor_OMPDistributeParallelForDirective = 266,
        CXCursor_OMPDistributeParallelForSimdDirective = 267,
        CXCursor_OMPDistributeSimdDirective = 268,
        CXCursor_OMPTargetParallelForSimdDirective = 269,
        CXCursor_LastStmt = CXCursor_OMPTargetParallelForSimdDirective,
        CXCursor_TranslationUnit = 300,
        CXCursor_FirstAttr = 400,
        CXCursor_UnexposedAttr = 400,
        CXCursor_IBActionAttr = 401,
        CXCursor_IBOutletAttr = 402,
        CXCursor_IBOutletCollectionAttr = 403,
        CXCursor_CXXFinalAttr = 404,
        CXCursor_CXXOverrideAttr = 405,
        CXCursor_AnnotateAttr = 406,
        CXCursor_AsmLabelAttr = 407,
        CXCursor_PackedAttr = 408,
        CXCursor_PureAttr = 409,
        CXCursor_ConstAttr = 410,
        CXCursor_NoDuplicateAttr = 411,
        CXCursor_CUDAConstantAttr = 412,
        CXCursor_CUDADeviceAttr = 413,
        CXCursor_CUDAGlobalAttr = 414,
        CXCursor_CUDAHostAttr = 415,
        CXCursor_CUDASharedAttr = 416,
        CXCursor_VisibilityAttr = 417,
        CXCursor_DLLExport = 418,
        CXCursor_DLLImport = 419,
        CXCursor_LastAttr = CXCursor_DLLImport,
        CXCursor_PreprocessingDirective = 500,
        CXCursor_MacroDefinition = 501,
        CXCursor_MacroExpansion = 502,
        CXCursor_MacroInstantiation = CXCursor_MacroExpansion,
        CXCursor_InclusionDirective = 503,
        CXCursor_FirstPreprocessing = CXCursor_PreprocessingDirective,
        CXCursor_LastPreprocessing = CXCursor_InclusionDirective,
        CXCursor_ModuleImportDecl = 600,
        CXCursor_TypeAliasTemplateDecl = 601,
        CXCursor_StaticAssert = 602,
        CXCursor_FirstExtraDecl = CXCursor_ModuleImportDecl,
        CXCursor_LastExtraDecl = CXCursor_StaticAssert,
        CXCursor_OverloadCandidate = 700
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCursor
    {
        public CXCursorKind kind;
        public int xdata;
        public fixed ulong data[3];
    }

    internal enum CXLinkageKind
    {
        CXLinkage_Invalid,
        CXLinkage_NoLinkage,
        CXLinkage_Internal,
        CXLinkage_UniqueExternal,
        CXLinkage_External
    }

    internal enum CXVisibilityKind
    {
        CXVisibility_Invalid,
        CXVisibility_Hidden,
        CXVisibility_Protected,
        CXVisibility_Default
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXPlatformAvailability
    {
        public CXString Platform;
        public CXVersion Introduced;
        public CXVersion Deprecated;
        public CXVersion Obsoleted;
        public int Unavailable;
        public CXString Message;
    }

    internal enum CXLanguageKind
    {
        CXLanguage_Invalid = 0,
        CXLanguage_C,
        CXLanguage_ObjC,
        CXLanguage_CPlusPlus
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXCursorSetImpl { }

    internal enum CXTypeKind
    {
        CXType_Invalid = 0,
        CXType_Unexposed = 1,
        CXType_Void = 2,
        CXType_Bool = 3,
        CXType_Char_U = 4,
        CXType_UChar = 5,
        CXType_Char16 = 6,
        CXType_Char32 = 7,
        CXType_UShort = 8,
        CXType_UInt = 9,
        CXType_ULong = 10,
        CXType_ULongLong = 11,
        CXType_UInt128 = 12,
        CXType_Char_S = 13,
        CXType_SChar = 14,
        CXType_WChar = 15,
        CXType_Short = 16,
        CXType_Int = 17,
        CXType_Long = 18,
        CXType_LongLong = 19,
        CXType_Int128 = 20,
        CXType_Float = 21,
        CXType_Double = 22,
        CXType_LongDouble = 23,
        CXType_NullPtr = 24,
        CXType_Overload = 25,
        CXType_Dependent = 26,
        CXType_ObjCId = 27,
        CXType_ObjCClass = 28,
        CXType_ObjCSel = 29,
        CXType_Float128 = 30,
        CXType_FirstBuiltin = CXType_Void,
        CXType_LastBuiltin = CXType_ObjCSel,
        CXType_Complex = 100,
        CXType_Pointer = 101,
        CXType_BlockPointer = 102,
        CXType_LValueReference = 103,
        CXType_RValueReference = 104,
        CXType_Record = 105,
        CXType_Enum = 106,
        CXType_Typedef = 107,
        CXType_ObjCInterface = 108,
        CXType_ObjCObjectPointer = 109,
        CXType_FunctionNoProto = 110,
        CXType_FunctionProto = 111,
        CXType_ConstantArray = 112,
        CXType_Vector = 113,
        CXType_IncompleteArray = 114,
        CXType_VariableArray = 115,
        CXType_DependentSizedArray = 116,
        CXType_MemberPointer = 117,
        CXType_Auto = 118,
        CXType_Elaborated = 119
    }

    internal enum CXCallingConv
    {
        CXCallingConv_Default = 0,
        CXCallingConv_C = 1,
        CXCallingConv_X86StdCall = 2,
        CXCallingConv_X86FastCall = 3,
        CXCallingConv_X86ThisCall = 4,
        CXCallingConv_X86Pascal = 5,
        CXCallingConv_AAPCS = 6,
        CXCallingConv_AAPCS_VFP = 7,
        CXCallingConv_IntelOclBicc = 9,
        CXCallingConv_X86_64Win64 = 10,
        CXCallingConv_X86_64SysV = 11,
        CXCallingConv_X86VectorCall = 12,
        CXCallingConv_Swift = 13,
        CXCallingConv_PreserveMost = 14,
        CXCallingConv_PreserveAll = 15,
        CXCallingConv_Invalid = 100,
        CXCallingConv_Unexposed = 200
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXType
    {
        public CXTypeKind kind;
        public fixed ulong data[2];
    }

    internal enum CXTemplateArgumentKind
    {
        CXTemplateArgumentKind_Null,
        CXTemplateArgumentKind_Type,
        CXTemplateArgumentKind_Declaration,
        CXTemplateArgumentKind_NullPtr,
        CXTemplateArgumentKind_Integral,
        CXTemplateArgumentKind_Template,
        CXTemplateArgumentKind_TemplateExpansion,
        CXTemplateArgumentKind_Expression,
        CXTemplateArgumentKind_Pack,
        CXTemplateArgumentKind_Invalid
    }

    internal enum CXTypeLayoutError
    {
        CXTypeLayoutError_Invalid = -1,
        CXTypeLayoutError_Incomplete = -2,
        CXTypeLayoutError_Dependent = -3,
        CXTypeLayoutError_NotConstantSize = -4,
        CXTypeLayoutError_InvalidFieldName = -5
    }

    internal enum CXRefQualifierKind
    {
        CXRefQualifier_None = 0,
        CXRefQualifier_LValue,
        CXRefQualifier_RValue
    }

    internal enum CX_CXXAccessSpecifier
    {
        CX_CXXInvalidAccessSpecifier,
        CX_CXXPublic,
        CX_CXXProtected,
        CX_CXXPrivate
    }

    internal enum CX_StorageClass
    {
        CX_SC_Invalid,
        CX_SC_None,
        CX_SC_Extern,
        CX_SC_Static,
        CX_SC_PrivateExtern,
        CX_SC_OpenCLWorkGroupLocal,
        CX_SC_Auto,
        CX_SC_Register
    }

    internal enum CXChildVisitResult
    {
        CXChildVisit_Break,
        CXChildVisit_Continue,
        CXChildVisit_Recurse
    }

    internal enum CXObjCPropertyAttrKind
    {
        CXObjCPropertyAttr_noattr = 0x00,
        CXObjCPropertyAttr_readonly = 0x01,
        CXObjCPropertyAttr_getter = 0x02,
        CXObjCPropertyAttr_assign = 0x04,
        CXObjCPropertyAttr_readwrite = 0x08,
        CXObjCPropertyAttr_retain = 0x10,
        CXObjCPropertyAttr_copy = 0x20,
        CXObjCPropertyAttr_nonatomic = 0x40,
        CXObjCPropertyAttr_setter = 0x80,
        CXObjCPropertyAttr_atomic = 0x100,
        CXObjCPropertyAttr_weak = 0x200,
        CXObjCPropertyAttr_strong = 0x400,
        CXObjCPropertyAttr_unsafe_unretained = 0x800,
        CXObjCPropertyAttr_class = 0x1000
    }

    internal enum CXObjCDeclQualifierKind
    {
        CXObjCDeclQualifier_None = 0x0,
        CXObjCDeclQualifier_In = 0x1,
        CXObjCDeclQualifier_Inout = 0x2,
        CXObjCDeclQualifier_Out = 0x4,
        CXObjCDeclQualifier_Bycopy = 0x8,
        CXObjCDeclQualifier_Byref = 0x10,
        CXObjCDeclQualifier_Oneway = 0x20
    }

    internal struct CXModuleImpl { }

    internal enum CXNameRefFlags
    {
        CXNameRange_WantQualifier = 0x1,
        CXNameRange_WantTemplateArgs = 0x2,
        CXNameRange_WantSinglePiece = 0x4
    }

    internal enum CXTokenKind
    {
        CXToken_Punctuation,
        CXToken_Keyword,
        CXToken_Identifier,
        CXToken_Literal,
        CXToken_Comment
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXToken
    {
        public fixed uint int_data[4];
        public void* ptr_data;
    }

    internal struct CXCompletionStringImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCompletionResult
    {
        public CXCursorKind CursorKind;
        public CXCompletionStringImpl* CompletionString;
    }

    internal enum CXCompletionChunkKind
    {
        CXCompletionChunk_Optional,
        CXCompletionChunk_TypedText,
        CXCompletionChunk_Text,
        CXCompletionChunk_Placeholder,
        CXCompletionChunk_Informative,
        CXCompletionChunk_CurrentParameter,
        CXCompletionChunk_LeftParen,
        CXCompletionChunk_RightParen,
        CXCompletionChunk_LeftBracket,
        CXCompletionChunk_RightBracket,
        CXCompletionChunk_LeftBrace,
        CXCompletionChunk_RightBrace,
        CXCompletionChunk_LeftAngle,
        CXCompletionChunk_RightAngle,
        CXCompletionChunk_Comma,
        CXCompletionChunk_ResultType,
        CXCompletionChunk_Colon,
        CXCompletionChunk_SemiColon,
        CXCompletionChunk_Equal,
        CXCompletionChunk_HorizontalSpace,
        CXCompletionChunk_VerticalSpace
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCodeCompleteResults
    {
        public CXCompletionResult* Results;
        public uint NumResults;
    }

    internal enum CXCodeComplete_Flags
    {
        CXCodeComplete_IncludeMacros = 0x01,
        CXCodeComplete_IncludeCodePatterns = 0x02,
        CXCodeComplete_IncludeBriefComments = 0x04
    }

    internal enum CXCompletionContext
    {
        CXCompletionContext_Unexposed = 0,
        CXCompletionContext_AnyType = 1 << 0,
        CXCompletionContext_AnyValue = 1 << 1,
        CXCompletionContext_ObjCObjectValue = 1 << 2,
        CXCompletionContext_ObjCSelectorValue = 1 << 3,
        CXCompletionContext_CXXClassTypeValue = 1 << 4,
        CXCompletionContext_DotMemberAccess = 1 << 5,
        CXCompletionContext_ArrowMemberAccess = 1 << 6,
        CXCompletionContext_ObjCPropertyAccess = 1 << 7,
        CXCompletionContext_EnumTag = 1 << 8,
        CXCompletionContext_UnionTag = 1 << 9,
        CXCompletionContext_StructTag = 1 << 10,
        CXCompletionContext_ClassTag = 1 << 11,
        CXCompletionContext_Namespace = 1 << 12,
        CXCompletionContext_NestedNameSpecifier = 1 << 13,
        CXCompletionContext_ObjCInterface = 1 << 14,
        CXCompletionContext_ObjCProtocol = 1 << 15,
        CXCompletionContext_ObjCCategory = 1 << 16,
        CXCompletionContext_ObjCInstanceMessage = 1 << 17,
        CXCompletionContext_ObjCClassMessage = 1 << 18,
        CXCompletionContext_ObjCSelectorName = 1 << 19,
        CXCompletionContext_MacroName = 1 << 20,
        CXCompletionContext_NaturalLanguage = 1 << 21,
        CXCompletionContext_Unknown = ((1 << 22) - 1)
    }

    internal enum CXEvalResultKind
    {
        CXEval_Int = 1,
        CXEval_Float = 2,
        CXEval_ObjCStrLiteral = 3,
        CXEval_StrLiteral = 4,
        CXEval_CFStr = 5,
        CXEval_Other = 6,
        CXEval_UnExposed = 0
    }

    internal struct CXEvalResultImpl { }

    internal struct CXRemappingImpl { }

    internal enum CXVisitorResult
    {
        CXVisit_Break,
        CXVisit_Continue
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCursorAndRangeVisitor
    {
        public void* context;
        public IntPtr visit;
    }

    internal enum CXResult
    {
        CXResult_Success = 0,
        CXResult_Invalid = 1,
        CXResult_VisitBreak = 2
    }

    internal struct CXIdxClientFileImpl { }

    internal struct CXIdxClientEntityImpl { }

    internal struct CXIdxClientContainerImpl { }

    internal struct CXIdxClientASTFileImpl { }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxLoc
    {
        public fixed ulong ptr_data[2];
        public uint int_data;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxIncludedFileInfo
    {
        public CXIdxLoc hashLoc;
        public sbyte* filename;
        public CXFileImpl* file;
        public int isImport;
        public int isAngled;
        public int isModuleImport;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxImportedASTFileInfo
    {
        public CXFileImpl* file;
        public CXModuleImpl* module;
        public CXIdxLoc loc;
        public int isImplicit;
    }

    internal enum CXIdxEntityKind
    {
        CXIdxEntity_Unexposed = 0,
        CXIdxEntity_Typedef = 1,
        CXIdxEntity_Function = 2,
        CXIdxEntity_Variable = 3,
        CXIdxEntity_Field = 4,
        CXIdxEntity_EnumConstant = 5,
        CXIdxEntity_ObjCClass = 6,
        CXIdxEntity_ObjCProtocol = 7,
        CXIdxEntity_ObjCCategory = 8,
        CXIdxEntity_ObjCInstanceMethod = 9,
        CXIdxEntity_ObjCClassMethod = 10,
        CXIdxEntity_ObjCProperty = 11,
        CXIdxEntity_ObjCIvar = 12,
        CXIdxEntity_Enum = 13,
        CXIdxEntity_Struct = 14,
        CXIdxEntity_Union = 15,
        CXIdxEntity_CXXClass = 16,
        CXIdxEntity_CXXNamespace = 17,
        CXIdxEntity_CXXNamespaceAlias = 18,
        CXIdxEntity_CXXStaticVariable = 19,
        CXIdxEntity_CXXStaticMethod = 20,
        CXIdxEntity_CXXInstanceMethod = 21,
        CXIdxEntity_CXXConstructor = 22,
        CXIdxEntity_CXXDestructor = 23,
        CXIdxEntity_CXXConversionFunction = 24,
        CXIdxEntity_CXXTypeAlias = 25,
        CXIdxEntity_CXXInterface = 26
    }

    internal enum CXIdxEntityLanguage
    {
        CXIdxEntityLang_None = 0,
        CXIdxEntityLang_C = 1,
        CXIdxEntityLang_ObjC = 2,
        CXIdxEntityLang_CXX = 3
    }

    internal enum CXIdxEntityCXXTemplateKind
    {
        CXIdxEntity_NonTemplate = 0,
        CXIdxEntity_Template = 1,
        CXIdxEntity_TemplatePartialSpecialization = 2,
        CXIdxEntity_TemplateSpecialization = 3
    }

    internal enum CXIdxAttrKind
    {
        CXIdxAttr_Unexposed = 0,
        CXIdxAttr_IBAction = 1,
        CXIdxAttr_IBOutlet = 2,
        CXIdxAttr_IBOutletCollection = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXIdxAttrInfo
    {
        public CXIdxAttrKind kind;
        public CXCursor cursor;
        public CXIdxLoc loc;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxEntityInfo
    {
        public CXIdxEntityKind kind;
        public CXIdxEntityCXXTemplateKind templateKind;
        public CXIdxEntityLanguage lang;
        public sbyte* name;
        public sbyte* USR;
        public CXCursor cursor;
        public CXIdxAttrInfo** attributes;
        public uint numAttributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CXIdxContainerInfo
    {
        public CXCursor cursor;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxIBOutletCollectionAttrInfo
    {
        public CXIdxAttrInfo* attrInfo;
        public CXIdxEntityInfo* objcClass;
        public CXCursor classCursor;
        public CXIdxLoc classLoc;
    }

    internal enum CXIdxDeclInfoFlags
    {
        CXIdxDeclFlag_Skipped = 0x1
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxDeclInfo
    {
        public CXIdxEntityInfo* entityInfo;
        public CXCursor cursor;
        public CXIdxLoc loc;
        public CXIdxContainerInfo* semanticContainer;
        public CXIdxContainerInfo* lexicalContainer;
        public int isRedeclaration;
        public int isDefinition;
        public int isContainer;
        public CXIdxContainerInfo* declAsContainer;
        public int isImplicit;
        public CXIdxAttrInfo** attributes;
        public uint numAttributes;
        public uint flags;
    }

    internal enum CXIdxObjCContainerKind
    {
        CXIdxObjCContainer_ForwardRef = 0,
        CXIdxObjCContainer_Interface = 1,
        CXIdxObjCContainer_Implementation = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCContainerDeclInfo
    {
        public CXIdxDeclInfo* declInfo;
        public CXIdxObjCContainerKind kind;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxBaseClassInfo
    {
        public CXIdxEntityInfo* @base;
        public CXCursor cursor;
        public CXIdxLoc loc;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCProtocolRefInfo
    {
        public CXIdxEntityInfo* protocol;
        public CXCursor cursor;
        public CXIdxLoc loc;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCProtocolRefListInfo
    {
        public CXIdxObjCProtocolRefInfo** protocols;
        public uint numProtocols;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCInterfaceDeclInfo
    {
        public CXIdxObjCContainerDeclInfo* containerInfo;
        public CXIdxBaseClassInfo* superInfo;
        public CXIdxObjCProtocolRefListInfo* protocols;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCCategoryDeclInfo
    {
        public CXIdxObjCContainerDeclInfo* containerInfo;
        public CXIdxEntityInfo* objcClass;
        public CXCursor classCursor;
        public CXIdxLoc classLoc;
        public CXIdxObjCProtocolRefListInfo* protocols;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxObjCPropertyDeclInfo
    {
        public CXIdxDeclInfo* declInfo;
        public CXIdxEntityInfo* getter;
        public CXIdxEntityInfo* setter;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxCXXClassDeclInfo
    {
        public CXIdxDeclInfo* declInfo;
        public CXIdxBaseClassInfo** bases;
        public uint numBases;
    }

    internal enum CXIdxEntityRefKind
    {
        CXIdxEntityRef_Direct = 1,
        CXIdxEntityRef_Implicit = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXIdxEntityRefInfo
    {
        public CXIdxEntityRefKind kind;
        public CXCursor cursor;
        public CXIdxLoc loc;
        public CXIdxEntityInfo* referencedEntity;
        public CXIdxEntityInfo* parentEntity;
        public CXIdxContainerInfo* container;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IndexerCallbacks
    {
        public IntPtr abortQuery;
        public IntPtr diagnostic;
        public IntPtr enteredMainFile;
        public IntPtr ppIncludedFile;
        public IntPtr importedASTFile;
        public IntPtr startedTranslationUnit;
        public IntPtr indexDeclaration;
        public IntPtr indexEntityReference;
    }

    internal struct CXIndexActionImpl { }

    internal enum CXIndexOptFlags
    {
        CXIndexOpt_None = 0x0,
        CXIndexOpt_SuppressRedundantRefs = 0x1,
        CXIndexOpt_IndexFunctionLocalSymbols = 0x2,
        CXIndexOpt_IndexImplicitTemplateInstantiations = 0x4,
        CXIndexOpt_SuppressWarnings = 0x8,
        CXIndexOpt_SkipParsedBodiesInSession = 0x10
    }
}
