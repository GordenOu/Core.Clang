using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public unsafe class TranslationUnitTests : IDisposable
    {
        private Index index;
        private TranslationUnit tranlsationUnit;

        public TranslationUnitTests()
        {
            Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            Monitor.Enter(TestFiles.Locker);
            index = new Index(true, true);
            CXTranslationUnitImpl* ptr;
            using (var fileName = new CString(TestFiles.AddSource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    index.Ptr,
                    fileName.Ptr,
                    null, 0,
                    null, 0,
                    (uint)CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord,
                    &ptr).Check();
            }
            tranlsationUnit = new TranslationUnit(ptr, index);
        }

        [TestCleanup]
        public void Dispose()
        {
            tranlsationUnit.Dispose();
            index.Dispose();
            Monitor.Exit(TestFiles.Locker);
        }

        [TestMethod]
        public void GetSourceFile()
        {
            Assert.IsNotNull(tranlsationUnit.GetFile(TestFiles.AddSource));
            Assert.IsNotNull(tranlsationUnit.GetFile(TestFiles.AddHeader));
            Assert.IsNotNull(tranlsationUnit.GetFile(TestFiles.CommonHeader));
            Assert.IsNull(tranlsationUnit.GetFile(TestFiles.CommonHeader + "pp"));
        }

        [TestMethod]
        public void SkipsConditionalCompilationSections()
        {
            var file = tranlsationUnit.GetFile(TestFiles.AddSource);
            var skippedRanges = tranlsationUnit.GetSkippedRanges(file);
            Assert.IsNotNull(skippedRanges);
            Assert.AreEqual(1, skippedRanges.Length);
        }
    }
}
