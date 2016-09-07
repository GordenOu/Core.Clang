using System;
using System.Runtime.InteropServices;

namespace Core.Clang
{
    internal static unsafe class NativeMethods
    {
        private const string dllName = "libclang";

        [DllImport(dllName)]
        public static extern sbyte* clang_getCString(
            CXString @string);

        [DllImport(dllName)]
        public static extern void clang_disposeString(
            CXString @string);

        [DllImport(dllName)]
        public static extern void clang_disposeStringSet(
            CXStringSet* set);

        [DllImport(dllName)]
        public static extern ulong clang_getBuildSessionTimestamp();

        [DllImport(dllName)]
        public static extern CXVirtualFileOverlayImpl* clang_VirtualFileOverlay_create(
            uint options);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_VirtualFileOverlay_addFileMapping(
            CXVirtualFileOverlayImpl* arg1,
            sbyte* virtualPath,
            sbyte* realPath);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_VirtualFileOverlay_setCaseSensitivity(
            CXVirtualFileOverlayImpl* arg1,
            int caseSensitive);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_VirtualFileOverlay_writeToBuffer(
            CXVirtualFileOverlayImpl* arg1,
            uint options,
            sbyte** out_buffer_ptr,
            uint* out_buffer_size);

        [DllImport(dllName)]
        public static extern void clang_free(
            void* buffer);

        [DllImport(dllName)]
        public static extern void clang_VirtualFileOverlay_dispose(
            CXVirtualFileOverlayImpl* arg1);

        [DllImport(dllName)]
        public static extern CXModuleMapDescriptorImpl* clang_ModuleMapDescriptor_create(
            uint options);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_ModuleMapDescriptor_setFrameworkModuleName(
            CXModuleMapDescriptorImpl* arg1,
            sbyte* name);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_ModuleMapDescriptor_setUmbrellaHeader(
            CXModuleMapDescriptorImpl* arg1,
            sbyte* name);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_ModuleMapDescriptor_writeToBuffer(
            CXModuleMapDescriptorImpl* arg1,
            uint options,
            sbyte** out_buffer_ptr,
            uint* out_buffer_size);

        [DllImport(dllName)]
        public static extern void clang_ModuleMapDescriptor_dispose(
            CXModuleMapDescriptorImpl* arg1);

        [DllImport(dllName)]
        public static extern CXIndexImpl* clang_createIndex(
            int excludeDeclarationsFromPCH,
            int displayDiagnostics);

        [DllImport(dllName)]
        public static extern void clang_disposeIndex(
            CXIndexImpl* index);

        [DllImport(dllName)]
        public static extern void clang_CXIndex_setGlobalOptions(
            CXIndexImpl* arg1,
            uint options);

        [DllImport(dllName)]
        public static extern uint clang_CXIndex_getGlobalOptions(
            CXIndexImpl* arg1);

        [DllImport(dllName)]
        public static extern CXString clang_getFileName(
            CXFileImpl* SFile);

        [DllImport(dllName)]
        public static extern long clang_getFileTime(
            CXFileImpl* SFile);

        [DllImport(dllName)]
        public static extern int clang_getFileUniqueID(
            CXFileImpl* file,
            CXFileUniqueID* outID);

        [DllImport(dllName)]
        public static extern uint clang_isFileMultipleIncludeGuarded(
            CXTranslationUnitImpl* tu,
            CXFileImpl* file);

        [DllImport(dllName)]
        public static extern CXFileImpl* clang_getFile(
            CXTranslationUnitImpl* tu,
            sbyte* file_name);

        [DllImport(dllName)]
        public static extern int clang_File_isEqual(
            CXFileImpl* file1,
            CXFileImpl* file2);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getNullLocation();

        [DllImport(dllName)]
        public static extern uint clang_equalLocations(
            CXSourceLocation loc1,
            CXSourceLocation loc2);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getLocation(
            CXTranslationUnitImpl* tu,
            CXFileImpl* file,
            uint line,
            uint column);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getLocationForOffset(
            CXTranslationUnitImpl* tu,
            CXFileImpl* file,
            uint offset);

