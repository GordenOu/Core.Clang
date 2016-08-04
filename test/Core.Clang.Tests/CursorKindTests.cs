using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class CursorKindTests
    {
        [TestMethod]
        public void AssertCursorKindRanges()
        {
            Assert.IsTrue(CursorKind.EnumDecl.IsDecalration());
            Assert.IsFalse(CursorKind.DoStmt.IsDecalration());
            Assert.IsTrue(CursorKind.TemplateRef.IsReference());
            Assert.IsFalse(CursorKind.FloatingLiteral.IsReference());
            Assert.IsTrue(CursorKind.UnexposedExpr.IsExpression());
            Assert.IsFalse(CursorKind.CaseStmt.IsExpression());
            Assert.IsTrue(CursorKind.ForStmt.IsStatement());
            Assert.IsFalse(CursorKind.LabelRef.IsStatement());
            Assert.IsTrue(CursorKind.InvalidCode.IsInvalid());
            Assert.IsFalse(CursorKind.Constructor.IsInvalid());
            Assert.IsTrue(CursorKind.TranslationUnit.IsTranslationUnit());
            Assert.IsFalse(CursorKind.CompoundStmt.IsTranslationUnit());
            Assert.IsTrue(CursorKind.MacroDefinition.IsPreprocessing());
            Assert.IsFalse(CursorKind.NullStmt.IsPreprocessing());
            Assert.IsTrue(CursorKind.UnexposedStmt.IsUnexposed());
            Assert.IsFalse(CursorKind.VarDecl.IsUnexposed());
        }
    }
}
