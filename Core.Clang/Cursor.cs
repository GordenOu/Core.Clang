using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Core.Linq;

namespace Core.Clang
{
    /// <summary>
    /// A cursor representing some element in the abstract syntax tree for a translation unit.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The cursor abstraction unifies the different kinds of entities in a program--declaration,
    /// statements, expressions, references to declarations, etc.--under a single "cursor"
    /// abstraction with a common set of operations. Common operation for a cursor include: getting
    /// the physical location in a source file where the cursor points, getting the name associated
    /// with a cursor, and retrieving cursors for any child nodes of a particular cursor.
    /// </para>
    /// <para>
    /// Cursors can be produced in two specific ways. <see cref="TranslationUnit.GetCursor()"/>
    /// produces a cursor for a translation unit, from which one can use
    /// <see cref="CursorVisitor.VisitChildren(Cursor)"/> to explore the rest of the translation
    /// unit. <see cref="TranslationUnit.GetCursor(SourceLocation)"/> maps from a physical source
    /// location to the entity that resides at that location, allowing one to map from the source
    /// code into the AST.
    /// </para>
    /// </remarks>
    public sealed unsafe class Cursor : IEquatable<Cursor>
    {
        internal CXCursor Struct { get; }

        /// <summary>
        /// Gets the translation unit that a cursor originated from.
        /// </summary>
        public TranslationUnit TranslationUnit { get; }

        /// <summary>
        /// Gets the kind of the cursor.
        /// </summary>
        public CursorKind Kind => (CursorKind)Struct.kind;

        private Cursor(CXCursor cxCursor, TranslationUnit translationUnit)
        {
            Struct = cxCursor;
            TranslationUnit = translationUnit;
        }

        internal static Cursor Create(CXCursor cxCursor, TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isNull(cxCursor) != 0
                ? null
                : new Cursor(cxCursor, translationUnit);
        }

        internal void ThrowIfDisposed()
        {
            Debug.Assert(TranslationUnit != null);

            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Determines whether the current instance is equal to another <see cref="Cursor"/>
        /// object.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Cursor"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// true if the current instance and the <paramref name="other"/> parameter represents the
        /// same cursor.
        /// </returns>
        public bool Equals(Cursor other)
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

            return NativeMethods.clang_equalCursors(Struct, other.Struct) != 0;
        }

        /// <summary>
        /// Determines whether the current instance is equal to another object, which must be of
        /// type <see cref="Cursor"/> and represents the same cursor.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>
        /// true if the current instance and the <paramref name="obj"/> parameter represents the
        /// same cursor.
        /// </returns>
        public override bool Equals(object obj)
        {
            ThrowIfDisposed();

            return Equals(obj as Cursor);
        }

        /// <summary>
        /// Computes a hash value for the cursor.
        /// </summary>
        /// <returns>The hash value for the cursor.</returns>
        public override int GetHashCode()
        {
            ThrowIfDisposed();

            uint hash = NativeMethods.clang_hashCursor(Struct);
            return unchecked((int)hash);
        }

        /// <summary>
        /// Determines whether the cursor has any attributes.
        /// </summary>
        /// <returns>true if the cursor has any attributes.</returns>
        public bool HasAttribute()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_hasAttrs(Struct) != 0;
        }

        /// <summary>
        /// Gets the linkage of the entity referred to.
        /// </summary>
        /// <returns>The linkage of the entity referred to.</returns>
        public LinkageKind GetLinkage()
        {
            ThrowIfDisposed();

            return (LinkageKind)NativeMethods.clang_getCursorLinkage(Struct);
        }

        /// <summary>
        /// Gets the "language" of the entity referred to.
        /// </summary>
        /// <returns>The "language" of the entity referred to.</returns>
        public LanguageKind GetLanguage()
        {
            ThrowIfDisposed();

            return (LanguageKind)NativeMethods.clang_getCursorLanguage(Struct);
        }

