using System;
using System.Linq;
using System.Threading;
using Core.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public class CursorTests : IDisposable
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
    }
}
