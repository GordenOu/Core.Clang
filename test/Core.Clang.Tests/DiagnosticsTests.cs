using System;
using System.Threading;
using Core.Clang.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
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
        public void GetOneDiagnosticInTranslationUnit()
        {
            string source = "#endif";
            using (var file = new UnsavedFile(TestFiles.Empty, source))
            using (var empty = disposables.Index.ParseTranslationUnit(
                TestFiles.Empty,
                null,
                new[] { file },
                TranslationUnitCreationOptions.DetailedPreprocessingRecord))
            {
                var set = DiagnosticSet.FromTranslationUnit(empty);
                Assert.AreEqual<uint>(1, set.GetNumDiagnostics());
                Assert.IsNotNull(set.GetDiagnostic(0));
                Assert.IsNull(set.GetDiagnostic(1));

                var diagnostic = set.GetDiagnostic(0);
                Assert.IsNull(diagnostic.GetChildDiagnostics());
                Assert.AreEqual(DiagnosticSeverity.Error, diagnostic.GetSeverity());
            }
        }
    }
}
