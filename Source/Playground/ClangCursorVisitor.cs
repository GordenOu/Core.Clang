using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Clang;
using Core.Diagnostics;
using Core.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Playground
{
    internal class ClangCursorVisitor : CursorVisitor
    {
        private readonly IndentedStringBuilder builder;

        private readonly Dictionary<string, string> knownTypes;

        private readonly IReadOnlyList<UsingDirectiveSyntax> usings;

        public ImmutableList<MethodDeclarationSyntax> Methods { get; private set; }

        public ClangCursorVisitor(IndentedStringBuilder builder)
        {
            Requires.NotNull(builder, nameof(builder));

            this.builder = builder;
            knownTypes = new Dictionary<string, string>();
            var syntaxTree = CSharpSyntaxTree.ParseText(builder.ToString());
            usings = syntaxTree.GetCompilationUnitRoot().Usings;
            Methods = ImmutableList<MethodDeclarationSyntax>.Empty;
        }

        protected override ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            if (!cursor.GetLocation().IsInSystemHeader())
            {
                switch (cursor.Kind)
                {
                    case CursorKind.EnumDecl:
                        VisitEnumDeclaration(cursor);
                        break;
                    case CursorKind.StructDecl:
                        VisitStructDeclaration(cursor);
                        break;
                    case CursorKind.TypedefDecl:
                        VisitTypedefDeclaration(cursor);
                        break;
                    case CursorKind.FunctionDecl:
                        VisitFunctionDeclaration(cursor);
                        break;
                    default:
                        break;
                }
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

        private string GetTypeName(TypeInfo info, out string suffix)
        {
            suffix = string.Empty; // For fixed size buffers.
            string typeName;
            switch (info.Kind)
            {
                case TypeKind.Unexposed:
                    string spelling = info.GetTypeDeclaration().GetSpelling();
                    if (string.IsNullOrEmpty(spelling)) // e.g., typedef struct { } A;
                    {
                        // Clear const qualifiers.
                        spelling = info.GetTypeDeclaration().GetTypeInfo().GetSpelling();
                    }
                    if (knownTypes.TryGetValue(spelling, out typeName))
                    {
                        return typeName;
                    }
                    else
                    {
                        BreakOrFail(spelling);
                        return spelling;
                    }
                case TypeKind.Void:
                    return "void";
                case TypeKind.Bool:
                    return "bool";
                case TypeKind.Char_U:
                case TypeKind.UChar:
                    return "byte";
                case TypeKind.Char16:
                    return "char";
                case TypeKind.UShort:
                    return "ushort";
                case TypeKind.UInt:
                    return "uint";
                case TypeKind.ULong: // Not sure.
                case TypeKind.ULongLong:
                    return "ulong";
                case TypeKind.Char_S:
                case TypeKind.SChar:
                    return "sbyte";
                case TypeKind.Short:
                    return "short";
                case TypeKind.Int:
                    return "int";
                case TypeKind.Long: // Not sure.
                case TypeKind.LongLong:
                    return "long";
                case TypeKind.Float:
                    return "float";
                case TypeKind.Double:
                    return "double";
                case TypeKind.Pointer:
                    var pointeeType = info.GetPointeeType();
                    if (pointeeType.GetResultType().Kind != TypeKind.Invalid)
                    {
                        // Function pointer.
                        if (usings.Any(syntax => syntax.Name.ToString() == nameof(System)))
                        {
                            return nameof(IntPtr);
                        }
                        else
                        {
                            return typeof(IntPtr).FullName;
                        }
                    }
                    else
                    {
                        return GetTypeName(pointeeType, out suffix) + '*';
                    }
                case TypeKind.Record:
                case TypeKind.Enum:
                case TypeKind.Elaborated:
                    goto case TypeKind.Unexposed;
                case TypeKind.Typedef:
                    if (knownTypes.TryGetValue(info.GetSpelling(), out typeName))
                    {
                        // e.g., typedef void* A;
                        return typeName;
                    }
                    else
                    {
                        return GetTypeName(info.GetCanonicalType(), out suffix);
                    }
                case TypeKind.ConstantArray:
                    suffix = $"[{info.GetArraySize()}]";
                    var elementType = info.GetArrayElementType();
                    if (elementType.Kind == TypeKind.Pointer &&
                        elementType.GetPointeeType().Kind == TypeKind.Void)
                    {
                        // e.g., struct A { void* a[3]; };
                        return "fixed ulong";
                    }
                    else
                    {
                        // e.g., struct A { int a[3]; };
                        string temp;
                        return "fixed " + GetTypeName(elementType, out temp);
                    }
                case TypeKind.Auto:
                    return GetTypeName(info.GetCanonicalType(), out suffix);
                default:
                    BreakOrFail(info.Kind.ToString());
                    return info.GetSpelling();
            }
        }

        private void VisitEnumDeclaration(Cursor cursor)
        {
            builder.AppendLine();

            string typeName = cursor.GetSpelling();
            if (string.IsNullOrEmpty(typeName)) // e.g., typedef enum { } A;
            {
                typeName = cursor.GetTypeInfo().GetSpelling();
            }
            knownTypes.Add(typeName, typeName);

            var constants = cursor.GetChildren();
            Debug.Assert(constants.All(constant => constant.Kind == CursorKind.EnumConstantDecl));

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
                    if (children.Length == 0) // e.g., enum A { A1 };
                    {
                        builder.AppendLine(constantName + comma);
                    }
                    else if (children.Length == 1)
                    {
                        var child = children[0];
                        switch (child.Kind)
                        {
                            case CursorKind.IntegerLiteral: // e.g., enum A { A1 = 0 };
                                string literal = child.GetExtent().GetText();
                                builder.AppendLine($"{constantName} = {literal}{comma}");
                                break;
                            case CursorKind.BinaryOperator:
                                var operands = child.GetChildren();
                                if (operands.Length == 2 &&
                                    operands.All(x => x.Kind == CursorKind.DeclRefExpr))
                                {
                                    // e.g., enum A { A1, A2, A3 = A1 | A2 };
                                    builder.AppendLine($"{constantName} =");
                                    string line = $"{operands[0].GetSpelling()} |";
                                    builder.IncreaseIndent().AppendLine(line);
                                    line = $"{operands[1].GetSpelling()}{comma}";
                                    builder.IncreaseIndent().AppendLine(line);
                                }
                                else if (
                                    operands.Length == 2 &&
                                    operands.All(x => x.Kind == CursorKind.IntegerLiteral))
                                {
                                    // e.g., enum A { A1 = 1 << 0 };
                                    goto case CursorKind.IntegerLiteral;
                                }
                                else
                                {
                                    BreakOrFail(operands.Length.ToString());
                                }
                                break;
                            case CursorKind.DeclRefExpr: // e.g., enum A { A1, A2 = A1 };
                                string referenceName = child.GetSpelling();
                                builder.AppendLine($"{constantName} = {referenceName}{comma}");
                                break;
                            case CursorKind.UnaryOperator: // e.g., enum A { A1 = -1 };
                            case CursorKind.ParenExpr: // e.g., enum A { A1 = (1 << 0) };
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

        private void VisitStructDeclaration(Cursor cursor)
        {
            builder.AppendLine();

            string typeName = cursor.GetSpelling();
            if (string.IsNullOrEmpty(typeName)) // e.g., typedef struct { } A;
            {
                typeName = cursor.GetTypeInfo().GetSpelling();
            }
            knownTypes.Add(typeName, typeName);

            var fields = cursor.GetChildren();
            Debug.Assert(fields.All(field => field.Kind == CursorKind.FieldDecl));

            if (usings.Any(syntax => syntax.Name.ToString() == typeof(LayoutKind).Namespace))
            {
                builder.AppendLine("[StructLayout(LayoutKind.Sequential)]");
            }
            else
            {
                string attribute = $"{typeof(StructLayoutAttribute).Namespace}.StructLayout";
                string argument = $"{typeof(LayoutKind).FullName}.{nameof(LayoutKind.Sequential)}";
                builder.AppendLine($"[{attribute}({argument})]");
            }

            if (fields.Length == 0)
            {
                builder.AppendLine($"internal struct {typeName} {{ }}");
                return;
            }

            bool isUnsafe = false;
            var fieldDeclarations = new List<string>();
            foreach (var field in fields)
            {
                var fieldType = field.GetTypeInfo();
                var cononicalType = fieldType.GetCanonicalType();
                string fieldName = field.GetSpelling();
                string suffix;
                string fieldTypeName = GetTypeName(fieldType, out suffix);

                if (cononicalType.Kind == TypeKind.Pointer &&
                    cononicalType.GetPointeeType().GetResultType().Kind == TypeKind.Invalid)
                {
                    // Pointers excluding function pointers.
                    isUnsafe = true;
                }
                if (!string.IsNullOrEmpty(suffix))
                {
                    // Fixed size buffers.
                    isUnsafe = true;
                }

                // Escape keywords
                if (SyntaxFacts.GetKeywordKind(fieldName) != SyntaxKind.None)
                {
                    fieldName = '@' + fieldName;
                }

                // e.g., public fixed int a[3];
                fieldDeclarations.Add($"public {fieldTypeName} {fieldName}{suffix};");
            }

            if (isUnsafe)
            {
                builder.AppendLine($"internal unsafe struct {typeName}");
            }
            else
            {
                builder.AppendLine($"internal struct {typeName}");
            }
            builder.AppendLine("{");
            fieldDeclarations.Apply(builder.IncreaseIndent().AppendLine);
            builder.AppendLine("}");
        }

        private void VisitTypedefDeclaration(Cursor cursor)
        {
            var children = cursor.GetChildren();

            if (children.Length == 0)
            {
                builder.AppendLine();

                var underlyingType = cursor.GetTypedefDeclUnderlyingType();
                if (underlyingType.Kind == TypeKind.Pointer)
                {
                    var pointeeType = underlyingType.GetPointeeType();
                    if (pointeeType.Kind == TypeKind.Void) // e.g., typedef void* A;
                    {
                        string spelling = cursor.GetSpelling();
                        string typename = spelling + "Impl";
                        builder.AppendLine($"internal struct {typename} {{ }}");
                        knownTypes.Add(spelling, typename + '*');
                    }
                    else
                    {
                        BreakOrFail(pointeeType.Kind.ToString());
                    }
                }
                else
                {
                    BreakOrFail(underlyingType.Kind.ToString());
                }
            }
            else if (children.Length == 1)
            {
                var child = children[0];

                switch (child.Kind)
                {
                    case CursorKind.StructDecl: // e.g., typedef struct { } A;
                    case CursorKind.EnumDecl: // e.g., typedef enum { } A;
                        break;
                    case CursorKind.TypeRef: // e.g., typedef struct AImpl* A; 
                        string typeName = child.GetCursorReferenced().GetSpelling(); // AImpl
                        string spelling = cursor.GetSpelling(); // A
                        if (cursor.GetTypedefDeclUnderlyingType().Kind == TypeKind.Pointer)
                        {
                            knownTypes.Add(spelling, typeName + '*');
                        }
                        else
                        {
                            BreakOrFail(spelling);
                        }
                        break;
                    default:
                        BreakOrFail(child.Kind.ToString());
                        break;
                }
            }
            else
            {
                var underlyingType = cursor.GetTypedefDeclUnderlyingType();
                if (underlyingType.Kind == TypeKind.Pointer &&
                    underlyingType.GetPointeeType().GetResultType().Kind != TypeKind.Invalid)
                {
                    // e.g., typedef void (*foo)();
                    return;
                }
                else
                {
                    BreakOrFail(underlyingType.Kind.ToString());
                }
            }
        }

        public void VisitFunctionDeclaration(Cursor cursor)
        {
            string functionName = cursor.GetSpelling();
            string suffix;
            string resultTypeName = GetTypeName(cursor.GetResultType(), out suffix);
            var method = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.ParseTypeName(resultTypeName),
                functionName);

            var parameters = new List<string>();
            foreach (var child in cursor.GetChildren())
            {
                if (child.Kind == CursorKind.ParmDecl)
                {
                    var parameterType = child.GetTypeInfo();
                    if (parameterType.Kind == TypeKind.Void) // e.g., void foo(void);
                    {
                        break;
                    }
                    string typeName = GetTypeName(parameterType, out suffix);

                    string parameterName = child.GetSpelling();
                    if (string.IsNullOrEmpty(parameterName))
                    {
                        parameterName = $"arg{parameters.Count + 1}";
                    }
                    // Escape keywords
                    if (SyntaxFacts.GetKeywordKind(parameterName) != SyntaxKind.None)
                    {
                        parameterName = '@' + parameterName;
                    }

                    parameters.Add($"{typeName} {parameterName}");
                }
            }

            if (parameters.Count != 0)
            {
                var parameterList = SyntaxFactory.ParseParameterList(string.Join(",", parameters));
                method = method.WithParameterList(parameterList);
            }

            Methods = Methods.Add(method);
        }
    }
}
