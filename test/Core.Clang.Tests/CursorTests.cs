using System;
using System.Threading;
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
            var tuAdd = disposables.Add;

            var addHeader = tuAdd.GetFile(TestFiles.AddHeader);
            Assert.AreEqual(
                CursorKind.FunctionDecl,
                tuAdd.GetCursor(addHeader.GetLocation(3, 1)).Kind);
            Assert.AreEqual(
                CursorKind.ParmDecl,
                tuAdd.GetCursor(addHeader.GetLocation(3, 9)).Kind);

            var addSource = tuAdd.GetFile(TestFiles.AddSource);
            Assert.AreEqual(
                CursorKind.FunctionDecl,
                tuAdd.GetCursor(addSource.GetLocation(3, 1)).Kind);
            Assert.AreEqual(
                CursorKind.CompoundStmt,
                tuAdd.GetCursor(addSource.GetLocation(4, 1)).Kind);
            Assert.AreEqual(
                CursorKind.ReturnStmt,
                tuAdd.GetCursor(addSource.GetLocation(5, 5)).Kind);
        }

        [TestMethod]
        public void CursorEquatable()
        {
            var tuAdd = disposables.Add;
            var tuMultiply = disposables.Multiply;

            Assert.AreEqual(tuAdd.GetCursor(), tuAdd.GetCursor());
            Assert.IsTrue(tuAdd.GetCursor().Equals(tuAdd.GetCursor()));
            Assert.AreNotEqual(tuAdd.GetCursor(), tuMultiply.GetCursor());
            Assert.IsFalse(tuAdd.GetCursor().Equals(tuMultiply.GetCursor()));
            Assert.AreEqual(tuAdd.GetCursor().GetHashCode(), tuAdd.GetCursor().GetHashCode());
            Assert.AreNotEqual(
                tuAdd.GetCursor().GetHashCode(),
                tuMultiply.GetCursor().GetHashCode());
        }

        [TestMethod]
        public void LinkageKinds()
        {
            string source = "void a() { int b; } static int d; namespace { int c; } ";
            using (var translationUnit = disposables.WriteToEmpty(source))
            {
                var file = translationUnit.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LinkageKind.External,
                    translationUnit.GetCursor(file.GetLocation(1, 6)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.NoLinkage,
                    translationUnit.GetCursor(file.GetLocation(1, 16)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.Internal,
                    translationUnit.GetCursor(file.GetLocation(1, 32)).GetLinkage());
                Assert.AreEqual(
                    LinkageKind.UniqueExternal,
                    translationUnit.GetCursor(file.GetLocation(1, 51)).GetLinkage());
            }
        }

        [TestMethod]
        public void CAndCPlusPlusLanguageKindsAreInferred()
        {
            string source = "int a = 0;";
            using (var translationUnit = disposables.WriteToEmpty(source))
            {
                var file = translationUnit.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LanguageKind.C,
                    translationUnit.GetCursor(file.GetLocation(1, 1)).GetLanguage());
            }

            source = "class a { };";
            using (var translationUnit = disposables.WriteToEmpty(source))
            {
                var file = translationUnit.GetFile(TestFiles.Empty);
                Assert.AreEqual(
                    LanguageKind.CPlusPlus,
                    translationUnit.GetCursor(file.GetLocation(1, 1)).GetLanguage());
            }
        }

        [TestMethod]
        public void SemanticAndLexicalParentAreNotEqual()
        {
            string source = "class C { void f(); }; void C::f() { }";
            using (var translationUnit = disposables.WriteToEmpty(source))
            {
                var file = translationUnit.GetFile(TestFiles.Empty);
                var declaration = translationUnit.GetCursor(file.GetLocation(1, 11));
                var definition = translationUnit.GetCursor(file.GetLocation(1, 24));
                var classC = translationUnit.GetCursor(file.GetLocation(1, 1));
                Assert.AreEqual(classC, declaration.GetSemanticParent());
                Assert.AreEqual(classC, declaration.GetLexicalParent());
                Assert.AreEqual(classC, definition.GetSemanticParent());
                Assert.AreEqual(translationUnit.GetCursor(), definition.GetLexicalParent());
                Assert.AreNotEqual(classC, translationUnit.GetCursor());
            }
        }
    }
}
