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
            var add = disposables.Add;
            var multiply = disposables.Multiply;

            Assert.AreEqual(
                add.GetFile(TestFiles.CommonHeader),
                multiply.GetFile(TestFiles.CommonHeader));
            Assert.IsTrue(
                add.GetFile(TestFiles.CommonHeader).Equals(
                    multiply.GetFile(TestFiles.CommonHeader)));
            Assert.AreEqual(
                add.GetFile(TestFiles.CommonHeader).GetHashCode(),
                multiply.GetFile(TestFiles.CommonHeader).GetHashCode());

            Assert.AreEqual(
                add.GetFile(TestFiles.AddHeader),
                multiply.GetFile(TestFiles.AddHeader));
            Assert.IsTrue(
                add.GetFile(TestFiles.AddHeader).Equals(
                    multiply.GetFile(TestFiles.AddHeader)));
            Assert.AreEqual(
                add.GetFile(TestFiles.AddHeader).GetHashCode(),
                multiply.GetFile(TestFiles.AddHeader).GetHashCode());

            Assert.AreEqual(
                add.GetFile(TestFiles.MultiplyHeader),
                multiply.GetFile(TestFiles.MultiplyHeader));
            Assert.IsTrue(
                add.GetFile(TestFiles.MultiplyHeader).Equals(
                    multiply.GetFile(TestFiles.MultiplyHeader)));
            Assert.AreEqual(
                add.GetFile(TestFiles.MultiplyHeader).GetHashCode(),
                multiply.GetFile(TestFiles.MultiplyHeader).GetHashCode());

            Assert.AreNotEqual(
                add.GetFile(TestFiles.AddHeader),
                multiply.GetFile(TestFiles.MultiplyHeader));
            Assert.AreNotEqual(
                add.GetFile(TestFiles.AddHeader).GetHashCode(),
                multiply.GetFile(TestFiles.MultiplyHeader).GetHashCode());
        }

        [TestMethod]
        public void GetName()
        {
            var add = disposables.Add;
            var multiply = disposables.Multiply;

            Assert.AreEqual(
                TestFiles.CommonHeader,
                add.GetFile(TestFiles.CommonHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddHeader,
                add.GetFile(TestFiles.AddHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddSource,
                add.GetFile(TestFiles.AddSource).GetName());
        }

        [TestMethod]
        public void MultipleIncludeGuarded()
        {
            var add = disposables.Add;

            Assert.IsFalse(add.GetFile(TestFiles.CommonHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(add.GetFile(TestFiles.AddHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(add.GetFile(TestFiles.MultiplyHeader).IsMultipleIncludeGuarded());
            Assert.IsFalse(add.GetFile(TestFiles.AddSource).IsMultipleIncludeGuarded());
        }

        [TestMethod]
        public void GetValidLocation()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddHeader);
            Assert.IsNotNull(file.GetLocation(1, 1));
            Assert.IsNotNull(file.GetLocationFromOffset(0));
        }

        [TestMethod]
        public void GetInvalidLocation()
        {
            var add = disposables.Add;

            var file = add.GetFile(TestFiles.AddHeader);
            Assert.IsNull(file.GetLocation(0, 0));
        }
    }
}
