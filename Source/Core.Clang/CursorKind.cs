using System.ComponentModel;

namespace Core.Clang
{
    /// <summary>
    /// Describes the kind of entity that a cursor refers to.
    /// </summary>
    [EnumMapping(typeof(CXCursorKind),
        Prefix = "CXCursor_",
        Excluded = new object[]
        {
            CXCursorKind.CXCursor_ObjCInterfaceDecl,
            CXCursorKind.CXCursor_ObjCCategoryDecl,
            CXCursorKind.CXCursor_ObjCProtocolDecl,
            CXCursorKind.CXCursor_ObjCPropertyDecl,
            CXCursorKind.CXCursor_ObjCIvarDecl,
            CXCursorKind.CXCursor_ObjCInstanceMethodDecl,
            CXCursorKind.CXCursor_ObjCClassMethodDecl,
            CXCursorKind.CXCursor_ObjCImplementationDecl,
            CXCursorKind.CXCursor_ObjCCategoryImplDecl,

            CXCursorKind.CXCursor_ObjCSynthesizeDecl,
            CXCursorKind.CXCursor_ObjCDynamicDecl,

            CXCursorKind.CXCursor_ObjCSuperClassRef,
            CXCursorKind.CXCursor_ObjCProtocolRef,
            CXCursorKind.CXCursor_ObjCClassRef,

            CXCursorKind.CXCursor_ObjCMessageExpr,

            CXCursorKind.CXCursor_AddrLabelExpr,
            CXCursorKind.CXCursor_StmtExpr,
            CXCursorKind.CXCursor_GenericSelectionExpr,
            CXCursorKind.CXCursor_GNUNullExpr,

            CXCursorKind.CXCursor_ObjCStringLiteral,
            CXCursorKind.CXCursor_ObjCEncodeExpr,
            CXCursorKind.CXCursor_ObjCSelectorExpr,
            CXCursorKind.CXCursor_ObjCProtocolExpr,
            CXCursorKind.CXCursor_ObjCBridgedCastExpr,

            CXCursorKind.CXCursor_ObjCBoolLiteralExpr,
            CXCursorKind.CXCursor_ObjCSelfExpr,
            CXCursorKind.CXCursor_OMPArraySectionExpr,
            CXCursorKind.CXCursor_ObjCAvailabilityCheckExpr,

            CXCursorKind.CXCursor_GCCAsmStmt,
            CXCursorKind.CXCursor_ObjCAtTryStmt,
            CXCursorKind.CXCursor_ObjCAtCatchStmt,
            CXCursorKind.CXCursor_ObjCAtFinallyStmt,
            CXCursorKind.CXCursor_ObjCAtThrowStmt,
            CXCursorKind.CXCursor_ObjCAtSynchronizedStmt,
            CXCursorKind.CXCursor_ObjCAutoreleasePoolStmt,
            CXCursorKind.CXCursor_ObjCForCollectionStmt,

            CXCursorKind.CXCursor_SEHTryStmt,
            CXCursorKind.CXCursor_SEHExceptStmt,
            CXCursorKind.CXCursor_SEHFinallyStmt,
            CXCursorKind.CXCursor_MSAsmStmt,

            CXCursorKind.CXCursor_OMPParallelDirective,
            CXCursorKind.CXCursor_OMPSimdDirective,
            CXCursorKind.CXCursor_OMPForDirective,
            CXCursorKind.CXCursor_OMPSectionsDirective,
            CXCursorKind.CXCursor_OMPSectionDirective,
            CXCursorKind.CXCursor_OMPSingleDirective,
            CXCursorKind.CXCursor_OMPParallelForDirective,
            CXCursorKind.CXCursor_OMPParallelSectionsDirective,
            CXCursorKind.CXCursor_OMPTaskDirective,
            CXCursorKind.CXCursor_OMPMasterDirective,
            CXCursorKind.CXCursor_OMPCriticalDirective,
            CXCursorKind.CXCursor_OMPTaskyieldDirective,
            CXCursorKind.CXCursor_OMPBarrierDirective,
            CXCursorKind.CXCursor_OMPTaskwaitDirective,
            CXCursorKind.CXCursor_OMPFlushDirective,
            CXCursorKind.CXCursor_SEHLeaveStmt,
            CXCursorKind.CXCursor_OMPOrderedDirective,
            CXCursorKind.CXCursor_OMPAtomicDirective,
            CXCursorKind.CXCursor_OMPForSimdDirective,
            CXCursorKind.CXCursor_OMPParallelForSimdDirective,
            CXCursorKind.CXCursor_OMPTargetDirective,
            CXCursorKind.CXCursor_OMPTeamsDirective,
            CXCursorKind.CXCursor_OMPTaskgroupDirective,
            CXCursorKind.CXCursor_OMPCancellationPointDirective,
            CXCursorKind.CXCursor_OMPCancelDirective,
            CXCursorKind.CXCursor_OMPTargetDataDirective,
            CXCursorKind.CXCursor_OMPTaskLoopDirective,
            CXCursorKind.CXCursor_OMPTaskLoopSimdDirective,
            CXCursorKind.CXCursor_OMPDistributeDirective,
            CXCursorKind.CXCursor_OMPTargetEnterDataDirective,
            CXCursorKind.CXCursor_OMPTargetExitDataDirective,
            CXCursorKind.CXCursor_OMPTargetParallelDirective,
            CXCursorKind.CXCursor_OMPTargetParallelForDirective,
            CXCursorKind.CXCursor_OMPTargetUpdateDirective,
            CXCursorKind.CXCursor_OMPDistributeParallelForDirective,
            CXCursorKind.CXCursor_OMPDistributeParallelForSimdDirective,
            CXCursorKind.CXCursor_OMPDistributeSimdDirective,
            CXCursorKind.CXCursor_OMPTargetParallelForSimdDirective,
            CXCursorKind.CXCursor_OMPTargetSimdDirective,
            CXCursorKind.CXCursor_OMPTeamsDistributeDirective,
            CXCursorKind.CXCursor_OMPTeamsDistributeSimdDirective,
            CXCursorKind.CXCursor_OMPTeamsDistributeParallelForSimdDirective,
            CXCursorKind.CXCursor_OMPTeamsDistributeParallelForDirective,
            CXCursorKind.CXCursor_OMPTargetTeamsDirective,
            CXCursorKind.CXCursor_OMPTargetTeamsDistributeDirective,
            CXCursorKind.CXCursor_OMPTargetTeamsDistributeParallelForDirective,
            CXCursorKind.CXCursor_OMPTargetTeamsDistributeParallelForSimdDirective,
            CXCursorKind.CXCursor_OMPTargetTeamsDistributeSimdDirective,

            CXCursorKind.CXCursor_IBActionAttr,
            CXCursorKind.CXCursor_IBOutletAttr,
            CXCursorKind.CXCursor_IBOutletCollectionAttr,
            CXCursorKind.CXCursor_CXXFinalAttr,
            CXCursorKind.CXCursor_CXXOverrideAttr,
            CXCursorKind.CXCursor_AnnotateAttr,

            CXCursorKind.CXCursor_PackedAttr,
            CXCursorKind.CXCursor_PureAttr,
            CXCursorKind.CXCursor_ConstAttr,
            CXCursorKind.CXCursor_NoDuplicateAttr,

            CXCursorKind.CXCursor_VisibilityAttr,

            CXCursorKind.CXCursor_TypeAliasTemplateDecl
        })]
    public enum CursorKind
    {
        #region Declarations

