using System;
using System.IO;

namespace Core.Clang.Tests
{
    public class ClangTests
    {
        public ClangTests()
        {
            var path = Environment.GetEnvironmentVariable(nameof(Path));
            path = string.Join(Path.PathSeparator.ToString(),
                Path.Combine(Native.LibClang.Restore.Program.LLVMDirectory, "bin"),
                path);
            Environment.SetEnvironmentVariable(nameof(Path), path);
        }
    }
}
