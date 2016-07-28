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
            using (var sourceFileName = new CString(TestFiles.AddSource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    index.Ptr,
                    sourceFileName.Ptr,
                    null, 0,
                    null, 0,
                    0,
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
            Assert.IsNotNull(tranlsationUnit.GetSourceFile(TestFiles.AddSource));
            Assert.IsNotNull(tranlsationUnit.GetSourceFile(TestFiles.AddHeader));
            Assert.IsNotNull(tranlsationUnit.GetSourceFile(TestFiles.CommonHeader));
            Assert.IsNull(tranlsationUnit.GetSourceFile(TestFiles.CommonHeader + "pp"));
        }
    }
}
