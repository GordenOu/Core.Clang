using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public unsafe class TranslationUnitTests : IDisposable
    {
        private Disposables disposables;

        public TranslationUnitTests()
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
        public void GetSourceFile()
        {
            var translationUnit = disposables.Add;

            Assert.IsNotNull(translationUnit.GetFile(TestFiles.AddSource));
            Assert.IsNotNull(translationUnit.GetFile(TestFiles.AddHeader));
            Assert.IsNotNull(translationUnit.GetFile(TestFiles.CommonHeader));
            Assert.IsNull(translationUnit.GetFile(TestFiles.CommonHeader + "pp"));
        }

        [TestMethod]
        public void SkipsConditionalCompilationSections()
        {
            var translationUnit = disposables.Add;

            var file = translationUnit.GetFile(TestFiles.AddSource);
            var skippedRanges = translationUnit.GetSkippedRanges(file);
            Assert.IsNotNull(skippedRanges);
            Assert.AreEqual(1, skippedRanges.Length);
        }
    }
}
