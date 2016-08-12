using System;
using System.IO;
using Core.Clang;

namespace Playground
{
    public class Program
    {
        static Program()
        {
            var path = Environment.GetEnvironmentVariable(nameof(Path));
            path = string.Join(Path.PathSeparator.ToString(),
                Path.Combine(Native.LibClang.Restore.Program.LLVMDirectory, "bin"),
                path);
            Environment.SetEnvironmentVariable(nameof(Path), path);
        }

        /// <summary>
        /// Generates the source code in Core.Clang/NativeTypes.cs and Core.Clang/NativeMethods.cs.
        /// </summary>
        /// <param name="includePath">Path to LLVM/include/</param>
        /// <param name="nativeMethods">
        /// The generated contents in Core.Clang/NativeMethods.cs.
        /// </param>
        /// <returns>The generated contents in Core.Clang/NativeTypes.cs.</returns>
        private static string ImportNativeTypes(string includePath, out string nativeMethods)
        {
            var builder = new IndentedStringBuilder()
                .AppendLine("using System;")
                .AppendLine("using System.Runtime.InteropServices;")
                .AppendLine()
                .AppendLine("namespace Core.Clang")
                .Append("{");
            var visitor = new ClangCursorVisitor(builder.IncreaseIndent());

            string fileName = Path.Combine(includePath, "clang-c", "index.h");
            var args = new[] { "-v", "-I" + includePath };
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
                var methodName = method.Identifier.ToString();
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
            nativeMethods = builder.ToString();

            return nativeTypes;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Yo~");
        }
    }
}
