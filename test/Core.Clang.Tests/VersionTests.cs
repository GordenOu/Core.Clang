using System.Linq;
using System.Reflection;
using Core.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public unsafe class VersionTests
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

        [TestMethod]
        public void UnstableAttributesAreUpToDate()
        {
            var assembly = typeof(Index).GetTypeInfo().Assembly;
            var version = assembly.GetName().Version.ToString();
            (from type in assembly.GetTypes()
             from method in type.GetMethods()
             from attribute in method.GetCustomAttributes<UnstableAttribute>()
             select attribute).Apply(attribute =>
             {
                 Assert.IsTrue(version.StartsWith(attribute.Version));
             });
        }
    }
}
