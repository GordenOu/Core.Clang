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
            tuAdd = new TranslationUnit(ptr, index);
            using (var sourceFileName = new CString(TestFiles.MultiplySource))
            {
                NativeMethods.clang_parseTranslationUnit2(
                    index.Ptr,
                    sourceFileName.Ptr,
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
            tuAdd.GetSourceFile(TestFiles.CommonHeader),
            tuMultiply.GetSourceFile(TestFiles.CommonHeader));
            Assert.IsTrue(
                tuAdd.GetSourceFile(TestFiles.CommonHeader).Equals(
                    tuMultiply.GetSourceFile(TestFiles.CommonHeader)));
            Assert.AreEqual(
                tuAdd.GetSourceFile(TestFiles.CommonHeader).GetHashCode(),
                tuMultiply.GetSourceFile(TestFiles.CommonHeader).GetHashCode());

            Assert.AreEqual(
                tuAdd.GetSourceFile(TestFiles.AddHeader),
                tuMultiply.GetSourceFile(TestFiles.AddHeader));
            Assert.IsTrue(
                tuAdd.GetSourceFile(TestFiles.AddHeader).Equals(
                    tuMultiply.GetSourceFile(TestFiles.AddHeader)));
            Assert.AreEqual(
                tuAdd.GetSourceFile(TestFiles.AddHeader).GetHashCode(),
                tuMultiply.GetSourceFile(TestFiles.AddHeader).GetHashCode());

            Assert.AreEqual(
                tuAdd.GetSourceFile(TestFiles.MultiplyHeader),
                tuMultiply.GetSourceFile(TestFiles.MultiplyHeader));
            Assert.IsTrue(
                tuAdd.GetSourceFile(TestFiles.MultiplyHeader).Equals(
                    tuMultiply.GetSourceFile(TestFiles.MultiplyHeader)));
            Assert.AreEqual(
                tuAdd.GetSourceFile(TestFiles.MultiplyHeader).GetHashCode(),
                tuMultiply.GetSourceFile(TestFiles.MultiplyHeader).GetHashCode());

            Assert.AreNotEqual(
                tuAdd.GetSourceFile(TestFiles.AddHeader),
                tuMultiply.GetSourceFile(TestFiles.MultiplyHeader));
            Assert.AreNotEqual(
                tuAdd.GetSourceFile(TestFiles.AddHeader).GetHashCode(),
                tuMultiply.GetSourceFile(TestFiles.MultiplyHeader).GetHashCode());
        }

        [TestMethod]
        public void GetName()
        {
            Assert.AreEqual(
               TestFiles.CommonHeader,
               tuAdd.GetSourceFile(TestFiles.CommonHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddHeader,
                tuAdd.GetSourceFile(TestFiles.AddHeader).GetName());
            Assert.AreEqual(
                TestFiles.AddSource,
                tuAdd.GetSourceFile(TestFiles.AddSource).GetName());
        }

        [TestMethod]
        public void MultipleIncludeGuarded()
        {
            Assert.IsFalse(tuAdd.GetSourceFile(TestFiles.CommonHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetSourceFile(TestFiles.AddHeader).IsMultipleIncludeGuarded());
            Assert.IsTrue(tuAdd.GetSourceFile(TestFiles.MultiplyHeader).IsMultipleIncludeGuarded());
            Assert.IsFalse(tuAdd.GetSourceFile(TestFiles.AddSource).IsMultipleIncludeGuarded());
        }
    }
}
