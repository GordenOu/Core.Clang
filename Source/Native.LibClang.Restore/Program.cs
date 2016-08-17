using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Native.LibClang.Restore
{
    public class Program
    {
        private static string GetFilePath([CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        public static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var assembly = Assembly.Load(new AssemblyName("Native.LibClang"));
                using (var stream = assembly.GetManifestResourceStream("Native.LibClang.LLVM.zip"))
                using (var zipArchive = new ZipArchive(stream))
                {
                    string directory = new FileInfo(GetFilePath())
                        .Directory
                        .Parent
                        .Parent
                        .CreateSubdirectory("Native")
                        .CreateSubdirectory("LLVM")
                        .FullName;
                    Directory.Delete(directory, recursive: true);
                    zipArchive.ExtractToDirectory(directory);
                }
            }
            Console.WriteLine("Yo~");
        }
    }
}