        /// <summary>
        /// A declaration whose specific kind is not exposed via this interface.
        /// </summary>
        /// <remarks>
        /// Unexposed declarations have the same operations as any other kind of declaration; one
        /// can extract their location information, spelling, find their definitions, etc. However,
        /// the specific kind of the declaration is not reported.
        /// </remarks>
        UnexposedDecl = 1,

        /// <summary>
        /// A C or C++ struct.
        /// </summary>
        StructDecl = 2,

        /// <summary>
        /// A C or C++ union.
        /// </summary>
        UnionDecl = 3,

        /// <summary>
        /// A C++ class.
        /// </summary>
        ClassDecl = 4,

        /// <summary>
        /// An enumeration.
        /// </summary>
        EnumDecl = 5,

        /// <summary>
        /// A field (in C) or non-static data member (in C++) in a struct, union, or C++ class.
        /// </summary>
        FieldDecl = 6,

        /// <summary>
        /// An enumerator constant.
        /// </summary>
        EnumConstantDecl = 7,

        /// <summary>
        /// A function.
        /// </summary>
        FunctionDecl = 8,

        /// <summary>
        /// A variable.
        /// </summary>
        VarDecl = 9,

        /// <summary>
        /// A function or method parameter.
        /// </summary>
        ParmDecl = 10,

