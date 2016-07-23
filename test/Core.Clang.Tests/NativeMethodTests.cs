using System;
using System.Runtime.InteropServices;

namespace Core.Clang.Tests
{
    public unsafe class NativeMethodTests
    {
        [TestMethod]
        public void ClangVersion()
        {
            var cxString = NativeMethods.clang_getClangVersion();
            var ptr = NativeMethods.clang_getCString(cxString);
            string version = Marshal.PtrToStringAnsi(new IntPtr(ptr));
            try
            {
                Assert.AreEqual("clang version 3.8.1 (branches/release_38)", version);
            }
            finally
            {
                NativeMethods.clang_disposeString(cxString);
            }
        }
    }
}
