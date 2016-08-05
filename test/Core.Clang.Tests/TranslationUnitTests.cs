using System;
using System.IO;
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
            var add = disposables.Add;

            Assert.IsNotNull(add.GetFile(TestFiles.AddSource));
            Assert.IsNotNull(add.GetFile(TestFiles.AddHeader));
            Assert.IsNotNull(add.GetFile(TestFiles.CommonHeader));
            Assert.IsNull(add.GetFile(TestFiles.CommonHeader + "pp"));
        }

        [TestMethod]
        public void SkipsConditionalCompilationSections()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddSource);
            var skippedRanges = add.GetSkippedRanges(file);
            Assert.IsNotNull(skippedRanges);
            Assert.AreEqual(1, skippedRanges.Length);
        }

        [TestMethod]
        public void GetSpellingReturnsSourceFileName()
        {
            Assert.AreEqual(TestFiles.AddSource, disposables.Add.GetSpelling());
            Assert.AreEqual(TestFiles.MultiplySource, disposables.Multiply.GetSpelling());
        }

        [TestMethod]
        public void SaveAndLoad()
        {
            var fileName = Path.GetTempFileName();
            Assert.AreEqual(TranslationUnitSaveError.None, disposables.Add.TrySave(fileName));
            var add = disposables.Index.CreateTranslationUnit(fileName);
            Assert.AreEqual(
                disposables.Add.GetFile(TestFiles.AddSource),
                add.GetFile(TestFiles.AddSource));
        }

        [TestMethod]
        public void InvalidatedAfterReparse()
        {
            TranslationUnit add;
            Assert.AreEqual(ErrorCode.Success, disposables.Add.TryReparse(null, out add));
            Assert.ThrowsException<ObjectDisposedException>(() => disposables.Add.GetSpelling());
            Assert.AreEqual(TestFiles.AddSource, add.GetSpelling());
            add.Dispose();
        }

        [TestMethod]
        public void ResourceUsageEntriesNotEmpty()
        {
            var entries = disposables.Add.GetResourceUsage();
            Assert.IsTrue(entries.Length != 0);
        }
    }
}
