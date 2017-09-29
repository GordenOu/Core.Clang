namespace Core.Clang
{
    /// <summary>
    /// Describes the kind of type.
    /// </summary>
    [EnumMapping(typeof(CXTypeKind),
        Prefix = "CXType_",
        Excluded = new object[]
        {
            CXTypeKind.CXType_ObjCId,
            CXTypeKind.CXType_ObjCClass,
            CXTypeKind.CXType_ObjCSel,

            CXTypeKind.CXType_Half,

            CXTypeKind.CXType_Complex,

            CXTypeKind.CXType_BlockPointer,

            CXTypeKind.CXType_ObjCInterface,
            CXTypeKind.CXType_ObjCObjectPointer,

            CXTypeKind.CXType_FunctionNoProto,

            CXTypeKind.CXType_Vector,

            CXTypeKind.CXType_Pipe,
            CXTypeKind.CXType_OCLImage1dRO,
            CXTypeKind.CXType_OCLImage1dArrayRO,
            CXTypeKind.CXType_OCLImage1dBufferRO,
            CXTypeKind.CXType_OCLImage2dRO,
            CXTypeKind.CXType_OCLImage2dArrayRO,
            CXTypeKind.CXType_OCLImage2dDepthRO,
            CXTypeKind.CXType_OCLImage2dArrayDepthRO,
            CXTypeKind.CXType_OCLImage2dMSAARO,
            CXTypeKind.CXType_OCLImage2dArrayMSAARO,
            CXTypeKind.CXType_OCLImage2dMSAADepthRO,
            CXTypeKind.CXType_OCLImage2dArrayMSAADepthRO,
            CXTypeKind.CXType_OCLImage3dRO,
            CXTypeKind.CXType_OCLImage1dWO,
            CXTypeKind.CXType_OCLImage1dArrayWO,
            CXTypeKind.CXType_OCLImage1dBufferWO,
            CXTypeKind.CXType_OCLImage2dWO,
            CXTypeKind.CXType_OCLImage2dArrayWO,
            CXTypeKind.CXType_OCLImage2dDepthWO,
            CXTypeKind.CXType_OCLImage2dArrayDepthWO,
            CXTypeKind.CXType_OCLImage2dMSAAWO,
            CXTypeKind.CXType_OCLImage2dArrayMSAAWO,
            CXTypeKind.CXType_OCLImage2dMSAADepthWO,
            CXTypeKind.CXType_OCLImage2dArrayMSAADepthWO,
            CXTypeKind.CXType_OCLImage3dWO,
            CXTypeKind.CXType_OCLImage1dRW,
            CXTypeKind.CXType_OCLImage1dArrayRW,
            CXTypeKind.CXType_OCLImage1dBufferRW,
            CXTypeKind.CXType_OCLImage2dRW,
            CXTypeKind.CXType_OCLImage2dArrayRW,
            CXTypeKind.CXType_OCLImage2dDepthRW,
            CXTypeKind.CXType_OCLImage2dArrayDepthRW,
            CXTypeKind.CXType_OCLImage2dMSAARW,
            CXTypeKind.CXType_OCLImage2dArrayMSAARW,
            CXTypeKind.CXType_OCLImage2dMSAADepthRW,
            CXTypeKind.CXType_OCLImage2dArrayMSAADepthRW,
            CXTypeKind.CXType_OCLImage3dRW,
            CXTypeKind.CXType_OCLSampler,
            CXTypeKind.CXType_OCLEvent,
            CXTypeKind.CXType_OCLQueue,
            CXTypeKind.CXType_OCLReserveID
        })]
    public enum TypeKind
    {
        /// <summary>
        /// Represents an invalid type (e.g., where no type is available).
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// A type whose specific kind is not exposed via this interface.
        /// </summary>
        Unexposed = 1,

        #region Builtin types

        /// <summary>
        /// void.
        /// </summary>
        Void = 2,

        /// <summary>
        /// 'bool' in C++, '_Bool' in C99.
        /// </summary>
        Bool = 3,

        /// <summary>
        /// 'char' for targets where it's unsigned.
        /// </summary>
        Char_U = 4,

        /// <summary>
        /// 'unsigned char', explicitly qualified.
        /// </summary>
        UChar = 5,

        /// <summary>
        /// 'char16_t' in C++.
        /// </summary>
        Char16 = 6,

        /// <summary>
        /// 'char32_t' in C++.
        /// </summary>
        Char32 = 7,

        /// <summary>
        /// 'unsigned short'.
        /// </summary>
        UShort = 8,

        /// <summary>
        /// 'unsigned int'.
        /// </summary>
        UInt = 9,

        /// <summary>
        /// 'unsigned long'.
        /// </summary>
        ULong = 10,

        /// <summary>
        /// 'unsigned long long'.
        /// </summary>
        ULongLong = 11,

        /// <summary>
        /// '__uint128_t'.
        /// </summary>
        UInt128 = 12,

        /// <summary>
        /// 'char' for targets where it's signed.
        /// </summary>
        Char_S = 13,

        /// <summary>
        /// 'signed char', explicitly qualified.
        /// </summary>
        SChar = 14,

        /// <summary>
        /// 'wchar_t'.
        /// </summary>
        WChar = 15,

        /// <summary>
        /// 'short' or 'signed short'.
        /// </summary>
        Short = 16,

        /// <summary>
        /// 'int' or 'signed int'.
        /// </summary>
        Int = 17,

        /// <summary>
        /// 'long' or 'signed long'.
        /// </summary>
        Long = 18,

        /// <summary>
        /// 'long long' or 'signed long long'.
        /// </summary>
        LongLong = 19,

        /// <summary>
        /// '__int128_t'.
        /// </summary>
        Int128 = 20,

        /// <summary>
        /// 'float'.
        /// </summary>
        Float = 21,

        /// <summary>
        /// 'double'.
        /// </summary>
        Double = 22,

        /// <summary>
        /// 'long double'.
        /// </summary>
        LongDouble = 23,

        /// <summary>
        /// This is the type of C++0x 'nullptr'.
        /// </summary>
        NullPtr = 24,

        /// <summary>
        /// The type of an unresolved overload set. A placeholder type. Expressions with this type
        /// have one of the following basic forms, with parentheses generally permitted:<para />
        /// foo # possibly qualified, not if an implicit access<para />
        /// &amp;foo # possibly qualified, not if an implicit access<para />
        /// x->foo # only if might be a static member function<para />
        /// &amp;x->foo # only if might be a static member function<para />
        /// &amp;Class::foo # when a pointer-to-member; sub-expr also has this type<para />
        /// </summary>
        Overload = 25,

        /// <summary>
        /// This represents the type of an expression whose type is totally unknown, e.g. 'T::foo'.
        /// It is permitted for this to appear in situations where the structure of the type is
        /// theoretically deducible.
        /// </summary>
        Dependent = 26,

        /// <summary>
        /// '__float128'.
        /// </summary>
        Float128 = 30,

        #endregion

        /// <summary>
        /// PointerType - C99 6.7.5.1 - Pointer Declarators.
        /// </summary>
        Pointer = 101,

        /// <summary>
        /// An lvalue reference type, per C++11 [dcl.ref].
        /// </summary>
        LValueReference = 103,

        /// <summary>
        /// An rvalue reference type, per C++11 [dcl.ref].
        /// </summary>
        RValueReference = 104,

        /// <summary>
        /// Classes containing a sequence of objects of various types.
        /// </summary>
        Record = 105,

        /// <summary>
        /// Enumerations, which comprise a set of named constant values.
        /// </summary>
        Enum = 106,

        /// <summary>
        /// A typedef specifier.
        /// </summary>
        Typedef = 107,

        /// <summary>
        /// Represents a prototype with parameter type info, e.g. 'int foo(int)' or 'int foo(void)'.
        /// 'void' is represented as having no parameters, not as having a single void parameter.
        /// Such a type can have an exception specification, but this specification is not part of
        /// the canonical type.
        /// </summary>
        FunctionProto = 111,

        /// <summary>
        /// Represents the canonical version of C arrays with a specified constant size.
        /// </summary>
        ConstantArray = 112,

        /// <summary>
        /// Represents a C array with an unspecified size.
        /// </summary>
        IncompleteArray = 114,

        /// <summary>
        /// Represents a C array with a specified size that is not an integer-constant-expression.
        /// </summary>
        VariableArray = 115,

        /// <summary>
        /// Represents an array type in C++ whose size is a value-dependent expression.
        /// </summary>
        /// <example>
        /// <code>
        /// template&lt;typename T, int Size&gt;
        /// class array
        /// {
        ///     T data[Size];
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// For these types, the array bound is unknown until template instantiation occurs, at
        /// which point this will become either a ConstantArrayType or a VariableArrayType.
        /// </remarks>
        DependentSizedArray = 116,

        /// <summary>
        /// A pointer to member type per C++ 8.3.3 - Pointers to members.
        /// </summary>
        /// <remarks>
        /// This includes both pointers to data members and pointer to member functions.
        /// </remarks>
        MemberPointer = 117,

        /// <summary>
        /// Represents a C++11 auto or C++14 decltype(auto) type.
        /// </summary>
        /// <remarks>
        /// These types are usually a placeholder for a deduced type. However, before the
        /// initializer is attached, or if the initializer is type-dependent, there is no deduced
        /// type and an auto type is canonical. In the latter case, it is also a dependent type.
        /// </remarks>
        Auto = 118,

        /// <summary>
        /// Represents a type that was referred to using an elaborated type keyword.
        /// </summary>
        /// <remarks>
        /// E.g., struct S, or via a qualified name, e.g., N::M::type, or both.
        /// </remarks>
        Elaborated = 119
    }
}
