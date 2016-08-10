using System.Diagnostics;
using System.Linq;
using Core.Clang;
using Core.Diagnostics;

namespace Playground
{
    public class ClangCursorVisitor : CursorVisitor
    {
        private IndentedStringBuilder builder;

        public ClangCursorVisitor(IndentedStringBuilder builder)
        {
            Requires.NotNull(builder, nameof(builder));

            this.builder = builder;
        }

        protected override ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            switch (cursor.Kind)
            {
                case CursorKind.EnumDecl:
                    VisitEnumDeclaration(cursor);
                    break;
                default:
                    break;
            }
            return ChildVisitResult.Continue;
        }

        private void BreakOrFail(string message)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                Debug.Fail(message);
            }
        }

        private void VisitEnumDeclaration(Cursor cursor)
        {
            builder.AppendLine();

            var constants = cursor.GetChildren();
            string typeName = cursor.GetSpelling();
            if (string.IsNullOrEmpty(typeName)) // e.g. typedef enum { } A;
            {
                typeName = cursor.GetTypeInfo().GetSpelling();
            }

            if (constants.Length == 0)
            {
                builder.AppendLine($"internal enum {typeName} {{ }}");
            }
            else
            {
                builder.AppendLine($"internal enum {typeName}");
                builder.AppendLine("{");

                for (int i = 0; i < constants.Length; i++)
                {
                    var builder = this.builder.IncreaseIndent();

                    var constant = constants[i];
                    string constantName = constant.GetSpelling();
                    string comma = i == constants.Length - 1 ? string.Empty : ",";

                    var children = constant.GetChildren();
                    if (children.Length == 0) // e.g. enum A { A1 };
                    {
                        builder.AppendLine(constantName + comma);
                    }
                    else if (children.Length == 1)
                    {
                        var child = children[0];
                        switch (child.Kind)
                        {
                            case CursorKind.IntegerLiteral: // e.g. enum A { A1 = 0 };
                                string literal = child.GetExtent().GetText();
                                builder.AppendLine($"{constantName} = {literal}{comma}");
                                break;
                            case CursorKind.BinaryOperator:
                                var operands = child.GetChildren();
                                if (operands.Length == 2 &&
                                    operands.All(x => x.Kind == CursorKind.DeclRefExpr))
                                {
                                    // e.g. enum A { A1, A2, A3 = A1 | A2 };
                                    builder.AppendLine($"{constantName} =");
                                    string line = $"{operands[0].GetSpelling()} |";
                                    builder.IncreaseIndent().AppendLine(line);
                                    line = $"{operands[1].GetSpelling()}{comma}";
                                    builder.IncreaseIndent().AppendLine(line);
                                }
                                else if (operands.Length == 2)
                                {
                                    // e.g. enum A { A1 = 1 << 0 };
                                    goto case CursorKind.IntegerLiteral;
                                }
                                else
                                {
                                    BreakOrFail(operands.Length.ToString());
                                }
                                break;
                            case CursorKind.DeclRefExpr: // e.g. enum A { A1, A2 = A1 };
                                string referenceName = child.GetSpelling();
                                builder.AppendLine($"{constantName} = {referenceName}{comma}");
                                break;
                            case CursorKind.UnaryOperator: // e.g. enum A { A1 = -1 };
                            case CursorKind.ParenExpr: // e.g. enum A { A1 = (1 << 0) };
                                goto case CursorKind.IntegerLiteral;
                            default:
                                BreakOrFail(child.Kind.ToString());
                                break;
                        }
                    }
                    else
                    {
                        BreakOrFail(children.Length.ToString());
                    }
                }

                builder.AppendLine("}");
            }
        }

        private string VisitEnumCostantDeclaration(Cursor cursor)
        {
            return null;
        }
    }
}
