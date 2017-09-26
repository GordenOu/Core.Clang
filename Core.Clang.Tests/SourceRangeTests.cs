using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class SourceRangeTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public SourceRangeTests()
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
        public void CreateInvalidRangeReturnsNull()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddSource);
            var range = SourceRange.Create(file.GetLocation(1, 1), file.GetLocation(2, 1));
            Assert.IsNotNull(range);

            range = SourceRange.Create(
                add.GetFile(TestFiles.AddHeader).GetLocation(1, 1),
                file.GetLocation(1, 1));
            Assert.IsNull(range);
        }

        [TestMethod]
        public void GetStartAndGetEndReturnOriginalLocations()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddSource);
            var begin = file.GetLocation(1, 1);
            var end = file.GetLocation(2, 1);
            var range = SourceRange.Create(begin, end);

            Assert.AreEqual(begin, range.GetStart());
            Assert.AreEqual(end, range.GetEnd());
        }
    }
}
