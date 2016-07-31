using System;
using Core.Clang.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public class DiagnosticsTests : IDisposable
    {
        private Disposables disposables;

        public DiagnosticsTests()
        {
            Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            disposables = new Disposables();
        }

        [TestCleanup]
        public void Dispose()
        {
            disposables.Dispose();
        }

        [TestMethod]
        public void GetOneDiagnosticInTranslationUnit()
        {
            var translationUnit = disposables.Add;

            var set = DiagnosticSet.FromTranslationUnit(translationUnit);
            Assert.AreEqual<uint>(1, set.GetNumDiagnostics());
            Assert.IsNotNull(set.GetDiagnostic(0));
            Assert.IsNull(set.GetDiagnostic(1));

            var diagnostic = set.GetDiagnostic(0);
            Assert.IsNull(diagnostic.GetChildDiagnostics());
            Assert.AreEqual(DiagnosticSeverity.Error, diagnostic.GetSeverity());
        }
    }
}
