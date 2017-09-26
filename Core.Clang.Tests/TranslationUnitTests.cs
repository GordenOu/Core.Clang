using System;
using System.IO;
using System.Threading;
using Core.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class TranslationUnitTests : ClangTests, IDisposable
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
            Assert.AreEqual(file, skippedRanges[0].GetStart().SourceFile);

            skippedRanges = add.GetAllSkippedRanges();
            Assert.IsNotNull(skippedRanges);
            Assert.AreEqual(1, skippedRanges.Length);
            Assert.AreEqual(file, skippedRanges[0].GetStart().SourceFile);
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
            string fileName = Path.GetTempFileName();
            Assert.AreEqual(TranslationUnitSaveError.None, disposables.Add.TrySave(fileName));
            using (var add = disposables.Index.CreateTranslationUnit(fileName))
            {
                Assert.AreEqual(
                    disposables.Add.GetFile(TestFiles.AddSource),
                    add.GetFile(TestFiles.AddSource));
            }
        }

        [TestMethod]
        public void InvalidatedAfterReparse()
        {
            var (errorCode, add) = disposables.Add.TryReparse(null);
            Assert.AreEqual(ErrorCode.Success, errorCode);
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

        [TestMethod]
        public void Tokenization()
        {
            string source = "void f(int x); void g(int x) { f(x); }";
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var range = SourceRange.Create(
                    file.GetLocationFromOffset((uint)source.IndexOf("f(x)")),
                    file.GetLocationFromOffset((uint)(source.IndexOf(" }"))));
                var tokens = empty.Tokenize(range);
                CollectionAssert.AreEqual(
                    new[] { "f", "(", "x", ")", ";" },
                    tokens.ToArray(token => token.GetSpelling()));

                Assert.AreEqual(TokenKind.Identifier, tokens[0].GetKind());
                Assert.AreEqual("f", tokens[0].GetSpelling());
                Assert.AreEqual(
                    file.GetLocationFromOffset((uint)source.IndexOf("f(x)")),
                    tokens[0].GetLocation());
                Assert.AreEqual(
                    (uint)source.IndexOf("f(x)"),
                    tokens[0].GetExtent().GetStart().Offset);
                Assert.AreEqual(
                    (uint)source.IndexOf("f(x)") + 1,
                    tokens[0].GetExtent().GetEnd().Offset);

                Assert.AreEqual(TokenKind.Punctuation, tokens[1].GetKind());
                Assert.AreEqual("(", tokens[1].GetSpelling());
                Assert.AreEqual(TokenKind.Identifier, tokens[2].GetKind());
                Assert.AreEqual("x", tokens[2].GetSpelling());
                Assert.AreEqual(TokenKind.Punctuation, tokens[3].GetKind());
                Assert.AreEqual(")", tokens[3].GetSpelling());
            }
        }
    }
}