        /// <summary>
        /// A typedef.
        /// </summary>
        TypedefDecl = 20,

        /// <summary>
        /// A C++ class method.
        /// </summary>
        CXXMethod = 21,

        /// <summary>
        /// A C++ namespace.
        /// </summary>
        Namespace = 22,

        /// <summary>
        /// A linkage specification, e.g. 'extern "C"'.
        /// </summary>
        LinkageSpec = 23,

        /// <summary>
        /// A C++ constructor.
        /// </summary>
        Constructor = 24,

        /// <summary>
        /// A C++ destructor.
        /// </summary>
        Destructor = 25,

        /// <summary>
        /// A C++ conversion function.
        /// </summary>
        ConversionFunction = 26,

        /// <summary>
        /// A C++ template type parameter.
        /// </summary>
        TemplateTypeParameter = 27,

        /// <summary>
        /// A C++ non-type template parameter.
        /// </summary>
        NonTypeTemplateParameter = 28,

        /// <summary>
        /// A C++ template template parameter. 
        /// </summary>
        TemplateTemplateParameter = 29,

        /// <summary>
        /// A C++ function template.
        /// </summary>
        FunctionTemplate = 30,

        /// <summary>
        /// A C++ class template.
        /// </summary>
        ClassTemplate = 31,

        /// <summary>
        /// A C++ class template partial specialization.
        /// </summary>
        ClassTemplatePartialSpecialization = 32,

        /// <summary>
        /// A C++ namespace alias declaration.
        /// </summary>
        NamespaceAlias = 33,

        /// <summary>
        /// A C++ using directive.
        /// </summary>
        UsingDirective = 34,

        /// <summary>
        /// A C++ using declaration.
        /// </summary>
        UsingDeclaration = 35,

        /// <summary>
        /// A C++ alias declaration
        /// </summary>
        TypeAliasDecl = 36,

        /// <summary>
        /// An access specifier. 
        /// </summary>
        CXXAccessSpecifier = 39,

        #endregion

        #region References

        /// <summary>
        /// A reference to a type declaration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A type reference occurs anywhere where a type is named but not declared. For example,
        /// given:
        /// </para>
        /// <code>
        /// typedef unsigned size_type;
        /// size_type size;
        /// </code>
        /// <para>
        /// The typedef is a declaration of size_type (<see cref="TypedefDecl"/>), while the type
        /// of the variable "size" is referenced. The cursor referenced by the type of size is the
        /// typedef for size_type.
        /// </para>
        /// </remarks>
        TypeRef = 43,

        /// <summary>
        /// A C++ base class specifier.
        /// </summary>
        CXXBaseSpecifier = 44,

        /// <summary>
        /// A reference to a class template, function template, template parameter, or class
        /// template partial specialization.
        /// </summary>
        TemplateRef = 45,

        /// <summary>
        /// A reference to a namespace or namespace alias.
        /// </summary>
        NamespaceRef = 46,

        /// <summary>
        /// A reference to a member of a struct, union, or class that occurs in some non-expression
        /// context, e.g., a designated initializer.
        /// </summary>
        MemberRef = 47,

        /// <summary>
        /// A reference to a labeled statement.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This cursor kind is used to describe the jump to "start_over" in the  goto statement in
        /// the following example:
        /// </para>
        /// <code>
        /// start_over: 
        ///     ++counter;
        ///     
        ///     goto start_over;
        /// </code>
        /// <para>
        /// A label reference cursor refers to a label statement.
        /// </para>
        /// </remarks>
        LabelRef = 48,

