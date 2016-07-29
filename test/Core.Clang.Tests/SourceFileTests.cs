using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public unsafe class SourceFileTests : IDisposable
    {
        private Index index;
        private TranslationUnit tuAdd;
        private TranslationUnit tuMultiply;

        public SourceFileTests()
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
            tuAdd = new TranslationUnit(ptr, index);
            using (var fileName = new CString(TestFiles.MultiplySource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    index.Ptr,
                    fileName.Ptr,
                    null, 0,
                    null, 0,
                    0,
                    &ptr).Check();
            }
            tuMultiply = new TranslationUnit(ptr, index);
        }

        [TestCleanup]
        public void Dispose()
        {
            tuAdd.Dispose();
            tuMultiply.Dispose();
            index.Dispose();
            Monitor.Exit(TestFiles.Locker);
        }

        [TestMethod]
        public void SourceFileEquatable()
        {
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
            Assert.IsFalse(tuAdd.GetFile(TestFiles.CommonHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetFile(TestFiles.AddHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetFile(TestFiles.MultiplyHeader).IsMultipleIncludeGuarded());
            Assert.IsFalse(tuAdd.GetFile(TestFiles.AddSource).IsMultipleIncludeGuarded());
        }

        [TestMethod]
        public void GetValidLocation()
        {
            var file = tuAdd.GetFile(TestFiles.AddHeader);
            Assert.IsNotNull(file.GetLocation(1, 1));
            Assert.IsNotNull(file.GetLocationFromOffset(0));
        }

        [TestMethod]
        public void GetInvalidLocation()
        {
            var file = tuAdd.GetFile(TestFiles.AddHeader);
            Assert.IsNull(file.GetLocation(0, 0));
        }
    }
}
