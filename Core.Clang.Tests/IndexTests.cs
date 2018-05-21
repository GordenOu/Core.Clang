using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class IndexTests : ClangTests, IDisposable
    {
        private Index index;

        public IndexTests()
        {
            Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            index = new Index(true, true);
        }

        [TestCleanup]
        public void Dispose()
        {
            index.Dispose();
        }

        [TestMethod]
        public void SetAndGetGlobalOptions()
        {
            var options = GlobalOptions.ThreadBackgroundPriorityForAll;
            index.SetGlobalOptions(options);
            Assert.AreEqual(options, index.GetGlobalOptions());

            options =
                GlobalOptions.ThreadBackgroundPriorityForEditing |
                GlobalOptions.ThreadBackgroundPriorityForIndexing;
            index.SetGlobalOptions(options);
            Assert.AreEqual(
                GlobalOptions.ThreadBackgroundPriorityForAll,
                index.GetGlobalOptions());
        }

        [TestMethod]
        public void InvocationEmissionPathForCrashLog()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            index.SetInvocationEmissionPathOption(tempPath);
            string source = "void Test();";
            using (var translationUnit = index.ParseTranslationUnit(
               TestFiles.Empty,
               new[] { "-std=c++11" },
               new[] { new UnsavedFile(TestFiles.Empty, source) },
               TranslationUnitCreationOptions.DetailedPreprocessingRecord))
            { }
            string[] files = Directory.GetFiles(tempPath);
            Assert.AreEqual(0, files.Length);

            source = "#pragma clang __debug parser_crash";
            var exception = Assert.ThrowsException<ErrorCodeException>(() =>
            {
                using (var translationUnit = index.ParseTranslationUnit(
                   TestFiles.Empty,
                   new[] { "-std=c++11" },
                   new[] { new UnsavedFile(TestFiles.Empty, source) },
                   TranslationUnitCreationOptions.DetailedPreprocessingRecord))
                { }
            });
            Assert.AreEqual(ErrorCode.Crashed, exception.ErrorCode);
            files = Directory.GetFiles(tempPath);
            Assert.AreEqual(1, files.Length);
            Assert.IsTrue(Path.GetFileName(files[0]).StartsWith("libclang-"));
        }
    }
}
