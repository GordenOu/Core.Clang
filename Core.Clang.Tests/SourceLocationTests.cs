using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class SourceLocationTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public SourceLocationTests()
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
        public void SourceLocationEquatable()
        {
            var add = disposables.Add;
            var file = add.GetFile(TestFiles.MultiplyHeader);

            var location1 = file.GetLocation(1, 2);
            var location2 = file.GetLocationFromOffset(1);
            Assert.AreEqual(location1, location2);
            Assert.IsTrue(location1.Equals(location2));
            Assert.AreEqual(location1.GetHashCode(), location2.GetHashCode());

            location2 = file.GetLocationFromOffset(2);
            Assert.AreNotEqual(location1, location2);
            Assert.IsFalse(location1.Equals(location2));
            Assert.AreNotEqual(location1.GetHashCode(), location2.GetHashCode());
        }

        [TestMethod]
        public void FileProperties()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddHeader);
            Assert.IsFalse(file.GetLocation(1, 1).IsInSystemHeader());
            Assert.IsFalse(file.GetLocation(1, 2).IsFromMainFile());

            file = add.GetFile(TestFiles.AddSource);
            Assert.IsFalse(file.GetLocationFromOffset(0).IsInSystemHeader());
            Assert.IsTrue(file.GetLocationFromOffset(1).IsFromMainFile());
        }
    }
}