        [DllImport(dllName)]
        public static extern int clang_Location_isInSystemHeader(
            CXSourceLocation location);

        [DllImport(dllName)]
        public static extern int clang_Location_isFromMainFile(
            CXSourceLocation location);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getNullRange();

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getRange(
            CXSourceLocation begin,
            CXSourceLocation end);

        [DllImport(dllName)]
        public static extern uint clang_equalRanges(
            CXSourceRange range1,
            CXSourceRange range2);

        [DllImport(dllName)]
        public static extern int clang_Range_isNull(
            CXSourceRange range);

        [DllImport(dllName)]
        public static extern void clang_getExpansionLocation(
            CXSourceLocation location,
            CXFileImpl** file,
            uint* line,
            uint* column,
            uint* offset);

        [DllImport(dllName)]
        public static extern void clang_getPresumedLocation(
            CXSourceLocation location,
            CXString* filename,
            uint* line,
            uint* column);

        [DllImport(dllName)]
        public static extern void clang_getInstantiationLocation(
            CXSourceLocation location,
            CXFileImpl** file,
            uint* line,
            uint* column,
            uint* offset);

        [DllImport(dllName)]
        public static extern void clang_getSpellingLocation(
            CXSourceLocation location,
            CXFileImpl** file,
            uint* line,
            uint* column,
            uint* offset);

        [DllImport(dllName)]
        public static extern void clang_getFileLocation(
            CXSourceLocation location,
            CXFileImpl** file,
            uint* line,
            uint* column,
            uint* offset);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getRangeStart(
            CXSourceRange range);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getRangeEnd(
            CXSourceRange range);

        [DllImport(dllName)]
        public static extern CXSourceRangeList* clang_getSkippedRanges(
            CXTranslationUnitImpl* tu,
            CXFileImpl* file);

        [DllImport(dllName)]
        public static extern void clang_disposeSourceRangeList(
            CXSourceRangeList* ranges);

        [DllImport(dllName)]
        public static extern uint clang_getNumDiagnosticsInSet(
            CXDiagnosticSetImpl* Diags);

        [DllImport(dllName)]
        public static extern CXDiagnosticImpl* clang_getDiagnosticInSet(
            CXDiagnosticSetImpl* Diags,
            uint Index);

        [DllImport(dllName)]
        public static extern CXDiagnosticSetImpl* clang_loadDiagnostics(
            sbyte* file,
            CXLoadDiag_Error* error,
            CXString* errorString);

        [DllImport(dllName)]
        public static extern void clang_disposeDiagnosticSet(
            CXDiagnosticSetImpl* Diags);

        [DllImport(dllName)]
        public static extern CXDiagnosticSetImpl* clang_getChildDiagnostics(
            CXDiagnosticImpl* D);

        [DllImport(dllName)]
        public static extern uint clang_getNumDiagnostics(
            CXTranslationUnitImpl* Unit);

        [DllImport(dllName)]
        public static extern CXDiagnosticImpl* clang_getDiagnostic(
            CXTranslationUnitImpl* Unit,
            uint Index);

        [DllImport(dllName)]
        public static extern CXDiagnosticSetImpl* clang_getDiagnosticSetFromTU(
            CXTranslationUnitImpl* Unit);

        [DllImport(dllName)]
        public static extern void clang_disposeDiagnostic(
            CXDiagnosticImpl* Diagnostic);

        [DllImport(dllName)]
        public static extern CXString clang_formatDiagnostic(
            CXDiagnosticImpl* Diagnostic,
            uint Options);

        [DllImport(dllName)]
        public static extern uint clang_defaultDiagnosticDisplayOptions();