        /// <summary>
        /// A reference to a set of overloaded functions or function templates that has not yet
        /// been resolved to a specific function or function template.
        /// </summary>
        /// <remarks>
        /// <para>
        /// An overloaded declaration reference cursor occurs in C++ templates where a dependent
        /// name refers to a function. For example:
        /// </para>
        /// <code>
        /// template&lt;typename T&gt; void swap(T&amp;, T&amp;);
        /// 
        /// struct X { ... };
        /// void swap(X&amp;, X&amp;);
        /// 
        /// template&lt;typename T&gt;
        /// void reverse(T* first, T* last)
        /// {
        ///     while (first &lt; last - 1)
        ///     {
        ///         swap(*first, *--last);
        ///         ++first;
        ///     }
        /// }
        /// 
        /// struct Y { };
        /// void swap(Y&amp;, Y&amp;);
        /// </code>
        /// <para>
        /// Here, the identifier "swap" is associated with an overloaded declaration reference. In
        /// the template definition, "swap" refers to either of the two "swap" functions declared
        /// above, so both results will be available. At instantiation time, "swap" may also refer
        /// to other functions found via argument-dependent lookup (e.g., the "swap" function at
        /// the end of the example).
        /// </para>
        /// <para>
        /// The functions <see cref="Cursor.GetNumOverloadedDecls"/> and 
        /// <see cref="Cursor.GetOverloadedDecl(uint)"/> can be used to retrieve the definitions
        /// referenced by this cursor.
        /// </para>
        /// </remarks>
        OverloadedDeclRef = 49,

        /// <summary>
        /// A reference to a variable that occurs in some non-expression context, e.g., a C++
        /// lambda capture list.
        /// </summary>
        VariableRef = 50,

        #endregion

        #region Error conditions

        /// <summary>
        /// Invalid File.
        /// </summary>
        InvalidFile = 70,

        /// <summary>
        /// No declaration found.
        /// </summary>
        NoDeclFound = 71,

        /// <summary>
        /// Not implemented.
        /// </summary>
        NotImplemented = 72,

        /// <summary>
        /// Invalid code.
        /// </summary>
        InvalidCode = 73,

        #endregion

        #region Expressions

        /// <summary>
        /// An expression whose specific kind is not exposed via this interface.
        /// </summary>
        /// <remarks>
        /// Unexposed expressions have the same operations as any other kind of expression; one can
        /// extract their location information, spelling, children, etc. However, the specific kind
        /// of the expression is not reported.
        /// </remarks>
        UnexposedExpr = 100,

        /// <summary>
        /// An expression that refers to some value declaration, such as a function, variable, or
        /// enumerator.
        /// </summary>
        DeclRefExpr = 101,

        /// <summary>
        /// An expression that refers to a member of a struct, union, class, etc.
        /// </summary>
        MemberRefExpr = 102,

        /// <summary>
        /// An expression that calls a function.
        /// </summary>
        CallExpr = 103,

        /// <summary>
        /// An expression that represents a block literal.
        /// </summary>
        BlockExpr = 105,

        /// <summary>
        /// An integer literal.
        /// </summary>
        IntegerLiteral = 106,

        /// <summary>
        /// A floating point number literal.
        /// </summary>
        FloatingLiteral = 107,

        /// <summary>
        /// An imaginary number literal.
        /// </summary>
        ImaginaryLiteral = 108,

        /// <summary>
        /// A string literal.
        /// </summary>
        StringLiteral = 109,

        /// <summary>
        /// A character literal.
        /// </summary>
        CharacterLiteral = 110,

        /// <summary>
        /// A parenthesized expression, e.g. "(1)".
        /// </summary>
        /// <remarks>
        /// This AST node is only formed if full location information is requested.
        /// </remarks>
        ParenExpr = 111,

        /// <summary>
        /// This represents the unary-expression's (except sizeof and alignof).
        /// </summary>
        UnaryOperator = 112,

        /// <summary>
        /// [C99 6.5.2.1] Array Subscripting.
        /// </summary>
        ArraySubscriptExpr = 113,