        /// <summary>
        /// Gets the semantic parent of the cursor.
        /// </summary>
        /// <returns>The semantic parent of the cursor.</returns>
        /// <remarks>
        /// <para>
        /// The semantic parent of a cursor is the cursor that semantically contains the given
        /// cursor. For many declarations, the lexical and semantic parents are equivalent (the
        /// lexical parent is returned by <see cref="GetLexicalParent"/>). They diverge when
        /// declarations or definitions are provided out-of-line.For example:
        /// </para>
        /// <code>
        /// class C
        /// {
        ///     void f();
        /// };
        /// 
        /// void C::f() { }
        /// </code>
        /// <para>
        /// In the out-of-line definition of <c>C::f</c>, the semantic parent is the class <c>C</c>,
        /// of which this function is a member. The lexical parent is the place where the
        /// declaration actually occurs in the source code; in this case, the definition occurs in
        /// the translation unit. In general, the lexical parent for a given entity can change
        /// without affecting the semantics of the program, and the lexical parent of different
        /// declarations of the same entity may be different. Changing the semantic parent of a
        /// declaration, on the other hand, can have a major impact on semantics, and
        /// redeclarations of a particular entity should all have the same semantic context.
        /// </para>
        /// <para>
        /// In the example above, both declarations of <c>C::f</c> have <c>C</c> as their semantic
        /// context, while the lexical context of the first <c>C::f</c> is <c>C</c> and the lexical
        /// context of the second <c>C::f</c> is the translation unit.
        /// </para>
        /// <para>
        /// For global declarations, the semantic parent is the translation unit.
        /// </para>
        /// </remarks>
        public Cursor GetSemanticParent()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCursorSemanticParent(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Gets the lexical parent of the cursor.
        /// </summary>
        /// <returns>The lexical parent of the cursor.</returns>
        /// <remarks>
        /// <para>
        /// The lexical parent of a cursor is the cursor in which the given cursor was actually
        /// written. For many declarations, the lexical and semantic parents are equivalent (the
        /// semantic parent is returned by  <see cref="GetSemanticParent"/>). They diverge when
        /// declarations or definitions are provided out-of-line.For example:
        /// </para>
        /// <code>
        /// class C
        /// {
        ///     void f();
        /// };
        /// 
        /// void C::f() { }
        /// </code>
        /// <para>
        /// In the out-of-line definition of <c>C::f</c>, the semantic parent is the class <c>C</c>,
        /// of which this function is a member. The lexical parent is the place where the
        /// declaration actually occurs in the source code; in this case, the definition occurs in
        /// the translation unit. In general, the lexical parent for a given entity can change
        /// without affecting the semantics of the program, and the lexical parent of different
        /// declarations of the same entity may be different. Changing the semantic parent of a
        /// declaration, on the other hand, can have a major impact on semantics, and
        /// redeclarations of a particular entity should all have the same semantic context.
        /// </para>
        /// <para>
        /// In the example above, both declarations of <c>C::f</c> have <c>C</c> as their semantic
        /// context, while the lexical context of the first <c>C::f</c> is <c>C</c> and the lexical
        /// context of the second <c>C::f</c> is the translation unit.
        /// </para>
        /// <para>
        /// For global declarations, the lexical parent is the translation unit.
        /// </para>
        /// </remarks>
        public Cursor GetLexicalParent()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCursorLexicalParent(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Gets the set of methods that are overridden by the method.
        /// </summary>
        /// <returns>
        /// An array of cursors, representing the set of overridden methods.
        /// </returns>
        /// <remarks>
        /// <para>
        /// In C++, a virtual member function can override a virtual method in a base class. If no
        /// such method exists, the search continues to the class's superclass.
        /// </para>
        /// <para>
        /// For C++, a virtual member function overrides any virtual member function with the same
        /// signature that occurs in its base classes. With multiple inheritance, a virtual member
        /// function can override several virtual member functions coming from different base
        /// classes.
        /// </para>
        /// <para>
        /// In all cases, this method determines the immediate overridden method, rather than all
        /// of the overridden methods. For example, if a method is originally declared in a class
        /// A, then overridden in B (which in inherits from A) and also in C (which inherited from
        /// B), then the only overridden method returned from this function when invoked on C's
        /// method will be B's method. The client may then invoke this function again, given the
        /// previously-found overridden methods, to map out the complete method-override set.
        /// </para>
        /// </remarks>
        public Cursor[] GetOverriddenCursors()
        {
            ThrowIfDisposed();

            CXCursor* overriden = null;
            uint num_overridden;
            NativeMethods.clang_getOverriddenCursors(Struct, &overriden, &num_overridden);
            try
            {
                var cursors = new Cursor[num_overridden];
                for (int i = 0; i < cursors.Length; i++)
                {
                    cursors[i] = Create(overriden[i], TranslationUnit);
                }
                return cursors;
            }
            finally
            {
                if (overriden != null)
                {
                    NativeMethods.clang_disposeOverriddenCursors(overriden);
                }
            }
        }

        /// <summary>
        /// Gets the file that is included by the inclusion directive cursor.
        /// </summary>
        /// <returns>
        /// The file that is included by the inclusion directive cursor, or null if the cursor is
        /// not a inclusion directive cursor.
        /// </returns>
        public SourceFile GetIncludedFile()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_getIncludedFile(Struct);
            return ptr == null ? null : new SourceFile(ptr, TranslationUnit);
        }

        /// <summary>
        /// Gets the physical location of the source constructor referenced by the cursor.
        /// </summary>
        /// <returns>
        /// The physical location of the source constructor referenced by the cursor.
        /// </returns>
        /// <remarks>
        /// The location of a declaration is typically the location of the name of that
        /// declaration, where the name of that declaration would occur if it is unnamed, or some
        /// keyword that introduces that particular declaration. The location of a reference is
        /// where that reference occurs within the source code.
        /// </remarks>
        public SourceLocation GetLocation()
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getCursorLocation(Struct);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets the physical extent of the source construct referenced by the cursor.
        /// </summary>
        /// <returns>
        /// The physical extent of the source construct referenced by the cursor.
        /// </returns>
        /// <remarks>
        /// The extent of a cursor starts with the file/line/column pointing at the first character
        /// within the source construct that the cursor refers to and ends with the last character
        /// within that source construct. For a declaration, the extent covers the declaration
        /// itself. For a reference, the extent covers the location of the reference (e.g., where
        /// the referenced entity was actually used).
        /// </remarks>
        public SourceRange GetExtent()
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_getCursorExtent(Struct);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }

        /// <summary>
        /// Gets the type of a <see cref="Cursor"/> (if any).
        /// </summary>
        /// <returns>The type of a <see cref="Cursor"/> (if any).</returns>
        public TypeInfo GetTypeInfo()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getCursorType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the underlying type of a typedef declaration.
        /// </summary>
        /// <returns>
        /// The underlying type of a typedef declaration, or an invalid type if the cursor does not
        /// reference a typedef declaration.
        /// </returns>
        public TypeInfo GetTypedefDeclUnderlyingType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getTypedefDeclUnderlyingType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the integer type of an enum declaration.
        /// </summary>
        /// <returns>
        /// The integer type of an enum declaration, or an invalid type if the cursor does not
        /// reference a enum declaration.
        /// </returns>
        public TypeInfo GetEnumDeclIntegerType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getEnumDeclIntegerType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the integer value of an enum constant declaration as a signed long.
        /// </summary>
        /// <returns>
        /// The integer value of an enum constant declaration as a signed long, or
        /// <see cref="long.MinValue"/> if the cursor does not reference an enum constant
        /// declaration. Since this is also potentially a valid constant value, the kind of the
        /// cursor must be verified before calling this function.
        /// </returns>
        public long GetEnumConstantDeclValue()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getEnumConstantDeclValue(Struct);
        }

        /// <summary>
        /// Gets the integer value of an enum constant declaration as an unsigned long.
        /// </summary>
        /// <returns>
        /// The integer value of an enum constant declaration as an unsigned long, or
        /// <see cref="ulong.MaxValue"/> if the cursor does not reference an enum constant
        /// declaration. Since this is also potentially a valid constant value, the kind of the
        /// cursor must be verified before calling this function.
        /// </returns>
        public ulong GetEnumConstantDeclUnsignedValue()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getEnumConstantDeclUnsignedValue(Struct);
        }

        /// <summary>
        /// Gets the bit width of a bit field declaration as an integer.
        /// </summary>
        /// <returns>
        /// The bit width of a bit field declaration as an integer, or -1 if the cursor is not a
        /// bit field declaration.
        /// </returns>
        public int GetFieldDeclBitWidth()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getFieldDeclBitWidth(Struct);
        }

        /// <summary>
        /// Gets the number of non-variadic arguments associated with the cursor.
        /// </summary>
        /// <returns>
        /// The number of arguments can be determined for calls as well as for declarations of
        /// functions or methods. For other cursors -1 is returned.
        /// </returns>
        public int GetNumArguments()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_getNumArguments(Struct);
        }

        /// <summary>
        /// Gets the argument cursor of a function or method.
        /// </summary>
        /// <param name="index">The index of the argument.</param>
        /// <returns>
        /// The argument cursor determined for calls as well as for declarations of functions or
        /// methods. For other cursors and for invalid indices, an invalid cursor is returned.
        /// </returns>
        public Cursor GetArgument(uint index)
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_Cursor_getArgument(Struct, index);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Gets the number of template args of a function declaration representing a template
        /// specialization.
        /// </summary>
        /// <returns>
        /// The number of template args of a function declaration representing a template
        /// specialization, or -1 if the cursor cannot be converted into a template function
        /// declaration.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For example, for the following declaration and specialization:
        /// </para>
        /// <code>
        /// template&lt;typename T, int kInt, bool kBool&gt;
        /// void foo() { ... }
        /// 
        /// template&lt;&gt;
        /// void foo&lt;float, -7, true&gt;();
        /// </code>
        /// <para>
        /// The value 3 would be returned from this call.
        /// </para>
        /// </remarks>
        public int GetNumTemplateArguments()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_getNumTemplateArguments(Struct);
        }

        /// <summary>
        /// Gets the kind of the <paramref name="index"/>'th template argument of the
        /// <see cref="Cursor"/>.
        /// </summary>
        /// <param name="index">The index of the template argument.</param>
        /// <returns>
        /// The kind of the <paramref name="index"/>'th template argument of the
        /// <see cref="Cursor"/>, or an invalid template argument type if the cursor does not
        /// represent a function declaration.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For example, for the following declaration and specialization:
        /// </para>
        /// <code>
        /// template&lt;typename T, int kInt, bool kBool&gt;
        /// void foo() { ... }
        /// 
        /// template&lt;&gt;
        /// void foo&lt;float, -7, true&gt;();
        /// </code>
        /// <para>
        /// For <paramref name="index"/> = 0, 1, and 2, Type, Integral, and Integral will be
        /// returned, respectively.
        /// </para>
        /// </remarks>
        public TemplateArgumentKind GetTemplateArgumentKind(uint index)
        {
            ThrowIfDisposed();

            var kind = NativeMethods.clang_Cursor_getTemplateArgumentKind(Struct, index);
            return (TemplateArgumentKind)kind;
        }

        /// <summary>
        /// Gets a <see cref="TypeInfo"/> representing the type of a TemplateArgument of a function
        /// declaration representing a template specialization.
        /// </summary>
        /// <param name="index">The index of the template argument.</param>
        /// <returns>
        /// A <see cref="TypeInfo"/> representing the type of a TemplateArgument of a function
        /// declaration representing a template specialization, or an invalid type if the cursor
        /// does not represent a function declaration, or its <paramref name="index"/>'th template
        /// argument has a kind of <see cref="TemplateArgumentKind.Integral"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For example, for the following declaration and specialization:
        /// </para>
        /// <code>
        /// template&lt;typename T, int kInt, bool kBool&gt;
        /// void foo() { ... }
        /// 
        /// template&lt;&gt;
        /// void foo&lt;float, -7, true&gt;();
        /// </code>
        /// <para>
        /// If called with <paramref name="index"/> = 0, "float", will be returned. Invalid types
        /// will be returned for <paramref name="index"/> == 1 or 2.
        /// </para>
        /// </remarks>
        public TypeInfo GetTemplateArgumentType(uint index)
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_Cursor_getTemplateArgumentType(Struct, index);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the value of an Integral TemplateArgument (of a function declaration representing
        /// a template specialization) as a signed long.
        /// </summary>
        /// <param name="index">The index of the template argument.</param>
        /// <returns>
        /// The value of an Integral TemplateArgument (of a function declaration representing
        /// a template specialization) as a signed long. It is undefined to call this function on a
        /// <see cref="Cursor"/> that does not represent a function declaration or whose 
        /// <paramref name="index"/>'th template argument is not an integral value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For example, for the following declaration and specialization:
        /// </para>
        /// <code>
        /// template&lt;typename T, int kInt, bool kBool&gt;
        /// void foo() { ... }
        /// 
        /// template&lt;&gt;
        /// void foo&lt;float, -7, true&gt;();
        /// </code>
        /// <para>
        /// If called with <paramref name="index"/> = 1 or 2, -7 or true will be returned,
        /// respectively. For <paramref name="index"/> == 0, this function's behavior is undefined.
        /// </para>
        /// </remarks>
        public long GetTemplateArgumentValue(uint index)
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_getTemplateArgumentValue(Struct, index);
        }

        /// <summary>
        /// Gets the value of an Integral TemplateArgument (of a function declaration representing
        /// a template specialization) as a unsigned long.
        /// </summary>
        /// <param name="index">The index of the template argument.</param>
        /// <returns>
        /// The value of an Integral TemplateArgument (of a function declaration representing
        /// a template specialization) as a signed long. It is undefined to call this function on a
        /// <see cref="Cursor"/> that does not represent a function declaration or whose 
        /// <paramref name="index"/>'th template argument is not an integral value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For example, for the following declaration and specialization:
        /// </para>
        /// <code>
        /// template&lt;typename T, int kInt, bool kBool&gt;
        /// void foo() { ... }
        /// 
        /// template&lt;&gt;
        /// void foo&lt;float, 2147483649, true&gt;();
        /// </code>
        /// <para>
        /// If called with <paramref name="index"/> = 1 or 2, 2147483649 or true will be returned,
        /// respectively. For <paramref name="index"/> == 0, this function's behavior is undefined.
        /// </para>
        /// </remarks>
        public ulong GetTemplateArgumentUnsignedValue(uint index)
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_getTemplateArgumentUnsignedValue(Struct, index);
        }

        /// <summary>
        /// Determines whether the cursor is a function like macro.
        /// </summary>
        /// <returns></returns>
        public bool IsFunctionLikeMacro()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isMacroFunctionLike(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the cursor is a builtin macro.
        /// </summary>
        /// <returns></returns>
        public bool IsBuiltinMacro()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isMacroBuiltin(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the cursor is an inline function declaration.
        /// </summary>
        /// <returns></returns>
        public bool IsInlinedFunction()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isFunctionInlined(Struct) != 0;
        }

        /// <summary>
        /// Gets the return type associated with the cursor.
        /// </summary>
        /// <returns>
        /// The return type associated with the cursor, or a invalid type if the cursor does not
        /// refer to a function or method.
        /// </returns>
        public TypeInfo GetResultType()
        {
            ThrowIfDisposed();

            var cxType = NativeMethods.clang_getCursorResultType(Struct);
            return new TypeInfo(cxType, TranslationUnit);
        }

        /// <summary>
        /// Gets the exception specification type associated with the cursor.
        /// </summary>
        /// <returns>
        /// A valid result if the cursor refers to a function or method.
        /// </returns>
        public ExceptionSpecificationKind GetExceptionSpecificationType()
        {
            ThrowIfDisposed();

            int result = NativeMethods.clang_getCursorExceptionSpecificationType(Struct);
            return (ExceptionSpecificationKind)result;
        }

        /// <summary>
        /// Gets the offset of the field represented by the Cursor.
        /// </summary>
        /// <returns>
        /// error:
        /// <para>
        /// If the cursor is not a field declaration, <see cref="TypeLayoutError.Invalid"/> is
        /// returned.
        /// </para>
        /// <para>
        /// If the cursor's semantic parent is not a record field declaration,
        /// <see cref="TypeLayoutError.Invalid"/> is returned.
        /// </para>
        /// <para>
        /// If the field's type declaration is an incomplete type,
        /// <see cref="TypeLayoutError.Incomplete"/> is returned.
        /// </para>
        /// <para>
        /// If the field's type declaration is a dependent type,
        /// <see cref="TypeLayoutError.Dependent"/> is returned.
        /// </para>
        /// <para>offset: The offset of the field.</para>
        /// </returns>
        public (TypeLayoutError? error, long offset) TryGetOffsetOfField()
        {
            ThrowIfDisposed();

            long offset = NativeMethods.clang_Cursor_getOffsetOfField(Struct);
            var error = offset < 0 ? (TypeLayoutError?)offset : null;
            return (error: error, offset: offset);
        }

        /// <summary>
        /// Determines whether the cursor represents an anonymous record declaration.
        /// </summary>
        /// <returns>true if the cursor represents an anonymous record declaration.</returns>
        public bool IsAnonymous()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isAnonymous(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the cursor specifies a Record member that is a bitfield.
        /// </summary>
        /// <returns>true if the cursor specifies a Record member that is a bitfield.</returns>
        public bool IsBitField()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isBitField(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the base class specified by the cursor with kind
        /// <see cref="CursorKind.CXXBaseSpecifier"/> is virtual.
        /// </summary>
        /// <returns>
        /// true if the base class specified by the cursor with kind
        /// <see cref="CursorKind.CXXBaseSpecifier"/> is virtual.
        /// </returns>
        public bool IsVirtualBase()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isVirtualBase(Struct) != 0;
        }

        /// <summary>
        /// Gets the access control level for the referenced object.
        /// </summary>
        /// <returns>
        /// If the cursor refers to a C++ declaration, its access control level within its parent
        /// scope is returned. Otherwise, if the cursor refers to a base specifier or access
        /// specifier, the specifier itself is returned.
        /// </returns>
        public CXXAccessSpecifier GetCXXAccessSpecifier()
        {
            ThrowIfDisposed();

            return (CXXAccessSpecifier)NativeMethods.clang_getCXXAccessSpecifier(Struct);
        }

        /// <summary>
        /// Gets the storage class for a function or variable declaration.
        /// </summary>
        /// <returns>
        /// The storage class for a function or variable declaration, or
        /// <see cref="StorageClass.Invalid"/> if the cursor is not a function or variable
        /// declaration.
        /// </returns>
        public StorageClass GetStorageClass()
        {
            ThrowIfDisposed();

            return (StorageClass)NativeMethods.clang_Cursor_getStorageClass(Struct);
        }

        /// <summary>
        /// Gets the number of overloaded declarations referenced by a
        /// <see cref="CursorKind.OverloadedDeclRef"/> cursor.
        /// </summary>
        /// <returns>
        /// The number of overloaded declarations referenced by the cursor. or 0 if it is not a
        /// <see cref="CursorKind.OverloadedDeclRef"/> cursor.
        /// </returns>
        public uint GetNumOverloadedDecls()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getNumOverloadedDecls(Struct);
        }

        /// <summary>
        /// Gets a cursor for one of the overloaded declarations referenced by a
        /// <see cref="CursorKind.OverloadedDeclRef"/> cursor.
        /// </summary>
        /// <param name="index">
        /// The zero-based index into the set of overloaded declarations in the cursor.
        /// </param>
        /// <returns>
        /// A cursor representing the declaration referenced by the cursor at the specified
        /// <paramref name="index"/>. If the cursor does not have an associated set of overloaded
        /// declarations, or if the index is out of bounds, returns null.
        /// </returns>
        public Cursor GetOverloadedDecl(uint index)
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getOverloadedDecl(Struct, index);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Gets a Unified Symbol Resolution (USR) for the entity referenced by the cursor.
        /// </summary>
        /// <returns>
        /// A Unified Symbol Resolution (USR) for the entity referenced by the cursor.
        /// </returns>
        /// <remarks>
        /// A Unified Symbol Resolution (USR) is a string that identifies a particular entity
        /// (function, class, variable, etc.) within a program. USRs can be compared across
        /// translation units to determine, e.g., when references in one translation refer to an
        /// entity defined in another translation unit.
        /// </remarks>
        public string GetUSR()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getCursorUSR(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets a name for the entity referenced by this cursor.
        /// </summary>
        /// <returns>A name for the entity referenced by this cursor.</returns>
        public string GetSpelling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getCursorSpelling(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets a range for a piece that forms the cursors spelling name.
        /// </summary>
        /// <returns>A range for a piece that forms the cursors spelling name.</returns>
        public SourceRange GetSpellingNameRange()
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_Cursor_getSpellingNameRange(Struct, 0, 0);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }

        /// <summary>
        /// Gets the display name for the entity referenced by this cursor.
        /// </summary>
        /// <returns>The display name for the entity referenced by this cursor.</returns>
        /// <remarks>
        /// The display name contains extra information that helps identify the cursor, such as the
        /// parameters of a function or template or the arguments of a class template
        /// specialization.
        /// </remarks>
        public string GetDisplayName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getCursorDisplayName(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// For a cursor that is a reference, gets a cursor representing the entity that it
        /// references.
        /// </summary>
        /// <returns>A cursor representing the entity that it references.</returns>
        /// <remarks>
        /// Reference cursors refer to other entities in the AST. If the input cursor is a
        /// declaration or definition, it returns that declaration or definition unchanged.
        /// Otherwise, returns null.
        /// </remarks>
        public Cursor GetCursorReferenced()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCursorReferenced(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// For a cursor that is either a reference to or a declaration of some entity, gets a
        /// cursor that describes the definition of that entity.
        /// </summary>
        /// <returns>A cursor that describes the definition of the entity.</returns>
        /// <remarks>
        /// <para>
        /// Some entities can be declared multiple times within a translation unit, but only one of
        /// those declarations can also be a definition. For example, given:
        /// </para>
        /// <code>
        /// int f(int, int);
        /// int g(int x, int y) { return f(x, y); }
        /// int f(int a, int b) { return a + b; }
        /// int f(int, int);
        /// </code>
        /// <para>
        /// There are three declarations of the function "f", but only the second one is a
        /// definition.The <see cref="GetDefinition"/> method will take any cursor pointing to a
        /// declaration of "f" (the first or fourth lines of the example) or a cursor referenced
        /// that uses "f" (the call to "f' inside "g") and will return a declaration cursor
        /// pointing to the definition(the second "f" declaration).
        /// </para>
        /// <para>
        /// If given a cursor for which there is no corresponding definition, e.g., because there
        /// is no definition of that entity within this translation unit, returns null.
        /// </para>
        /// </remarks>
        public Cursor GetDefinition()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCursorDefinition(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Determines whether the declaration pointed to by this cursor is also a definition of
        /// that entity.
        /// </summary>
        /// <returns>
        /// true if the declaration pointed to by this cursor is also a definition of that entity.
        /// </returns>
        public bool IsDefinition()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isCursorDefinition(Struct) != 0;
        }

        /// <summary>
        /// Gets the canonical cursor corresponding to the cursor.
        /// </summary>
        /// <returns>The canonical cursor for the entity referred to by the cursor.</returns>
        /// <remarks>
        /// <para>
        /// In the C family of languages, many kinds of entities can be declared several times
        /// within a single translation unit. For example, a structure type can be forward-declared
        /// (possibly multiple times) and later defined:
        /// </para>
        /// <code>
        /// struct X;
        /// struct X;
        /// struct X
        /// {
        ///     int member;
        /// };
        /// </code>
        /// <para>
        /// The declarations and the definition of <c>X</c> are represented by three different
        /// cursors, all of which are declarations of the same underlying entity. One of these
        /// cursor is considered the "canonical" cursor, which is effectively the representative
        /// for the underlying entity. One can determine if two cursors are declarations of the
        /// same underlying entity by comparing their canonical cursors.
        /// </para>
        /// </remarks>
        public Cursor GetCanonicalCursor()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getCanonicalCursor(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        /// <summary>
        /// Given a cursor pointing to a C++ method call, returns true if the method is "dynamic",
        /// meaning: the call is virtual.
        /// </summary>
        /// <returns>
        /// true if the method is "dynamic", or false if the method is "static" or the cursor does
        /// not point to a method.
        /// </returns>
        public bool IsDynamicCall()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isDynamicCall(Struct) != 0;
        }

        /// <summary>
        /// Determines whether the cursor is a variadic function or method.
        /// </summary>
        /// <returns>true if the cursor is a variadic function or method.</returns>
        public bool IsVariadic()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Cursor_isVariadic(Struct) != 0;
        }

        /// <summary>
        /// Given a cursor that represents a declaration, returns the associated comment's source
        /// range. The range may include multiple consecutive comments with whitespace in between.
        /// </summary>
        /// <returns>The cursor's associated comment's source range.</returns>
        public SourceRange GetCommentRange()
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_Cursor_getCommentRange(Struct);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }

        /// <summary>
        /// For a cursor that represents a declaration, returns the associated comment text,
        /// including comment markers.
        /// </summary>
        /// <returns>
        /// The associated comment text if the cursor represents a declaration.
        /// </returns>
        public string GetRawCommentText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_Cursor_getRawCommentText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// For a cursor that represents a documentable entity (e.g., declaration), returns the
        /// first paragraph of the associated comment text.
        /// </summary>
        /// <returns>
        /// The first paragraph of the comment associated comment text if the cursor represents a
        /// documentable entity.
        /// </returns>
        public string GetBriefCommentText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_Cursor_getBriefCommentText(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the string representing the mangled name of the cursor.
        /// </summary>
        /// <returns>The string representing the mangled name of the cursor.</returns>
        public string GetMangling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_Cursor_getMangling(Struct);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the strings representing the mangled symbols of the C++ constructor or destructor
        /// at the cursor.
        /// </summary>
        /// <returns>
        /// The strings representing the mangled symbols of the C++ constructor or destructor at
        /// the cursor.
        /// </returns>
        [Unstable("5.0.0", seealso: new[]
        {
            "https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CXString.cpp"
        })]
        public string[] GetCXXManglings()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_Cursor_getCXXManglings(Struct);
            if (ptr == null)
            {
                return Array.Empty<string>();
            }
            else
            {
                try
                {
                    var manglings = new string[ptr->Count];
                    manglings.SetValues(i =>
                    {
                        var cString = NativeMethods.clang_getCString(ptr->Strings[i]);
                        return Marshal.PtrToStringAnsi(new IntPtr(cString));
                    });
                    return manglings;
                }
                finally
                {
                    NativeMethods.clang_disposeStringSet(ptr);
                }
            }
        }

        /// <summary>
        /// Gets the module associated with a <see cref="CursorKind.ModuleImportDecl"/> cursor.
        /// </summary>
        /// <returns>
        /// The module associated with a <see cref="CursorKind.ModuleImportDecl"/> cursor.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Module GetModule()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_Cursor_getModule(Struct);
            return ptr == null ? null : new Module(ptr, TranslationUnit);
        }

        /// <summary>
        /// Determines if a if a C++ constructor is a converting constructor.
        /// </summary>
        /// <returns>true if the cursor is a converting C++ constructor.</returns>
        public bool IsConvertingConstructor()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXConstructor_isConvertingConstructor(Struct) != 0;
        }

        /// <summary>
        /// Determines if a if a C++ constructor is a copy constructor.
        /// </summary>
        /// <returns>true if the cursor is a copy C++ constructor.</returns>
        public bool IsCopyConstructor()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXConstructor_isCopyConstructor(Struct) != 0;
        }

        /// <summary>
        /// Determines if a if a C++ constructor is the default constructor.
        /// </summary>
        /// <returns>true if the cursor is the default C++ constructor.</returns>
        public bool IsDefaultConstructor()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXConstructor_isDefaultConstructor(Struct) != 0;
        }

        /// <summary>
        /// Determines if a if a C++ constructor is a move constructor.
        /// </summary>
        /// <returns>true if the cursor is a move C++ constructor.</returns>
        public bool IsMoveConstructor()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXConstructor_isMoveConstructor(Struct) != 0;
        }

        /// <summary>
        /// Determines if a C++ field is declared 'mutable'.
        /// </summary>
        /// <returns>true if the cursor is a mutable C++ field.</returns>
        public bool IsMutableField()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXField_isMutable(Struct) != 0;
        }

        /// <summary>
        /// Determines if a if a C++ method is declared '= default'.
        /// </summary>
        /// <returns>true if the cursor is a C++ method is declared '= default'.</returns>
        public bool IsDefaultedMethod()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXMethod_isDefaulted(Struct) != 0;
        }

        /// <summary>
        /// Determines if a C++ member function or member function template is pure virtual.
        /// </summary>
        /// <returns>
        /// true if the cursor is a pure virtual C++ member function or member function template.
        /// </returns>
        public bool IsPureVirtualMethod()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXMethod_isPureVirtual(Struct) != 0;
        }

        /// <summary>
        /// Determines if a C++ member function or member function template is declared 'static'.
        /// </summary>
        /// <returns>
        /// true if the cursor is a static C++ member function or member function template.
        /// </returns>
        public bool IsStaticMethod()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXMethod_isStatic(Struct) != 0;
        }

        /// <summary>
        /// Determines if a C++ member function or member function template is explicitly declared
        /// "virtual" or if it overrides a virtual method from one of the base classes.
        /// </summary>
        /// <returns>
        /// true if the cursor is a virtual C++ member function or member function template.
        /// </returns>
        public bool IsVirtualMethod()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXMethod_isVirtual(Struct) != 0;
        }

        /// <summary>
        /// Determines if an enum declaration refers to a scoped enum.
        /// </summary>
        /// <returns>true if the enum declaration refers to a scoped enum.</returns>
        public bool IsScopedEnumDecl()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_EnumDecl_isScoped(Struct) != 0;
        }

        /// <summary>
        /// Determines if a C++ member function or member function template is declared 'const'.
        /// </summary>
        /// <returns>
        /// true if the cursor is a constant C++ member function or member function template.
        /// </returns>
        public bool IsConstMethod()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_CXXMethod_isConst(Struct) != 0;
        }

        /// <summary>
        /// For a cursor that represents a template, determines the cursor kind of the
        /// specializations would be generated by instantiating the template.
        /// </summary>
        /// <returns>
        /// The cursor kind of the specialization that would be generated by instantiating the
        /// template, or <see cref="CursorKind.NoDeclFound"/> if the cursor is not a template.
        /// </returns>
        /// <remarks>
        /// This routine can be used to determine what flavor of function template, class template,
        /// or class template partial specialization is stored in the cursor. For example, it can
        /// describe whether a class template cursor is declared with "struct", "class" or "union".
        /// </remarks>
        public CursorKind GetTemplateCursorKind()
        {
            ThrowIfDisposed();

            return (CursorKind)NativeMethods.clang_getTemplateCursorKind(Struct);
        }

        /// <summary>
        /// For a cursor that may represent a specialization or instantiation of a template,
        /// retrieve the cursor that represents the template that it specializes or from which it
        /// was instantiated.
        /// </summary>
        /// <returns>
        /// The template or member that the cursor specializes or from which it was instantiated if
        /// the cursor is a specialization or instantiation of a template or a member thereof.
        /// Otherwise, returns null.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This routine determines the template involved both for explicit specializations of
        /// templates and for implicit instantiations of the template, both of which are referred
        /// to as "specializations". For a class template specialization 
        /// (e.g., <c>std::vector&lt;bool&gt;</c>), this routine will return either the primary
        /// template (<c>std::vector</c>) or, if the specialization was instantiated from a class
        /// template partial specialization, the class template partial specialization. For a class
        /// template partial specialization and a function template specialization (including
        /// instantiations), this routine will return the specialized template.
        /// </para>
        /// <para>
        /// For members of a class template (e.g., member functions, member classes, or static data
        /// members), returns the specialized or instantiated member. Although not strictly
        /// "templates" in the C++ language, members of class templates have the same notions of
        /// specializations and instantiations that templates do, so this routine treats them
        /// similarly.
        /// </para>
        /// </remarks>
        public Cursor GetSpecializedTemplate()
        {
            ThrowIfDisposed();

            var cxCursor = NativeMethods.clang_getSpecializedCursorTemplate(Struct);
            return Create(cxCursor, TranslationUnit);
        }

        public Comment GetParsedComment()
        {
            ThrowIfDisposed();

            var cxComment = NativeMethods.clang_Cursor_getParsedComment(Struct);
            return Comment.Create(cxComment);
        }

        /// <summary>
        /// For a cursor that references something else, return the source range covering that
        /// reference.
        /// </summary>
        /// <param name="flags">
        /// A bitset with three independent flags: <see cref="NameReferenceFlags.WantQualifier"/>,
        /// <see cref="NameReferenceFlags.WantTemplateArgs"/> and
        /// <see cref="NameReferenceFlags.WantSinglePiece"/>.
        /// </param>
        /// <param name="pieceIndex">
        /// For contiguous names or when passing the flag
        /// <see cref="NameReferenceFlags.WantSinglePiece"/>, only one piece with index 0 is
        /// available. When the <see cref="NameReferenceFlags.WantSinglePiece"/> flag is not passed
        /// for a non-contiguous names, this index can be used to retrieve the individual pieces of
        /// the name. See also <see cref="NameReferenceFlags.WantSinglePiece"/>.
        /// </param>
        /// <returns>
        /// The piece of the name pointed to by the cursor. If there is no name, or if the
        /// <paramref name="pieceIndex"/> is out-of-range, null will be returned.
        /// </returns>
        public SourceRange GetReferenceNameRange(NameReferenceFlags flags, uint pieceIndex)
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_getCursorReferenceNameRange(
                Struct,
                (uint)flags,
                pieceIndex);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }

        /// <summary>
        /// If the cursor is a statement declaration tries to evaluate the statement, if it's a
        /// variable, tries to evaluate its initializer, into its corresponding type.
        /// </summary>
        /// <returns>
        /// <para>
        /// resultKind: The kind of the evaluation result.
        /// </para>
        /// <para>
        /// result: The value of the evaluation result, or its string representation.
        /// </para>
        /// </returns>
        public (EvaluationResultKind resultKind, object result) Evaluate()
        {
            var resultKind = EvaluationResultKind.Unexposed;
            object result = null;

            var ptr = NativeMethods.clang_Cursor_Evaluate(Struct);
            if (ptr != null)
            {
                try
                {
                    var kind = NativeMethods.clang_EvalResult_getKind(ptr);
                    resultKind = (EvaluationResultKind)kind;
                    switch (kind)
                    {
                        case CXEvalResultKind.CXEval_Int:
                            if (NativeMethods.clang_EvalResult_isUnsignedInt(ptr) != 0)
                            {
                                result = NativeMethods.clang_EvalResult_getAsUnsigned(ptr);
                            }
                            else
                            {
                                result = NativeMethods.clang_EvalResult_getAsLongLong(ptr);
                            }
                            break;
                        case CXEvalResultKind.CXEval_Float:
                            result = NativeMethods.clang_EvalResult_getAsDouble(ptr);
                            break;
                        default:
                            var cString = NativeMethods.clang_EvalResult_getAsStr(ptr);
                            if (cString != null)
                            {
                                result = Marshal.PtrToStringAnsi(new IntPtr(cString));
                            }
                            break;
                    }
                }
                finally
                {
                    NativeMethods.clang_EvalResult_dispose(ptr);
                }
            }

            return (resultKind: resultKind, result: result);
        }
    }
}