        [DllImport(dllName)]
        public static extern CXDiagnosticSeverity clang_getDiagnosticSeverity(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getDiagnosticLocation(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern CXString clang_getDiagnosticSpelling(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern CXString clang_getDiagnosticOption(
            CXDiagnosticImpl* Diag,
            CXString* Disable);

        [DllImport(dllName)]
        public static extern uint clang_getDiagnosticCategory(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern CXString clang_getDiagnosticCategoryName(
            uint Category);

        [DllImport(dllName)]
        public static extern CXString clang_getDiagnosticCategoryText(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern uint clang_getDiagnosticNumRanges(
            CXDiagnosticImpl* arg1);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getDiagnosticRange(
            CXDiagnosticImpl* Diagnostic,
            uint Range);

        [DllImport(dllName)]
        public static extern uint clang_getDiagnosticNumFixIts(
            CXDiagnosticImpl* Diagnostic);

        [DllImport(dllName)]
        public static extern CXString clang_getDiagnosticFixIt(
            CXDiagnosticImpl* Diagnostic,
            uint FixIt,
            CXSourceRange* ReplacementRange);

        [DllImport(dllName)]
        public static extern CXString clang_getTranslationUnitSpelling(
            CXTranslationUnitImpl* CTUnit);

        [DllImport(dllName)]
        public static extern CXTranslationUnitImpl* clang_createTranslationUnitFromSourceFile(
            CXIndexImpl* CIdx,
            sbyte* source_filename,
            int num_clang_command_line_args,
            sbyte** clang_command_line_args,
            uint num_unsaved_files,
            CXUnsavedFile* unsaved_files);

        [DllImport(dllName)]
        public static extern CXTranslationUnitImpl* clang_createTranslationUnit(
            CXIndexImpl* CIdx,
            sbyte* ast_filename);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_createTranslationUnit2(
            CXIndexImpl* CIdx,
            sbyte* ast_filename,
            CXTranslationUnitImpl** out_TU);

        [DllImport(dllName)]
        public static extern uint clang_defaultEditingTranslationUnitOptions();

        [DllImport(dllName)]
        public static extern CXTranslationUnitImpl* clang_parseTranslationUnit(
            CXIndexImpl* CIdx,
            sbyte* source_filename,
            sbyte** command_line_args,
            int num_command_line_args,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            uint options);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_parseTranslationUnit2(
            CXIndexImpl* CIdx,
            sbyte* source_filename,
            sbyte** command_line_args,
            int num_command_line_args,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            uint options,
            CXTranslationUnitImpl** out_TU);

        [DllImport(dllName)]
        public static extern CXErrorCode clang_parseTranslationUnit2FullArgv(
            CXIndexImpl* CIdx,
            sbyte* source_filename,
            sbyte** command_line_args,
            int num_command_line_args,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            uint options,
            CXTranslationUnitImpl** out_TU);

        [DllImport(dllName)]
        public static extern uint clang_defaultSaveOptions(
            CXTranslationUnitImpl* TU);

        [DllImport(dllName)]
        public static extern int clang_saveTranslationUnit(
            CXTranslationUnitImpl* TU,
            sbyte* FileName,
            uint options);

        [DllImport(dllName)]
        public static extern void clang_disposeTranslationUnit(
            CXTranslationUnitImpl* arg1);

        [DllImport(dllName)]
        public static extern uint clang_defaultReparseOptions(
            CXTranslationUnitImpl* TU);

        [DllImport(dllName)]
        public static extern int clang_reparseTranslationUnit(
            CXTranslationUnitImpl* TU,
            uint num_unsaved_files,
            CXUnsavedFile* unsaved_files,
            uint options);

        [DllImport(dllName)]
        public static extern sbyte* clang_getTUResourceUsageName(
            CXTUResourceUsageKind kind);

        [DllImport(dllName)]
        public static extern CXTUResourceUsage clang_getCXTUResourceUsage(
            CXTranslationUnitImpl* TU);

        [DllImport(dllName)]
        public static extern void clang_disposeCXTUResourceUsage(
            CXTUResourceUsage usage);

        [DllImport(dllName)]
        public static extern CXCursor clang_getNullCursor();

        [DllImport(dllName)]
        public static extern CXCursor clang_getTranslationUnitCursor(
            CXTranslationUnitImpl* arg1);

        [DllImport(dllName)]
        public static extern uint clang_equalCursors(
            CXCursor arg1,
            CXCursor arg2);

        [DllImport(dllName)]
        public static extern int clang_Cursor_isNull(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern uint clang_hashCursor(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXCursorKind clang_getCursorKind(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern uint clang_isDeclaration(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isReference(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isExpression(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isStatement(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isAttribute(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_hasAttrs(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_isInvalid(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isTranslationUnit(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isPreprocessing(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern uint clang_isUnexposed(
            CXCursorKind arg1);

        [DllImport(dllName)]
        public static extern CXLinkageKind clang_getCursorLinkage(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXVisibilityKind clang_getCursorVisibility(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXAvailabilityKind clang_getCursorAvailability(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern int clang_getCursorPlatformAvailability(
            CXCursor cursor,
            int* always_deprecated,
            CXString* deprecated_message,
            int* always_unavailable,
            CXString* unavailable_message,
            CXPlatformAvailability* availability,
            int availability_size);

        [DllImport(dllName)]
        public static extern void clang_disposeCXPlatformAvailability(
            CXPlatformAvailability* availability);

        [DllImport(dllName)]
        public static extern CXLanguageKind clang_getCursorLanguage(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXTranslationUnitImpl* clang_Cursor_getTranslationUnit(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXCursorSetImpl* clang_createCXCursorSet();

        [DllImport(dllName)]
        public static extern void clang_disposeCXCursorSet(
            CXCursorSetImpl* cset);

        [DllImport(dllName)]
        public static extern uint clang_CXCursorSet_contains(
            CXCursorSetImpl* cset,
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern uint clang_CXCursorSet_insert(
            CXCursorSetImpl* cset,
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCursorSemanticParent(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCursorLexicalParent(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern void clang_getOverriddenCursors(
            CXCursor cursor,
            CXCursor** overridden,
            uint* num_overridden);

        [DllImport(dllName)]
        public static extern void clang_disposeOverriddenCursors(
            CXCursor* overridden);

        [DllImport(dllName)]
        public static extern CXFileImpl* clang_getIncludedFile(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCursor(
            CXTranslationUnitImpl* arg1,
            CXSourceLocation arg2);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getCursorLocation(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getCursorExtent(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXType clang_getCursorType(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXString clang_getTypeSpelling(
            CXType CT);

        [DllImport(dllName)]
        public static extern CXType clang_getTypedefDeclUnderlyingType(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXType clang_getEnumDeclIntegerType(
            CXCursor C);

        [DllImport(dllName)]
        public static extern long clang_getEnumConstantDeclValue(
            CXCursor C);

        [DllImport(dllName)]
        public static extern ulong clang_getEnumConstantDeclUnsignedValue(
            CXCursor C);

        [DllImport(dllName)]
        public static extern int clang_getFieldDeclBitWidth(
            CXCursor C);

        [DllImport(dllName)]
        public static extern int clang_Cursor_getNumArguments(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXCursor clang_Cursor_getArgument(
            CXCursor C,
            uint i);

        [DllImport(dllName)]
        public static extern int clang_Cursor_getNumTemplateArguments(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXTemplateArgumentKind clang_Cursor_getTemplateArgumentKind(
            CXCursor C,
            uint I);

        [DllImport(dllName)]
        public static extern CXType clang_Cursor_getTemplateArgumentType(
            CXCursor C,
            uint I);

        [DllImport(dllName)]
        public static extern long clang_Cursor_getTemplateArgumentValue(
            CXCursor C,
            uint I);

        [DllImport(dllName)]
        public static extern ulong clang_Cursor_getTemplateArgumentUnsignedValue(
            CXCursor C,
            uint I);

        [DllImport(dllName)]
        public static extern uint clang_equalTypes(
            CXType A,
            CXType B);

        [DllImport(dllName)]
        public static extern CXType clang_getCanonicalType(
            CXType T);

        [DllImport(dllName)]
        public static extern uint clang_isConstQualifiedType(
            CXType T);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isMacroFunctionLike(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isMacroBuiltin(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isFunctionInlined(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_isVolatileQualifiedType(
            CXType T);

        [DllImport(dllName)]
        public static extern uint clang_isRestrictQualifiedType(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getPointeeType(
            CXType T);

        [DllImport(dllName)]
        public static extern CXCursor clang_getTypeDeclaration(
            CXType T);

        [DllImport(dllName)]
        public static extern CXString clang_getDeclObjCTypeEncoding(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXString clang_Type_getObjCEncoding(
            CXType type);

        [DllImport(dllName)]
        public static extern CXString clang_getTypeKindSpelling(
            CXTypeKind K);

        [DllImport(dllName)]
        public static extern CXCallingConv clang_getFunctionTypeCallingConv(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getResultType(
            CXType T);

        [DllImport(dllName)]
        public static extern int clang_getNumArgTypes(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getArgType(
            CXType T,
            uint i);

        [DllImport(dllName)]
        public static extern uint clang_isFunctionTypeVariadic(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getCursorResultType(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_isPODType(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getElementType(
            CXType T);

        [DllImport(dllName)]
        public static extern long clang_getNumElements(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_getArrayElementType(
            CXType T);

        [DllImport(dllName)]
        public static extern long clang_getArraySize(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_Type_getNamedType(
            CXType T);

        [DllImport(dllName)]
        public static extern long clang_Type_getAlignOf(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_Type_getClassType(
            CXType T);

        [DllImport(dllName)]
        public static extern long clang_Type_getSizeOf(
            CXType T);

        [DllImport(dllName)]
        public static extern long clang_Type_getOffsetOf(
            CXType T,
            sbyte* S);

        [DllImport(dllName)]
        public static extern long clang_Cursor_getOffsetOfField(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isAnonymous(
            CXCursor C);

        [DllImport(dllName)]
        public static extern int clang_Type_getNumTemplateArguments(
            CXType T);

        [DllImport(dllName)]
        public static extern CXType clang_Type_getTemplateArgumentAsType(
            CXType T,
            uint i);

        [DllImport(dllName)]
        public static extern CXRefQualifierKind clang_Type_getCXXRefQualifier(
            CXType T);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isBitField(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_isVirtualBase(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CX_CXXAccessSpecifier clang_getCXXAccessSpecifier(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CX_StorageClass clang_Cursor_getStorageClass(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern uint clang_getNumOverloadedDecls(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern CXCursor clang_getOverloadedDecl(
            CXCursor cursor,
            uint index);

        [DllImport(dllName)]
        public static extern CXType clang_getIBOutletCollectionType(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern uint clang_visitChildren(
            CXCursor parent,
            IntPtr visitor,
            CXClientDataImpl* client_data);

        [DllImport(dllName)]
        public static extern CXString clang_getCursorUSR(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCClass(
            sbyte* class_name);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCCategory(
            sbyte* class_name,
            sbyte* category_name);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCProtocol(
            sbyte* protocol_name);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCIvar(
            sbyte* name,
            CXString classUSR);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCMethod(
            sbyte* name,
            uint isInstanceMethod,
            CXString classUSR);

        [DllImport(dllName)]
        public static extern CXString clang_constructUSR_ObjCProperty(
            sbyte* property,
            CXString classUSR);

        [DllImport(dllName)]
        public static extern CXString clang_getCursorSpelling(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_Cursor_getSpellingNameRange(
            CXCursor arg1,
            uint pieceIndex,
            uint options);

        [DllImport(dllName)]
        public static extern CXString clang_getCursorDisplayName(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCursorReferenced(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCursorDefinition(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern uint clang_isCursorDefinition(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXCursor clang_getCanonicalCursor(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern int clang_Cursor_getObjCSelectorIndex(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern int clang_Cursor_isDynamicCall(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXType clang_Cursor_getReceiverType(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_getObjCPropertyAttributes(
            CXCursor C,
            uint reserved);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_getObjCDeclQualifiers(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isObjCOptional(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_Cursor_isVariadic(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_Cursor_getCommentRange(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXString clang_Cursor_getRawCommentText(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXString clang_Cursor_getBriefCommentText(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXString clang_Cursor_getMangling(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXStringSet* clang_Cursor_getCXXManglings(
            CXCursor arg1);

        [DllImport(dllName)]
        public static extern CXModuleImpl* clang_Cursor_getModule(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXModuleImpl* clang_getModuleForFile(
            CXTranslationUnitImpl* arg1,
            CXFileImpl* arg2);

        [DllImport(dllName)]
        public static extern CXFileImpl* clang_Module_getASTFile(
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern CXModuleImpl* clang_Module_getParent(
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern CXString clang_Module_getName(
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern CXString clang_Module_getFullName(
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern int clang_Module_isSystem(
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern uint clang_Module_getNumTopLevelHeaders(
            CXTranslationUnitImpl* arg1,
            CXModuleImpl* Module);

        [DllImport(dllName)]
        public static extern CXFileImpl* clang_Module_getTopLevelHeader(
            CXTranslationUnitImpl* arg1,
            CXModuleImpl* Module,
            uint Index);

        [DllImport(dllName)]
        public static extern uint clang_CXXConstructor_isConvertingConstructor(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXConstructor_isCopyConstructor(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXConstructor_isDefaultConstructor(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXConstructor_isMoveConstructor(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXField_isMutable(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXMethod_isDefaulted(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXMethod_isPureVirtual(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXMethod_isStatic(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXMethod_isVirtual(
            CXCursor C);

        [DllImport(dllName)]
        public static extern uint clang_CXXMethod_isConst(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXCursorKind clang_getTemplateCursorKind(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXCursor clang_getSpecializedCursorTemplate(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getCursorReferenceNameRange(
            CXCursor C,
            uint NameFlags,
            uint PieceIndex);

        [DllImport(dllName)]
        public static extern CXTokenKind clang_getTokenKind(
            CXToken arg1);

        [DllImport(dllName)]
        public static extern CXString clang_getTokenSpelling(
            CXTranslationUnitImpl* arg1,
            CXToken arg2);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_getTokenLocation(
            CXTranslationUnitImpl* arg1,
            CXToken arg2);

        [DllImport(dllName)]
        public static extern CXSourceRange clang_getTokenExtent(
            CXTranslationUnitImpl* arg1,
            CXToken arg2);

        [DllImport(dllName)]
        public static extern void clang_tokenize(
            CXTranslationUnitImpl* TU,
            CXSourceRange Range,
            CXToken** Tokens,
            uint* NumTokens);

        [DllImport(dllName)]
        public static extern void clang_annotateTokens(
            CXTranslationUnitImpl* TU,
            CXToken* Tokens,
            uint NumTokens,
            CXCursor* Cursors);

        [DllImport(dllName)]
        public static extern void clang_disposeTokens(
            CXTranslationUnitImpl* TU,
            CXToken* Tokens,
            uint NumTokens);

        [DllImport(dllName)]
        public static extern CXString clang_getCursorKindSpelling(
            CXCursorKind Kind);

        [DllImport(dllName)]
        public static extern void clang_getDefinitionSpellingAndExtent(
            CXCursor arg1,
            sbyte** startBuf,
            sbyte** endBuf,
            uint* startLine,
            uint* startColumn,
            uint* endLine,
            uint* endColumn);

        [DllImport(dllName)]
        public static extern void clang_enableStackTraces();

        [DllImport(dllName)]
        public static extern void clang_executeOnThread(
            IntPtr fn,
            void* user_data,
            uint stack_size);

        [DllImport(dllName)]
        public static extern CXCompletionChunkKind clang_getCompletionChunkKind(
            CXCompletionStringImpl* completion_string,
            uint chunk_number);

        [DllImport(dllName)]
        public static extern CXString clang_getCompletionChunkText(
            CXCompletionStringImpl* completion_string,
            uint chunk_number);

        [DllImport(dllName)]
        public static extern CXCompletionStringImpl* clang_getCompletionChunkCompletionString(
            CXCompletionStringImpl* completion_string,
            uint chunk_number);

        [DllImport(dllName)]
        public static extern uint clang_getNumCompletionChunks(
            CXCompletionStringImpl* completion_string);

        [DllImport(dllName)]
        public static extern uint clang_getCompletionPriority(
            CXCompletionStringImpl* completion_string);

        [DllImport(dllName)]
        public static extern CXAvailabilityKind clang_getCompletionAvailability(
            CXCompletionStringImpl* completion_string);

        [DllImport(dllName)]
        public static extern uint clang_getCompletionNumAnnotations(
            CXCompletionStringImpl* completion_string);

        [DllImport(dllName)]
        public static extern CXString clang_getCompletionAnnotation(
            CXCompletionStringImpl* completion_string,
            uint annotation_number);

        [DllImport(dllName)]
        public static extern CXString clang_getCompletionParent(
            CXCompletionStringImpl* completion_string,
            CXCursorKind* kind);

        [DllImport(dllName)]
        public static extern CXString clang_getCompletionBriefComment(
            CXCompletionStringImpl* completion_string);

        [DllImport(dllName)]
        public static extern CXCompletionStringImpl* clang_getCursorCompletionString(
            CXCursor cursor);

        [DllImport(dllName)]
        public static extern uint clang_defaultCodeCompleteOptions();

        [DllImport(dllName)]
        public static extern CXCodeCompleteResults* clang_codeCompleteAt(
            CXTranslationUnitImpl* TU,
            sbyte* complete_filename,
            uint complete_line,
            uint complete_column,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            uint options);

        [DllImport(dllName)]
        public static extern void clang_sortCodeCompletionResults(
            CXCompletionResult* Results,
            uint NumResults);

        [DllImport(dllName)]
        public static extern void clang_disposeCodeCompleteResults(
            CXCodeCompleteResults* Results);

        [DllImport(dllName)]
        public static extern uint clang_codeCompleteGetNumDiagnostics(
            CXCodeCompleteResults* Results);

        [DllImport(dllName)]
        public static extern CXDiagnosticImpl* clang_codeCompleteGetDiagnostic(
            CXCodeCompleteResults* Results,
            uint Index);

        [DllImport(dllName)]
        public static extern ulong clang_codeCompleteGetContexts(
            CXCodeCompleteResults* Results);

        [DllImport(dllName)]
        public static extern CXCursorKind clang_codeCompleteGetContainerKind(
            CXCodeCompleteResults* Results,
            uint* IsIncomplete);

        [DllImport(dllName)]
        public static extern CXString clang_codeCompleteGetContainerUSR(
            CXCodeCompleteResults* Results);

        [DllImport(dllName)]
        public static extern CXString clang_codeCompleteGetObjCSelector(
            CXCodeCompleteResults* Results);

        [DllImport(dllName)]
        public static extern CXString clang_getClangVersion();

        [DllImport(dllName)]
        public static extern void clang_toggleCrashRecovery(
            uint isEnabled);

        [DllImport(dllName)]
        public static extern void clang_getInclusions(
            CXTranslationUnitImpl* tu,
            IntPtr visitor,
            CXClientDataImpl* client_data);

        [DllImport(dllName)]
        public static extern CXEvalResultImpl* clang_Cursor_Evaluate(
            CXCursor C);

        [DllImport(dllName)]
        public static extern CXEvalResultKind clang_EvalResult_getKind(
            CXEvalResultImpl* E);

        [DllImport(dllName)]
        public static extern int clang_EvalResult_getAsInt(
            CXEvalResultImpl* E);

        [DllImport(dllName)]
        public static extern double clang_EvalResult_getAsDouble(
            CXEvalResultImpl* E);

        [DllImport(dllName)]
        public static extern sbyte* clang_EvalResult_getAsStr(
            CXEvalResultImpl* E);

        [DllImport(dllName)]
        public static extern void clang_EvalResult_dispose(
            CXEvalResultImpl* E);

        [DllImport(dllName)]
        public static extern CXRemappingImpl* clang_getRemappings(
            sbyte* path);

        [DllImport(dllName)]
        public static extern CXRemappingImpl* clang_getRemappingsFromFileList(
            sbyte** filePaths,
            uint numFiles);

        [DllImport(dllName)]
        public static extern uint clang_remap_getNumFiles(
            CXRemappingImpl* arg1);

        [DllImport(dllName)]
        public static extern void clang_remap_getFilenames(
            CXRemappingImpl* arg1,
            uint index,
            CXString* original,
            CXString* transformed);

        [DllImport(dllName)]
        public static extern void clang_remap_dispose(
            CXRemappingImpl* arg1);

        [DllImport(dllName)]
        public static extern CXResult clang_findReferencesInFile(
            CXCursor cursor,
            CXFileImpl* file,
            CXCursorAndRangeVisitor visitor);

        [DllImport(dllName)]
        public static extern CXResult clang_findIncludesInFile(
            CXTranslationUnitImpl* TU,
            CXFileImpl* file,
            CXCursorAndRangeVisitor visitor);

        [DllImport(dllName)]
        public static extern int clang_index_isEntityObjCContainerKind(
            CXIdxEntityKind arg1);

        [DllImport(dllName)]
        public static extern CXIdxObjCContainerDeclInfo* clang_index_getObjCContainerDeclInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxObjCInterfaceDeclInfo* clang_index_getObjCInterfaceDeclInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxObjCCategoryDeclInfo* clang_index_getObjCCategoryDeclInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxObjCProtocolRefListInfo* clang_index_getObjCProtocolRefListInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxObjCPropertyDeclInfo* clang_index_getObjCPropertyDeclInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxIBOutletCollectionAttrInfo* clang_index_getIBOutletCollectionAttrInfo(
            CXIdxAttrInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxCXXClassDeclInfo* clang_index_getCXXClassDeclInfo(
            CXIdxDeclInfo* arg1);

        [DllImport(dllName)]
        public static extern CXIdxClientContainerImpl* clang_index_getClientContainer(
            CXIdxContainerInfo* arg1);

        [DllImport(dllName)]
        public static extern void clang_index_setClientContainer(
            CXIdxContainerInfo* arg1,
            CXIdxClientContainerImpl* arg2);

        [DllImport(dllName)]
        public static extern CXIdxClientEntityImpl* clang_index_getClientEntity(
            CXIdxEntityInfo* arg1);

        [DllImport(dllName)]
        public static extern void clang_index_setClientEntity(
            CXIdxEntityInfo* arg1,
            CXIdxClientEntityImpl* arg2);

        [DllImport(dllName)]
        public static extern CXIndexActionImpl* clang_IndexAction_create(
            CXIndexImpl* CIdx);

        [DllImport(dllName)]
        public static extern void clang_IndexAction_dispose(
            CXIndexActionImpl* arg1);

        [DllImport(dllName)]
        public static extern int clang_indexSourceFile(
            CXIndexActionImpl* arg1,
            CXClientDataImpl* client_data,
            IndexerCallbacks* index_callbacks,
            uint index_callbacks_size,
            uint index_options,
            sbyte* source_filename,
            sbyte** command_line_args,
            int num_command_line_args,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            CXTranslationUnitImpl** out_TU,
            uint TU_options);

        [DllImport(dllName)]
        public static extern int clang_indexSourceFileFullArgv(
            CXIndexActionImpl* arg1,
            CXClientDataImpl* client_data,
            IndexerCallbacks* index_callbacks,
            uint index_callbacks_size,
            uint index_options,
            sbyte* source_filename,
            sbyte** command_line_args,
            int num_command_line_args,
            CXUnsavedFile* unsaved_files,
            uint num_unsaved_files,
            CXTranslationUnitImpl** out_TU,
            uint TU_options);

        [DllImport(dllName)]
        public static extern int clang_indexTranslationUnit(
            CXIndexActionImpl* arg1,
            CXClientDataImpl* client_data,
            IndexerCallbacks* index_callbacks,
            uint index_callbacks_size,
            uint index_options,
            CXTranslationUnitImpl* arg6);

        [DllImport(dllName)]
        public static extern void clang_indexLoc_getFileLocation(
            CXIdxLoc loc,
            CXIdxClientFileImpl** indexFile,
            CXFileImpl** file,
            uint* line,
            uint* column,
            uint* offset);

        [DllImport(dllName)]
        public static extern CXSourceLocation clang_indexLoc_getCXSourceLocation(
            CXIdxLoc loc);

        [DllImport(dllName)]
        public static extern uint clang_Type_visitFields(
            CXType T,
            IntPtr visitor,
            CXClientDataImpl* client_data);
    }
}