        /// <summary>
        /// A builtin binary operation expression such as "x + y" or "x &lt;= y".
        /// </summary>
        BinaryOperator = 114,

        /// <summary>
        /// Compound assignment such as "+=".
        /// </summary>
        CompoundAssignOperator = 115,

        /// <summary>
        /// The ?: ternary operator.
        /// </summary>
        ConditionalOperator = 116,

        /// <summary>
        /// An explicit cast in C (C99 6.5.4) or a C-style cast in C++ (C++ [expr.cast]), which
        /// uses the syntax (Type)expr.
        /// </summary>
        CStyleCastExpr = 117,

        /// <summary>
        /// [C99 6.5.2.5]
        /// </summary>
        CompoundLiteralExpr = 118,

        /// <summary>
        /// Describes an C or C++ initializer list.
        /// </summary>
        InitListExpr = 119,

        /// <summary>
        /// C++'s static_cast&lt;&gt; expression.
        /// </summary>
        CXXStaticCastExpr = 124,

        /// <summary>
        /// C++'s dynamic_cast&lt;&gt; expression.
        /// </summary>
        CXXDynamicCastExpr = 125,

        /// <summary>
        /// C++'s reinterpret_cast&lt;&gt; expression.
        /// </summary>
        CXXReinterpretCastExpr = 126,

        /// <summary>
        /// C++'s const_cast&lt;&gt; expression.
        /// </summary>
        CXXConstCastExpr = 127,

        /// <summary>
        /// Represents an explicit C++ type conversion that uses "functional" notion (C++
        /// [expr.type.conv]).
        /// </summary>
        /// <example>
        /// <code>
        /// x = int(0.5);
        /// </code>
        /// </example>
        CXXFunctionalCastExpr = 128,

        /// <summary>
        /// A C++ typeid expression (C++ [expr.typeid]).
        /// </summary>
        CXXTypeidExpr = 129,

        /// <summary>
        /// [C++ 2.13.5] C++ Boolean Literal.
        /// </summary>
        CXXBoolLiteralExpr = 130,

        /// <summary>
        /// [C++0x 2.14.7] C++ Pointer Literal.
        /// </summary>
        CXXNullPtrLiteralExpr = 131,

        /// <summary>
        /// Represents the "this" expression in C++
        /// </summary>
        CXXThisExpr = 132,

        /// <summary>
        /// [C++ 15] C++ Throw Expression.
        /// </summary>
        /// <remarks>
        /// This handles 'throw' and 'throw' assignment-expression. When assignment-expression
        /// isn't present, Op will be null.
        /// </remarks>
        CXXThrowExpr = 133,

        /// <summary>
        /// A new expression for memory allocation and constructor calls, e.g. 
        /// "new CXXNewExpr(foo)".
        /// </summary>
        CXXNewExpr = 134,

        /// <summary>
        /// A delete expression for memory deallocation and destructor calls, e.g.
        /// "delete[] pArray".
        /// </summary>
        CXXDeleteExpr = 135,

        /// <summary>
        /// A unary expression (noexcept, sizeof, or other traits).
        /// </summary>
        UnaryExpr = 136,

        /// <summary>
        /// Represents a C++0x pack expansion that produces a sequence of expressions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A pack expansion expression contains a pattern (which itself is an expression) followed
        /// by an ellipsis. For example:
        /// </para>
        /// <code>
        /// template&lt;typename F, typename ...Types&gt;
        /// void forward(F f, Types &amp;&amp;...args) 
        /// {
        ///     f(static_cast&lt;Types&amp;&amp;&gt;(args)...);
        /// }
        /// </code>
        /// </remarks>
        PackExpansionExpr = 142,

        /// <summary>
        /// Represents an expression that computes the length of a parameter pack.
        /// </summary>
        /// <remarks>
        /// <code>
        /// template&lt;typename ...Types&gt; 
        /// struct count 
        /// { 
        ///     static const unsigned value = sizeof...(Types);
        /// };
        /// </code>
        /// </remarks>
        SizeOfPackExpr = 143,

