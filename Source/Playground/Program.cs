using System;
using System.IO;
using Core.Clang;

namespace Playground
{
    public class Program
    {
        /// <summary>
        /// Generates the source code in Core.Clang/NativeTypes.cs.
        /// </summary>
        /// <param name="includePath">Path to LLVM/include/</param>
        /// <returns>The generated contents in Core.Clang/NativeTypes.cs.</returns>
        private static string ImportNativeTypes(string includePath)
        {
            string fileName = Path.Combine(includePath, "clang-c", "index.h");
            var args = new[] { "-v", "-I" + includePath };
            using (var index = new Index(true, true))
            using (var translationUnit = index.ParseTranslationUnit(fileName, args))
            {
                var builder = new IndentedStringBuilder()
                    .AppendLine("using System;")
                    .AppendLine("using System.Runtime.InteropServices;")
                    .AppendLine()
                    .AppendLine("namespace Core.Clang")
                    .Append("{");

                var visitor = new ClangCursorVisitor(builder.IncreaseIndent());
                visitor.VisitChildren(translationUnit.GetCursor());

                builder.AppendLine("}").AppendLine();
                return builder.ToString();
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Yo~");
        }
    }
}
