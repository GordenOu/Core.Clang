using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public unsafe class SourceRangeTests : IDisposable
    {
        private Index index;
        private TranslationUnit translationUnit;

        public SourceRangeTests()
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
                    0,
                    &ptr).Check();
            }
            translationUnit = new TranslationUnit(ptr, index);
        }

        [TestCleanup]
        public void Dispose()
        {
            translationUnit.Dispose();
            index.Dispose();
            Monitor.Exit(TestFiles.Locker);
        }

        [TestMethod]
        public void CreateInvalidRangeReturnsNull()
        {
            var file = translationUnit.GetFile(TestFiles.AddSource);
            var range = SourceRange.Create(file.GetLocation(1, 1), file.GetLocation(2, 1));
            Assert.IsNotNull(range);

            range = SourceRange.Create(
                translationUnit.GetFile(TestFiles.AddHeader).GetLocation(1, 1),
                file.GetLocation(1, 1));
            Assert.IsNull(range);
        }

        [TestMethod]
        public void GetStartAndGetEndReturnOriginalLocations()
        {
            var file = translationUnit.GetFile(TestFiles.AddSource);
            var begin = file.GetLocation(1, 1);
            var end = file.GetLocation(2, 1);
            var range = SourceRange.Create(begin, end);

            Assert.AreEqual(begin, range.GetStart());
            Assert.AreEqual(end, range.GetEnd());
        }
    }
}