        /// <summary>
        /// Represents a C++ lambda expression that produces a local function object.
        /// </summary>
        /// <remarks>
        /// <code>
        /// void abssort(float* x, unsigned N) 
        /// {
        ///     std::sort(x, x + N, [](float a, float b) 
        ///     {
        ///         return std::abs(a) &lt; std::abs(b); 
        ///     });
        ///}
        /// </code>
        /// </remarks>
        LambdaExpr = 144,

        #endregion

        #region Statements

        /// <summary>
        /// A statement whose specific kind is not exposed via this interface.
        /// </summary>
        /// <remarks>
        /// Unexposed statements have the same operations as any other kind of statement; one can
        /// extract their location information, spelling, children, etc. However, the specific kind
        /// of the statement is not reported.
        /// </remarks>
        UnexposedStmt = 200,

        /// <summary>
        /// A labeled statement in a function. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// This cursor kind is used to describe the "start_over:" label statement in the following
        /// example:
        /// </para>
        /// <code>
        /// start_over:
        ///     ++counter;
        /// </code>
        /// </remarks>
        LabelStmt = 201,

        /// <summary>
        /// A group of statements like { stmt stmt }.
        /// </summary>
        /// <remarks>
        /// This cursor kind is used to describe compound statements, e.g. function bodies.
        /// </remarks>
        CompoundStmt = 202,

        /// <summary>
        /// A case statement.
        /// </summary>
        CaseStmt = 203,

        /// <summary>
        /// A default statement.
        /// </summary>
        DefaultStmt = 204,

        /// <summary>
        /// An if statement.
        /// </summary>
        IfStmt = 205,

        /// <summary>
        /// A switch statement.
        /// </summary>
        SwitchStmt = 206,

        /// <summary>
        /// A while statement.
        /// </summary>
        WhileStmt = 207,

        /// <summary>
        /// A do statement.
        /// </summary>
        DoStmt = 208,

        /// <summary>
        /// A for statement.
        /// </summary>
        ForStmt = 209,

        /// <summary>
        /// A goto statement.
        /// </summary>
        GotoStmt = 210,

        /// <summary>
        /// An indirect goto statement.
        /// </summary>
        IndirectGotoStmt = 211,

        /// <summary>
        /// A continue statement.
        /// </summary>
        ContinueStmt = 212,

        /// <summary>
        /// A break statement.
        /// </summary>
        BreakStmt = 213,

        /// <summary>
        /// A return statement.
        /// </summary>
        ReturnStmt = 214,

        /// <summary>
        /// C++'s catch statement.
        /// </summary>
        CXXCatchStmt = 223,

        /// <summary>
        /// C++'s try statement.
        /// </summary>
        CXXTryStmt = 224,

        /// <summary>
        /// C++'s for (* : *) statement.
        /// </summary>
        CXXForRangeStmt = 225,

        /// <summary>
        /// The null statement ";": C99 6.8.3p3.
        /// </summary>
        /// <remarks>
        /// This cursor kind is used to describe the null statement.
        /// </remarks>
        NullStmt = 230,

        /// <summary>
        /// Adapter class for mixing declarations with statements and expressions.
        /// </summary>
        DeclStmt = 231,

        #endregion

        /// <summary>
        /// Cursor that represents the translation unit itself.
        /// </summary>
        /// <remarks>
        /// The translation unit cursor exists primarily to act as the root cursor for traversing
        /// the contents of a translation unit.
        /// </remarks>
        TranslationUnit = 300,

        #region Attributes

        /// <summary>
        /// An attribute whose specific kind is not exposed via this interface.
        /// </summary>
        UnexposedAttr = 400,

        /// <summary>
        /// Assembly label.
        /// </summary>
        AsmLabelAttr = 407,

        /// <summary>
        /// "__constant__ ".
        /// </summary>
        CUDAConstantAttr = 412,

        /// <summary>
        /// "__device__ ".
        /// </summary>
        CUDADeviceAttr = 413,

        /// <summary>
        /// "__global__ ".
        /// </summary>
        CUDAGlobalAttr = 414,

        /// <summary>
        /// "__host__ ".
        /// </summary>
        CUDAHostAttr = 415,

