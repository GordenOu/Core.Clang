using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Core.Clang.Tests
{
    public class ClangTests
    {
        private static string GetFilePath([CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        static ClangTests()
        {
            var solutionDirectory = new FileInfo(GetFilePath()).Directory.Parent.Parent;
            var path = Environment.GetEnvironmentVariable(nameof(Path));
            path = string.Join(Path.PathSeparator.ToString(),
                Path.Combine(solutionDirectory.FullName, "Native", "LLVM", "bin"),
                path);
            Environment.SetEnvironmentVariable(nameof(Path), path);
        }
    }
}
