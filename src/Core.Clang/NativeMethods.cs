using System.Runtime.InteropServices;

namespace Core.Clang
{
    internal static unsafe class NativeMethods
    {
        private const string dllName = "libclang";

        [DllImport(dllName)]
        public static extern sbyte* clang_getCString(
            CXString @string);

        [DllImport(dllName)]
        public static extern void clang_disposeString(
            CXString @string);

        [DllImport(dllName)]
        public static extern CXString clang_getClangVersion();
    }
}