        /// <summary>
        /// "__shared__ ".
        /// </summary>
        CUDASharedAttr = 416,

        /// <summary>
        /// "__declspec(dllexport)".
        /// </summary>
        DLLExport = 418,

        /// <summary>
        /// "__declspec(dllimport)".
        /// </summary>
        DLLImport = 419,

        #endregion

        #region Preprocessing

        /// <summary>
        /// A preprocessing directive.
        /// </summary>
        PreprocessingDirective = 500,

        /// <summary>
        /// A macro definition.
        /// </summary>
        MacroDefinition = 501,

        /// <summary>
        /// A macro expansion.
        /// </summary>
        MacroExpansion = 502,

        /// <summary>
        /// A inclusion directive.
        /// </summary>
        InclusionDirective = 503,

        #endregion

        #region Extra Declarations

        /// <summary>
        /// A module import declaration.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        ModuleImportDecl = 600,

        /// <summary>
        /// A static_assert node.
        /// </summary>
        StaticAssert = 602,

        /// <summary>
        /// A friend declaration.
        /// </summary>
        FriendDecl = 603,

        #endregion

        /// <summary>
        /// A code completion overload candidate.
        /// </summary>
        OverloadCandidate = 700
    }

    /// <summary>
    /// CursorKind extensions methods for range assertion.
    /// </summary>
    public static class CursorKindExtensions
    {
        /// <summary>
        /// Determines whether the cursor kind represents a declaration.
        /// </summary>
        /// <returns>true if the cursor kind represents a declaration.</returns>
        public static bool IsDecalration(this CursorKind kind)
        {
            return NativeMethods.clang_isDeclaration((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents a simple reference.
        /// </summary>
        /// <returns>true if the cursor kind represents a simple reference.</returns>
        /// <remarks>
        /// Note that other kinds of cursors (such as expressions) can also refer to other cursors.
        /// Use <see cref="Cursor.GetCursorReferenced"/> to determine whether a particular cursor
        /// refers to another entity.
        /// </remarks>
        public static bool IsReference(this CursorKind kind)
        {
            return NativeMethods.clang_isReference((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents an expression.
        /// </summary>
        /// <returns>true if the cursor kind represents an expression.</returns>
        public static bool IsExpression(this CursorKind kind)
        {
            return NativeMethods.clang_isExpression((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents a statement.
        /// </summary>
        /// <returns>true if the cursor kind represents a statement.</returns>
        public static bool IsStatement(this CursorKind kind)
        {
            return NativeMethods.clang_isStatement((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents a attribute.
        /// </summary>
        /// <returns>true if the cursor kind represents a statement.</returns>
        public static bool IsAttribute(this CursorKind kind)
        {
            return NativeMethods.clang_isAttribute((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind is invalid.
        /// </summary>
        /// <returns>true if the cursor kind is invalid.</returns>
        public static bool IsInvalid(this CursorKind kind)
        {
            return NativeMethods.clang_isInvalid((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents a translation unit.
        /// </summary>
        /// <returns>true if the cursor kind represents a translation unit.</returns>
        public static bool IsTranslationUnit(this CursorKind kind)
        {
            return NativeMethods.clang_isTranslationUnit((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determines whether the cursor kind represents a preprocessing element, such as a
        /// preprocessor directive or macro instantiation.
        /// </summary>
        /// <returns>true if the cursor kind represents a preprocessing element.</returns>
        public static bool IsPreprocessing(this CursorKind kind)
        {
            return NativeMethods.clang_isPreprocessing((CXCursorKind)kind) != 0;
        }

        /// <summary>
        /// Determine whether the cursor kind represents a currently unexposed piece of the AST 
        /// (e.g., <see cref="CursorKind.UnexposedStmt"/>).
        /// </summary>
        /// <returns>
        /// true if the cursor kind represents a currently unexposed piece of the AST.
        /// </returns>
        public static bool IsUnexposed(this CursorKind kind)
        {
            return NativeMethods.clang_isUnexposed((CXCursorKind)kind) != 0;
        }
    }
}
