using System.IO;
using System.Runtime.CompilerServices;

namespace Core.Clang.Tests
{
    public static class TestFiles
    {
        public static object Locker { get; } = new object();

        private static string GetFilePath([CallerFilePath] string path = null)
        {
            return path;
        }

        public static string Directory { get; } =
            Path.Combine(new FileInfo(GetFilePath()).Directory.FullName, nameof(TestFiles));

        public static string CommonHeader { get; } = Path.Combine(Directory, "common.h");

        public static string AddHeader { get; } = Path.Combine(Directory, "add.h");

        public static string AddSource { get; } = Path.Combine(Directory, "add.cpp");

        public static string MultiplyHeader { get; } = Path.Combine(Directory, "multiply.h");

        public static string MultiplySource { get; } = Path.Combine(Directory, "multiply.cpp");

        public static string Empty { get; } = Path.Combine(Directory, "empty.cpp");

        public static class Doxygen
        {
            public static string Documentation { get; } = Path.Combine(
                Directory,
                "documentation",
                "doxygen.cpp");

            public static string DoxygenNormalizedDocumentText { get; } = Path.Combine(
                Directory,
                "documentation",
                "NormalizedDocumentText.txt");
        }
    }
}
