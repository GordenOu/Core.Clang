using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public unsafe class SourceFileTests : IDisposable
    {
        private Disposables disposables;

        public SourceFileTests()
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
        public void SourceFileEquatable()
        {
            var tuAdd = disposables.Add;
            var tuMultiply = disposables.Multiply;

            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.CommonHeader),
                tuMultiply.GetFile(TestFiles.CommonHeader));
            Assert.IsTrue(
                tuAdd.GetFile(TestFiles.CommonHeader).Equals(
                    tuMultiply.GetFile(TestFiles.CommonHeader)));
            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.CommonHeader).GetHashCode(),
                tuMultiply.GetFile(TestFiles.CommonHeader).GetHashCode());

            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.AddHeader),
                tuMultiply.GetFile(TestFiles.AddHeader));
            Assert.IsTrue(
                tuAdd.GetFile(TestFiles.AddHeader).Equals(
                    tuMultiply.GetFile(TestFiles.AddHeader)));
            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.AddHeader).GetHashCode(),
                tuMultiply.GetFile(TestFiles.AddHeader).GetHashCode());

            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.MultiplyHeader),
                tuMultiply.GetFile(TestFiles.MultiplyHeader));
            Assert.IsTrue(
                tuAdd.GetFile(TestFiles.MultiplyHeader).Equals(
                    tuMultiply.GetFile(TestFiles.MultiplyHeader)));
            Assert.AreEqual(
                tuAdd.GetFile(TestFiles.MultiplyHeader).GetHashCode(),
                tuMultiply.GetFile(TestFiles.MultiplyHeader).GetHashCode());

            Assert.AreNotEqual(
                tuAdd.GetFile(TestFiles.AddHeader),
                tuMultiply.GetFile(TestFiles.MultiplyHeader));
            Assert.AreNotEqual(
                tuAdd.GetFile(TestFiles.AddHeader).GetHashCode(),
                tuMultiply.GetFile(TestFiles.MultiplyHeader).GetHashCode());
        }

        [TestMethod]
        public void GetName()
        {
            var tuAdd = disposables.Add;
            var tuMultiply = disposables.Multiply;

            Assert.AreEqual(
                TestFiles.CommonHeader,
                tuAdd.GetFile(TestFiles.CommonHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddHeader,
                tuAdd.GetFile(TestFiles.AddHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddSource,
                tuAdd.GetFile(TestFiles.AddSource).GetName());
        }

        [TestMethod]
        public void MultipleIncludeGuarded()
        {
            var tuAdd = disposables.Add;

            Assert.IsFalse(tuAdd.GetFile(TestFiles.CommonHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetFile(TestFiles.AddHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetFile(TestFiles.MultiplyHeader).IsMultipleIncludeGuarded());
            Assert.IsFalse(tuAdd.GetFile(TestFiles.AddSource).IsMultipleIncludeGuarded());
        }

        [TestMethod]
        public void GetValidLocation()
        {
            var tuAdd = disposables.Add;

            var file = tuAdd.GetFile(TestFiles.AddHeader);
            Assert.IsNotNull(file.GetLocation(1, 1));
            Assert.IsNotNull(file.GetLocationFromOffset(0));
        }

        [TestMethod]
        public void GetInvalidLocation()
        {
            var tuAdd = disposables.Add;

            var file = tuAdd.GetFile(TestFiles.AddHeader);
            Assert.IsNull(file.GetLocation(0, 0));
        }
    }
}
