using System;
using System.Linq;
using System.Text;
using System.Threading;
using Core.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class CursorTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public CursorTests()
        {
            Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            Monitor.Enter(TestFiles.Locker);
            disposables = new Disposables();
        }

        [TestCleanup]
        public void Dispose()
        {
            disposables.Dispose();
            Monitor.Exit(TestFiles.Locker);
        }

        [TestMethod]
        public void TranslationUnitGetCursorNotNull()
        {
            Assert.IsNotNull(disposables.Add.GetCursor());
            Assert.IsNotNull(disposables.Multiply.GetCursor());
            Assert.AreEqual(CursorKind.TranslationUnit, disposables.Add.GetCursor().Kind);
            Assert.IsTrue(disposables.Add.GetCursor().Kind.IsTranslationUnit());
        }

        [TestMethod]
        public void GetCursorKindsInTestFiles()
        {
            var add = disposables.Add;

            var addHeader = add.GetFile(TestFiles.AddHeader);
            Assert.AreEqual(
                CursorKind.FunctionDecl,
                add.GetCursor(addHeader.GetLocation(3, 1)).Kind);
            Assert.AreEqual(
                CursorKind.ParmDecl,
                add.GetCursor(addHeader.GetLocation(3, 9)).Kind);

            var addSource = add.GetFile(TestFiles.AddSource);
            Assert.AreEqual(
                CursorKind.InclusionDirective,
                add.GetCursor(addSource.GetLocation(1, 10)).Kind);
            Assert.AreEqual(
                CursorKind.FunctionDecl,
                add.GetCursor(addSource.GetLocation(3, 1)).Kind);
            Assert.AreEqual(
                CursorKind.CompoundStmt,
                add.GetCursor(addSource.GetLocation(4, 1)).Kind);
            Assert.AreEqual(
                CursorKind.ReturnStmt,
                add.GetCursor(addSource.GetLocation(5, 5)).Kind);
        }

        [TestMethod]
        public void CursorEquatable()
        {
            var add = disposables.Add;
            var multiply = disposables.Multiply;

            Assert.AreEqual(add.GetCursor(), add.GetCursor());
            Assert.IsTrue(add.GetCursor().Equals(add.GetCursor()));
            Assert.AreNotEqual(add.GetCursor(), multiply.GetCursor());
            Assert.IsFalse(add.GetCursor().Equals(multiply.GetCursor()));
            Assert.AreEqual(add.GetCursor().GetHashCode(), add.GetCursor().GetHashCode());
            Assert.AreNotEqual(add.GetCursor().GetHashCode(), multiply.GetCursor().GetHashCode());
        }

        [TestMethod]
        public void LinkageKinds()
        {
            string source = "void a() { int b; } static int d; namespace { int c; } ";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LinkageKind.External,
                    empty.GetCursor(file.GetLocation(1, 6)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.NoLinkage,
                    empty.GetCursor(file.GetLocation(1, 16)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.Internal,
                    empty.GetCursor(file.GetLocation(1, 32)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.UniqueExternal,
                    empty.GetCursor(file.GetLocation(1, 51)).GetLinkage());
            }
        }

        [TestMethod]
        public void CAndCPlusPlusLanguageKindsAreInferred()
        {
            string source = "int a = 0;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LanguageKind.C,
                    empty.GetCursor(file.GetLocation(1, 1)).GetLanguage());
            }

            source = "class a { };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LanguageKind.CPlusPlus,
                    empty.GetCursor(file.GetLocation(1, 1)).GetLanguage());
            }
        }

        [TestMethod]
        public void SemanticAndLexicalParentAreNotEqual()
        {
            string source = "class C { void f(); }; void C::f() { }";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 11));
                var definition = empty.GetCursor(file.GetLocation(1, 24));
                var classC = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(classC, declaration.GetSemanticParent());
                Assert.AreEqual(classC, declaration.GetLexicalParent());
                Assert.AreEqual(classC, definition.GetSemanticParent());
                Assert.AreEqual(empty.GetCursor(), definition.GetLexicalParent());
                Assert.AreNotEqual(classC, empty.GetCursor());
            }
        }

        [TestMethod]
        public void OverridenCursorsAreBaseMethods()
        {
            string classA = "class A { public: virtual void f(); };";
            string classB = "class B : virtual public A { public: virtual void f(); };";
            string classC = "class C : virtual public A { public: virtual void f(); };";
            string classD = "class D : public B, public C { public: virtual void f(); };";
            string source = classA + classB + classC + classD;
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var aMethod = file.GetLocationFromOffset((uint)source.IndexOf("virtual"));
                var bMethod = file.GetLocationFromOffset(
                    (uint)source.IndexOf("virtual void", classA.Length));
                var cMethod = file.GetLocationFromOffset(
                    (uint)source.IndexOf("virtual void", (classA + classB).Length));
                var dMethod = file.GetLocationFromOffset((uint)(source.LastIndexOf("virtual")));

                var overridenCursors = empty.GetCursor(dMethod).GetOverriddenCursors();
                Assert.AreEqual(2, overridenCursors.Length);
                CollectionAssert.AreEquivalent(
                    new[] { bMethod, cMethod }.ToArray(empty.GetCursor),
                    overridenCursors);

                var baseMethods = overridenCursors[0].GetOverriddenCursors();
                Assert.AreEqual(1, baseMethods.Length);
                Assert.AreEqual(empty.GetCursor(aMethod), baseMethods[0]);

                baseMethods = overridenCursors[1].GetOverriddenCursors();
                Assert.AreEqual(1, baseMethods.Length);
                Assert.AreEqual(empty.GetCursor(aMethod), baseMethods[0]);
            }
        }

        [TestMethod]
        public void IncludedFileIsCommonHeader()
        {
            string source = "#include \"common.h\"";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var inclusion = empty.GetCursor(file.GetLocation(1, 10));
                var includedFile = inclusion.GetIncludedFile();
                Assert.IsNotNull(includedFile);
                Assert.AreEqual(empty.GetFile(TestFiles.CommonHeader), includedFile);
            }

            source = "int a = 0;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var cursor = empty.GetCursor(file.GetLocation(1, 1));
                var includedFile = cursor.GetIncludedFile();
                Assert.IsNull(includedFile);
            }
        }

        [TestMethod]
        public void LocationPointsToDeclaredName()
        {
            string source = "int a = 0;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = empty.GetCursor(file.GetLocation(1, 2)).GetLocation();
                Assert.AreEqual(file.GetLocation(1, 5), location);
            }
        }

        [TestMethod]
        public void CursorExtentCoversTheWholeDeclaration()
        {
            string source = "int a = 0;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var range = empty.GetCursor(file.GetLocation(1, 2)).GetExtent();
                Assert.AreEqual(file.GetLocation(1, 1), range.GetStart());
                Assert.AreEqual(file.GetLocation(1, 10), range.GetEnd());
            }
        }

        [DataTestMethod]
        [DataRow("typedef int Int32;", CursorKind.TypedefDecl)]
        [DataRow("using Int32 = int;", CursorKind.TypeAliasDecl)]
        public void GetTypedefDeclUnderlyingType(string source, CursorKind cursorKind)
        {
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var typedef = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(cursorKind, typedef.Kind);
                Assert.AreEqual(TypeKind.Typedef, typedef.GetTypeInfo().Kind);
                Assert.AreEqual(TypeKind.Int, typedef.GetTypedefDeclUnderlyingType().Kind);
            }
        }

        [TestMethod]
        public void GetEnumIntegerTypeAndValue()
        {
            string source = "enum A : unsigned long long { Value = 1 };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(CursorKind.EnumDecl, declaration.Kind);
                Assert.AreEqual(TypeKind.ULongLong, declaration.GetEnumDeclIntegerType().Kind);

                var location = file.GetLocationFromOffset((uint)source.IndexOf("Value"));
                var constant = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.EnumConstantDecl, constant.Kind);
                Assert.AreEqual(1ul, constant.GetEnumConstantDeclUnsignedValue());
            }

            source = "enum A : long long { Value = -1 };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(CursorKind.EnumDecl, declaration.Kind);
                Assert.AreEqual(TypeKind.LongLong, declaration.GetEnumDeclIntegerType().Kind);

                var location = file.GetLocationFromOffset((uint)source.IndexOf("Value"));
                var constant = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.EnumConstantDecl, constant.Kind);
                Assert.AreEqual(-1L, constant.GetEnumConstantDeclValue());
            }
        }

        [TestMethod]
        public void GetBitFieldWidth()
        {
            string source = "struct A { unsigned int a : 3; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 12));
                Assert.AreEqual(CursorKind.FieldDecl, declaration.Kind);
                Assert.IsTrue(declaration.IsBitField());
                Assert.AreEqual(3, declaration.GetFieldDeclBitWidth());
            }
        }

        [TestMethod]
        public void GetFunctionArguments()
        {
            string source = "void foo(int a, int b);";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(CursorKind.FunctionDecl, declaration.Kind);
                Assert.AreEqual(2, declaration.GetNumArguments());

                var a = declaration.GetArgument(0);
                Assert.AreEqual(CursorKind.ParmDecl, a.Kind);
                Assert.AreEqual(TypeKind.Int, a.GetTypeInfo().Kind);

                var b = declaration.GetArgument(1);
                Assert.AreEqual(CursorKind.ParmDecl, b.Kind);
                Assert.AreEqual(TypeKind.Int, b.GetTypeInfo().Kind);
            }
        }

        [TestMethod]
        public void GetTemplateFunctionInfo()
        {
            string source = string.Join(" ",
                "template<typename T, int kInt, bool kBool> void foo();",
                "template<> void foo<float, -7, true>();");
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("template<>"));
                var declaration = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.FunctionDecl, declaration.Kind);
                Assert.AreEqual(3, declaration.GetNumTemplateArguments());

                Assert.AreEqual(
                    TemplateArgumentKind.Type,
                    declaration.GetTemplateArgumentKind(0));
                Assert.AreEqual(
                    TemplateArgumentKind.Integral,
                    declaration.GetTemplateArgumentKind(1));
                Assert.AreEqual(
                    TemplateArgumentKind.Integral,
                    declaration.GetTemplateArgumentKind(2));

                Assert.AreEqual(
                    TypeKind.Float,
                    declaration.GetTemplateArgumentType(0).Kind);
                Assert.AreEqual(
                    TypeKind.Invalid,
                    declaration.GetTemplateArgumentType(1).Kind);
                Assert.AreEqual(
                    TypeKind.Invalid,
                    declaration.GetTemplateArgumentType(2).Kind);

                Assert.AreEqual(
                    -7L,
                    declaration.GetTemplateArgumentValue(1));
                Assert.AreEqual(
                    Convert.ToInt64(true),
                    declaration.GetTemplateArgumentValue(2));

                Assert.AreEqual(
                   Convert.ToUInt64(true),
                   declaration.GetTemplateArgumentUnsignedValue(2));

                Assert.AreEqual(TypeKind.Void, declaration.GetResultType().Kind);
            }
        }

        [TestMethod]
        public void GetFieldOffset()
        {
            string source = "struct A { int a; int b; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf('a'));
                var declaration = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.FieldDecl, declaration.Kind);
                long offset;
                var error = declaration.TryGetOffsetOfField(out offset);
                Assert.IsNull(error);
                Assert.AreEqual(0, offset);

                location = file.GetLocationFromOffset((uint)source.IndexOf('b'));
                declaration = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.FieldDecl, declaration.Kind);
                error = declaration.TryGetOffsetOfField(out offset);
                Assert.IsNull(error);
                Assert.AreEqual(32, offset);
            }
        }

        [TestMethod]
        public void AnonymousStruct()
        {
            string source = "struct { struct { int a; }; } b;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 10));
                Assert.AreEqual(CursorKind.StructDecl, declaration.Kind);
                Assert.IsTrue(declaration.IsAnonymous());
            }
        }

        [TestMethod]
        public void VirtualInheritanceAndPublicMember()
        {
            string source = "class A { }; class B : virtual A { public: int a; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("virtual"));
                var specifier = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.CXXBaseSpecifier, specifier.Kind);
                Assert.IsTrue(specifier.IsVirtualBase());

                location = file.GetLocationFromOffset((uint)source.IndexOf("int a"));
                var field = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.FieldDecl, field.Kind);
                Assert.AreEqual(CXXAccessSpecifier.Public, field.GetCXXAccessSpecifier());
            }
        }

        [TestMethod]
        public void StaticVariableInFunction()
        {
            string source = "int foo() { static int a = 0; return a++; }";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 13));
                Assert.AreEqual(CursorKind.VarDecl, declaration.Kind);
                Assert.AreEqual(StorageClass.Static, declaration.GetStorageClass());
            }
        }

        [TestMethod]
        public void FunctionOverloadsAreUnresolvedInUsingDeclarations()
        {
            string source = "void foo(int a); void foo(float a); using ::foo;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("foo;"));
                var reference = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.OverloadedDeclRef, reference.Kind);
                Assert.AreEqual(2u, reference.GetNumOverloadedDecls());

                var declaration1 = empty.GetCursor(file.GetLocation(1, 1));
                var declaration2 = empty.GetCursor(file.GetLocation(1, 18));

                Assert.AreEqual(CursorKind.FunctionDecl, declaration1.Kind);
                Assert.AreEqual(CursorKind.FunctionDecl, declaration2.Kind);
                CollectionAssert.AreEquivalent(
                    new[] { declaration1, declaration2 },
                    new[] { reference.GetOverloadedDecl(0), reference.GetOverloadedDecl(1) });
            }
        }

        [TestMethod]
        public void USRsAreNotEqual()
        {
            string source = "void foo(int a); void foo(float a);";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration1 = empty.GetCursor(file.GetLocation(1, 1));
                var declaration2 = empty.GetCursor(file.GetLocation(1, 18));
                Assert.AreNotEqual(declaration1.GetUSR(), declaration2.GetUSR());
            }
        }

        [TestMethod]
        public void SpellingsOfFunctionDeclarationsAreFunctionNames()
        {
            string source = "void foo(int a); void bar(float a);";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration1 = empty.GetCursor(file.GetLocation(1, 1));
                var declaration2 = empty.GetCursor(file.GetLocation(1, 18));
                Assert.AreEqual("foo", declaration1.GetSpelling());
                Assert.AreEqual("bar", declaration2.GetSpelling());

                var range = declaration1.GetSpellingNameRange();
                Assert.AreEqual(5u, range.GetStart().Offset);
                Assert.AreEqual(8u, range.GetEnd().Offset);
            }
        }

        [TestMethod]
        public void FunctionDisplayNamesIncludeSignatures()
        {
            string source = "void foo(int a);";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual("foo(int)", declaration.GetDisplayName());
            }
        }

        [TestMethod]
        public void GetReferencedCursor()
        {
            string source = "int foo(int a) { return a; }";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var reference = empty.GetCursor(file.GetLocation(1, 25));
                Assert.AreEqual(CursorKind.DeclRefExpr, reference.Kind);
                var declaration = empty.GetCursor(file.GetLocation(1, 9));
                Assert.AreEqual(CursorKind.ParmDecl, declaration.Kind);
                Assert.AreEqual(declaration, reference.GetCursorReferenced());
            }
        }

        [TestMethod]
        public void GetDefinition()
        {
            string source = string.Join(" ",
                "int f(int, int);",
                "int g(int x, int y) { return f(x, y); }",
                "int f(int a, int b) { return a + b; }",
                "int f(int, int);");
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("int f(int a"));
                var definition = empty.GetCursor(location);
                Assert.IsTrue(definition.IsDefinition());

                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(definition, declaration.GetDefinition());

                location = file.GetLocationFromOffset((uint)source.IndexOf("f(x, y)"));
                declaration = empty.GetCursor(location);
                Assert.AreEqual(definition, declaration.GetDefinition());

                location = file.GetLocationFromOffset((uint)source.IndexOf("int f(int, int);"));
                declaration = empty.GetCursor(location);
                Assert.AreEqual(definition, declaration.GetDefinition());
            }
        }

        [TestMethod]
        public void CanonicalCursorsAreEqualForDuplicatedDeclarations()
        {
            string source = "struct A; struct A; struct A { int a; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration1 = empty.GetCursor(file.GetLocation(1, 1));
                var declaration2 = empty.GetCursor(file.GetLocation(1, 11));
                Assert.AreNotEqual(declaration1, declaration2);
                Assert.AreEqual(
                    declaration1.GetCanonicalCursor(),
                    declaration2.GetCanonicalCursor());
            }
        }

        [TestMethod]
        public void VirtualFunctionCallsAreDynamicCalls()
        {
            string source = "class A { public: virtual void foo(); }; void bar() { A().foo(); }";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.LastIndexOf("foo"));
                var call = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.MemberRefExpr, call.Kind);
                Assert.IsTrue(call.IsDynamicCall());
            }
        }

        [TestMethod]
        public void VariadicFunction()
        {
            string source = "void foo(int...);";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var declaration = empty.GetCursor(file.GetLocation(1, 1));
                Assert.IsTrue(declaration.IsVariadic());
            }
        }

        [TestMethod]
        public void GetComment()
        {
            var builder = new StringBuilder();
            builder.AppendLine("/// test");
            builder.AppendLine("void foo();");
            string source = builder.ToString();
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("void"));
                var declaration = empty.GetCursor(location);
                var range = declaration.GetCommentRange();
                Assert.IsNotNull(range);
                Assert.AreEqual("/// test", declaration.GetRawCommentText());
                Assert.AreEqual("test", declaration.GetBriefCommentText());
            }
        }

        [TestMethod]
        public void NameMangling()
        {
            string source = "class A { public: A(); void foo(); };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("A()"));
                var constructor = empty.GetCursor(location);
                Assert.AreNotEqual(0, constructor.GetCXXManglings().Length);

                location = file.GetLocationFromOffset((uint)source.IndexOf("void"));
                var method = empty.GetCursor(location);
                Assert.IsFalse(string.IsNullOrEmpty(method.GetMangling()));
            }
        }

        [TestMethod]
        public void CPlusPlusMethods()
        {
            string source = "class A { mutable int a; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("a;"));
                var field = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.FieldDecl, field.Kind);
                Assert.IsTrue(field.IsMutableField());
            }

            source = "class A { virtual void foo() const = 0; };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("foo"));
                var method = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.CXXMethod, method.Kind);
                Assert.IsTrue(method.IsPureVirtualMethod());
                Assert.IsTrue(method.IsVirtualMethod());
                Assert.IsTrue(method.IsConstMethod());
            }

            source = "class A { static void foo(); };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.IndexOf("foo"));
                var method = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.CXXMethod, method.Kind);
                Assert.IsTrue(method.IsStaticMethod());
            }
        }

        [TestMethod]
        public void TemplateCursors()
        {
            string source = "template<typename T> class A { }; template<> class A<int> { };";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);

                var location = file.GetLocationFromOffset((uint)source.IndexOf("class A"));
                var template = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.ClassTemplate, template.Kind);
                Assert.AreEqual(CursorKind.ClassDecl, template.GetTemplateCursorKind());

                location = file.GetLocationFromOffset((uint)source.IndexOf("class A<int>"));
                var specialization = empty.GetCursor(location);
                Assert.AreEqual(CursorKind.ClassDecl, specialization.Kind);
                Assert.AreEqual(template, specialization.GetSpecializedTemplate());
            }
        }

        [TestMethod]
        public void GetQualifierInReferenceName()
        {
            string source = "namespace A { int a; } int b = A::a;";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)source.LastIndexOf("a;"));
                var cursor = empty.GetCursor(location);
                var range1 = cursor.GetReferenceNameRange(NameReferenceFlags.None, 0);
                Assert.AreEqual((uint)source.LastIndexOf("a;"), range1.GetStart().Offset);
                Assert.AreEqual((uint)source.LastIndexOf("a;") + 1, range1.GetEnd().Offset);
                var range2 = cursor.GetReferenceNameRange(NameReferenceFlags.WantQualifier, 0);
                Assert.AreEqual((uint)source.IndexOf("A::a"), range2.GetStart().Offset);
                Assert.AreEqual((uint)source.LastIndexOf("a;"), range2.GetEnd().Offset);
                var range3 = cursor.GetReferenceNameRange(NameReferenceFlags.WantQualifier, 1);
                Assert.AreEqual(range1, range3);
            }
        }
    }
}
