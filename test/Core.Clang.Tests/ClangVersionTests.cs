using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public unsafe class ClangVersionTests
    {
        [TestMethod]
        public void ClangVersion()
        {
            var cxString = NativeMethods.clang_getClangVersion();
            using (var str = new String(cxString))
            {
                Assert.AreEqual("clang version 3.8.1 (branches/release_38)", str.ToString());
            }
        }
    }
}
