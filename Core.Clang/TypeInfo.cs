using System;
using System.Diagnostics;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// The type of an element in the abstract syntax tree.
    /// </summary>
    public unsafe class TypeInfo : IEquatable<TypeInfo>
    {
        internal CXType Struct { get; }

        /// <summary>
        /// The kind of the <see cref="TypeInfo"/>.
        /// </summary>
        public TypeKind Kind => (TypeKind)Struct.kind;

        internal TranslationUnit TranslationUnit { get; }

        internal TypeInfo(CXType cxType, TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            Struct = cxType;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Determines whether two <see cref="TypeInfo"/>s represent the same type.
        /// </summary>
        /// <param name="other">The <see cref="TypeInfo"/> to compare with this instance.</param>
        /// <returns>true if the <see cref="TypeInfo"/>s represent the same type.</returns>
        public bool Equals(TypeInfo other)
        {
            ThrowIfDisposed();

            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            other.ThrowIfDisposed();

            return NativeMethods.clang_equalTypes(Struct, other.Struct) != 0;
        }

        /// <summary>
        /// Determines whether the current instance is equal to another object, which must be of
        /// type <see cref="TypeInfo"/> and represents the same type.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>
        /// true if the current instance and the <paramref name="obj"/> parameter refer to the same
        /// type.
        /// </returns>
        public override bool Equals(object obj)
        {
            ThrowIfDisposed();

            return Equals(obj as TypeInfo);
        }

        /// <summary>
        /// Gets the hash code for this <see cref="TypeInfo"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="TypeInfo"/>.</returns>
        [Unstable(version: "4.0.0", seealso: new[]
        {
            "https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CXType.cpp"
        })]
        public override int GetHashCode()
        {
            ThrowIfDisposed();

            var cxType = Struct;
            return cxType.data[0].GetHashCode() ^ cxType.data[1].GetHashCode();
        }

        /// <summary>
        /// Pretty-print the underlying type using the rules of the language of the translation
        /// unit from which it came.
        /// </summary>
        /// <returns>If the type is invalid, an empty string is returned.</returns>
        public string GetSpelling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getTypeSpelling(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the canonical type for the <see cref="TypeInfo"/>.
        /// </summary>
        /// <returns>The canonical type for the <see cref="TypeInfo"/>.</returns>
        /// <remarks>
        /// Clang's type system explicitly models typedefs and all the ways a specific type can be
        /// represented. The canonical type is the underlying type with all the "sugar" removed.
        /// For example, if 'T' is a typedef for 'int', the canonical type for 'T' would be 'int'.
        /// </remarks>
        public TypeInfo GetCanonicalType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getCanonicalType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Determines whether the <see cref="TypeInfo"/> has the "const" qualifier set, without
        /// looking through typedefs that may have added "const" at a different level.
        /// </summary>
        /// <returns>true if the <see cref="TypeInfo"/> has the "const" qualifier set.</returns>
        public bool IsConstQualified()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isConstQualifiedType(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the <see cref="TypeInfo"/> has the "volatile" qualifier set, without
        /// looking through typedefs that may have added "volatile" at a different level.
        /// </summary>
        /// <returns>true if the <see cref="TypeInfo"/> has the "volatile" qualifier set.</returns>
        public bool IsVolatileQualified()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isVolatileQualifiedType(Struct) != 0;
        }

        /// <summary>
        /// Gets the typedef name of the given type.
        /// </summary>
        /// <returns>The typedef name of the given type.</returns>
        public string GetTypedefName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getTypedefName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the type of the pointee for pointer types.
        /// </summary>
        /// <returns>The pointee for pointer types.</returns>
        public TypeInfo GetPointeeType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getPointeeType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the cursor for the declaration of the type.
        /// </summary>
        /// <returns>The cursor for the declaration of the type.</returns>
        public Cursor GetTypeDeclaration()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getTypeDeclaration(Struct);
            return Cursor.Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Gets the return type associated with a function type.
        /// </summary>
        /// <returns>
        /// The return type associated with a function type, or an invalid type if the type is a
        /// non-function type.
        /// </returns>
        public TypeInfo GetResultType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getResultType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the exception specification type associated with a function type.
        /// </summary>
        /// <returns>
        /// null if the type is a non-function type.
        /// </returns>
        public ExceptionSpecificationKind? GetExceptionSpecificationType()
        {
            ThrowIfDisposed();

            int kind = NativeMethods.clang_getExceptionSpecificationType(Struct);
            if (kind == -1)
            {
                return null;
            }
            else
            {
                return (ExceptionSpecificationKind?)kind;
            }
        }

        /// <summary>
        /// Gets the number of non-variadic parameters associated with a function type.
        /// </summary>
        /// <returns>
        /// The number of non-variadic parameters associated with a function type, or -1 if the
        /// type is a non-function type.
        /// </returns>
        public int GetNumArgTypes()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getNumArgTypes(Struct);
        }

        /// <summary>
        /// Gets the type of a parameter of a function type.
        /// </summary>
        /// <param name="index">The index of the parameter.</param>
        /// <returns>
        /// The type of a parameter of a function type, or an invalid type if the type is a
        /// non-function type or the function does not have enough parameters.
        /// </returns>
        public TypeInfo GetArgType(uint index)
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getArgType(Struct, index);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Determines whether the <see cref="TypeInfo"/> is a variadic function type.
        /// </summary>
        /// <returns>true if the <see cref="TypeInfo"/> is a variadic function type.</returns>
        public bool IsFunctionTypeVariadic()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isFunctionTypeVariadic(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the <see cref="TypeInfo"/> is a POD (plain old data) type.
        /// </summary>
        /// <returns>true if the <see cref="TypeInfo"/> is a POD (plain old data) type.</returns>
        public bool IsPODType()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isPODType(Struct) != 0;
        }

        /// <summary>
        /// Gets the element type of an array type.
        /// </summary>
        /// <returns>
        /// The element type of an array type, or an invalid type if the type is a non-array type.
        /// </returns>
        public TypeInfo GetArrayElementType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getArrayElementType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the array size of a constant array.
        /// </summary>
        /// <returns>
        /// The array size of a constant array, or -1 if the type is a non-array type.
        /// </returns>
        public long GetArraySize()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getArraySize(Struct);
        }

        /// <summary>
        /// Gets the type named by the qualified-id.
        /// </summary>
        /// <returns>
        /// The type named by the qualified-id, or an invalid type if the type is a  non-elaborated
        /// type.
        /// </returns>
        public TypeInfo GetNamedType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_Type_getNamedType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the alignment of a type in bytes as per C++ [expr.alignof] standard.
        /// </summary>
        /// <returns>
        /// error:
        /// <para>
        /// If the type declaration is invalid, <see cref="TypeLayoutError.Invalid"/> is returned.
        /// </para>
        /// <para>
        /// If the type declaration is an incomplete type, <see cref="TypeLayoutError.Incomplete"/>
        /// is returned.
        /// </para>
        /// <para>
        /// If the type declaration is a dependent type, <see cref="TypeLayoutError.Dependent"/> is
        /// returned.
        /// </para>
        /// <para>
        /// If the type declaration is not a constant size type, 
        /// <see cref="TypeLayoutError.NotConstantSize"/> is returned.
        /// </para>
        /// <para>align: The alignment of a type in bytes.</para>
        /// </returns>
        public (TypeLayoutError? error, long align) TryGetAlignOf()
        {
            ThrowIfDisposed();

            long align = NativeMethods.clang_Type_getAlignOf(Struct);
            var error = align < 0 ? (TypeLayoutError?)align : null;
            return (error: error, align: align);
        }

        /// <summary>
        /// Gets the class type of an member pointer type.
        /// </summary>
        /// <returns>
        /// The class type of an member pointer type, or an invalid type if the type is a
        /// non-member-pointer type.
        /// </returns>
        public TypeInfo GetClassType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_Type_getClassType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the size of a type in bytes as per C++ [expr.sizeof] standard.
        /// </summary>
        /// <returns>
        /// error:
        /// <para>
        /// If the type declaration is invalid, <see cref="TypeLayoutError.Invalid"/> is returned.
        /// </para>
        /// <para>
        /// If the type declaration is an incomplete type, <see cref="TypeLayoutError.Incomplete"/>
        /// is returned.
        /// </para>
        /// <para>
        /// If the type declaration is a dependent type, <see cref="TypeLayoutError.Dependent"/> is
        /// returned.
        /// </para>
        /// <para>size: The alignment of a type in bytes.</para>
        /// </returns>
        public (TypeLayoutError? error, long size) TryGetSizeOf()
        {
            ThrowIfDisposed();

            long size = NativeMethods.clang_Type_getSizeOf(Struct);
            var error = size < 0 ? (TypeLayoutError?)size : null;
            return (error: error, size: size);
        }

        /// <summary>
        /// Gets the offset of a field named <paramref name="fieldName"/> in a record of type T in
        /// bits as it would be returned by __offsetof__ as per C++11 [18.2p4].
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>
        /// error:
        /// <para>
        /// If the cursor is not a record field declaration, <see cref="TypeLayoutError.Invalid"/>
        /// is returned.
        /// </para>
        /// <para>
        /// If the field's type declaration is an incomplete type,
        /// <see cref="TypeLayoutError.Incomplete"/> is returned.
        /// </para>
        /// <para>
        /// If the field's type declaration is a dependent type,
        /// <see cref="TypeLayoutError.Dependent"/> is returned.
        /// </para>
        /// <para>
        /// If the field's name <paramref name="fieldName"/> is not found,
        /// <see cref="TypeLayoutError.InvalidFieldName"/> is returned.
        /// </para>
        /// <para>offset: The offset of the field named <paramref name="fieldName"/>.</para>
        /// </returns>
        public (TypeLayoutError? error, long offset) TryGetOffsetOf(string fieldName)
        {
            Requires.NotNullOrEmpty(fieldName, nameof(fieldName));
            ThrowIfDisposed();

            using (var cString = new CString(fieldName))
            {
                long offset = NativeMethods.clang_Type_getOffsetOf(Struct, cString.Ptr);
                var error = offset < 0 ? (TypeLayoutError?)offset : null;
                return (error: error, offset: offset);
            }
        }

        /// <summary>
        /// Gets the number of template arguments for a class template specialization, or -1 if
        /// type <c>T</c> is not a class template specialization.
        /// </summary>
        /// <returns>
        /// The number of template arguments for the class template specialization, or -1 if type
        /// <c>T</c> is not a class template specialization.
        /// </returns>
        /// <remarks>
        /// Variadic argument packs count as only one argument, and can not be inspected further.
        /// </remarks>
        public int GetNumTemplateArguments()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Type_getNumTemplateArguments(Struct);
        }

        /// <summary>
        /// Gets the type template argument of a template class specialization at given index.
        /// </summary>
        /// <param name="index">The index of the type template argument.</param>
        /// <returns>The template type argument at given index.</returns>
        /// <remarks>
        /// This method only returns template type arguments and does not handle template template
        /// arguments or variadic packs.
        /// </remarks>
        public TypeInfo GetTemplateArgumentAsType(uint index)
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_Type_getTemplateArgumentAsType(Struct, index);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the ref-qualifier kind of a function or method.
        /// </summary>
        /// <returns>
        /// The ref-qualifier kind of a function or method, or <see cref="RefQualifierKind.None"/>
        /// if the type is not a C++ function or method.
        /// </returns>
        public RefQualifierKind GetCXXRefQualifier()
        {
            ThrowIfDisposed();

            return (RefQualifierKind)NativeMethods.clang_Type_getCXXRefQualifier(Struct);
        }
    }
}
