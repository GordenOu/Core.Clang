using System;
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
    }
}
