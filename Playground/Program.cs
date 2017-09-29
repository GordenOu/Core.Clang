using System;
using System.IO;
using System.Runtime.CompilerServices;
using Core.Clang;
using Core.Linq;

namespace Playground
{
    public class Program
    {
        private static string GetFilePath([CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        private static string nativeTypesPath;

        private static string nativeMethodsPath;

        static Program()
        {
            var solutionDirectory = new FileInfo(GetFilePath()).Directory.Parent;
            string path = Environment.GetEnvironmentVariable(nameof(Path));
            path = string.Join(Path.PathSeparator.ToString(),
                Path.Combine(solutionDirectory.FullName, "Native", "LLVM", "bin"),
                path);
            Environment.SetEnvironmentVariable(nameof(Path), path);
            nativeTypesPath = Path.Combine(
                solutionDirectory.FullName, "Core.Clang", "NativeTypes.cs");
            nativeMethodsPath = Path.Combine(
                solutionDirectory.FullName, "Core.Clang", "NativeMethods.cs");
        }

        /// <summary>
        /// Generates the source code in Core.Clang/NativeTypes.cs and Core.Clang/NativeMethods.cs.
        /// </summary>
        /// <param name="includePath">Path to LLVM/include/</param>
        /// <param name="systemIncludePaths">Paths to system header files.</param>
        /// <param name="nativeMethods">
        /// The generated contents in Core.Clang/NativeMethods.cs.
        /// </param>
        /// <returns>The generated contents in Core.Clang/NativeTypes.cs.</returns>
        private static (string nativeTypes, string nativeMethods) ImportNativeTypesAndMethods(
            string includePath,
            string[] systemIncludePaths)
        {
            var builder = new IndentedStringBuilder()
                .AppendLine("using System;")
                .AppendLine("using System.Runtime.InteropServices;")
                .AppendLine()
                .AppendLine("namespace Core.Clang")
                .Append("{");
            var visitor = new ClangCursorVisitor(builder.IncreaseIndent());

            string fileName = Path.Combine(includePath, "clang-c", "index.h");
            var args = systemIncludePaths.ToList(path => "-isystem" + path);
            args.Add("-v");
            args.Add("-I" + includePath);
            using (var index = new Index(true, true))
            using (var translationUnit = index.ParseTranslationUnit(fileName, args))
            {
                visitor.VisitChildren(translationUnit.GetCursor());
            }

            builder.AppendLine("}");
            string nativeTypes = builder.ToString();

            builder = new IndentedStringBuilder()
                .AppendLine("using System;")
                .AppendLine("using System.Runtime.InteropServices;")
                .AppendLine()
                .AppendLine("namespace Core.Clang")
                .AppendLine("{");
            var methodBuilder = builder
                .IncreaseIndent()
                .AppendLine("internal static unsafe class NativeMethods")
                .AppendLine("{")
                .IncreaseIndent()
                .AppendLine("private const string dllName = \"libclang\";");
            foreach (var method in visitor.Methods)
            {
                methodBuilder.AppendLine().AppendLine("[DllImport(dllName)]");

                string resultType = method.ReturnType.ToString();
                string methodName = method.Identifier.ToString();
                var parameters = method.ParameterList.Parameters;
                if (parameters.Count == 0)
                {
                    methodBuilder.AppendLine($"public static extern {resultType} {methodName}();");
                }
                else
                {
                    methodBuilder.AppendLine($"public static extern {resultType} {methodName}(");
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        string end = i == parameters.Count - 1 ? ");" : ",";
                        methodBuilder.IncreaseIndent().AppendLine(parameters[i].ToString() + end);
                    }
                }
            }
            builder.IncreaseIndent().AppendLine("}");
            builder.AppendLine("}");
            string nativeMethods = builder.ToString();

            return (nativeTypes: nativeTypes, nativeMethods: nativeMethods);
        }

        public static void Main(string[] args)
        {
            var (nativeTypes, nativeMethods) = ImportNativeTypesAndMethods(
                includePath: @"C:\Program Files\LLVM\include\",
                systemIncludePaths: new[]
                {
                    @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.11.25503\include\",
                    @"C:\Program Files (x86)\Windows Kits\10\Include\10.0.15063.0\ucrt"
                });
            File.WriteAllText(nativeTypesPath, nativeTypes);
            File.WriteAllText(nativeMethodsPath, nativeMethods);
            Console.WriteLine("Yo~");
        }
    }
}
