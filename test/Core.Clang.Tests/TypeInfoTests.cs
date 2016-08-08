using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class TypeInfoTests : IDisposable
    {
        private Disposables disposables;

        public TypeInfoTests()
        {
            Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            Monitor.Enter(TestFiles.Locker);
            disposables = new Disposables();
        }

        [TestCleanup]
        public void Dispose()
        {
            disposables.Dispose();
            Monitor.Exit(TestFiles.Locker);
        }


        [DataTestMethod]
        [DataRow(
            "void foo(int a);", 0,
            nameof(TypeInfo.GetArgType), new object[] { 0u },
            TypeKind.Int, "get_Kind")]
        [DataRow(
            "void foo(int a);", 0,
            nameof(TypeInfo.GetArgType), new object[] { 1u },
            TypeKind.Invalid, "get_Kind")]
        [DataRow(
            "template<typename A, typename B> class C { }; C<int, int> a;", 47,
            nameof(TypeInfo.GetTemplateArgumentAsType), new object[] { 0u },
            TypeKind.Int, "get_Kind")]
        public void TestMethods(
            string source,
            int offset,
            string methodName,
            object[] parameters,
            object expected,
            string resultAdapter)
        {
            using (var empty = disposables.WriteToEmpty(source))
            {
                var file = empty.GetFile(TestFiles.Empty);
                var location = file.GetLocationFromOffset((uint)offset);
                var typeInfo = empty.GetCursor(location).GetTypeInfo();
                var method = typeof(TypeInfo).GetTypeInfo().GetMethod(methodName);
                object result = method.Invoke(typeInfo, parameters);
                if (resultAdapter != null)
                {
                    method = result.GetType().GetTypeInfo().GetMethod(resultAdapter);
                    result = method.Invoke(result, null);
                }
                Assert.AreEqual(expected, result);
            }
        }

        [DataTestMethod]
        [DataRow("int a;", 0, nameof(TypeInfo.GetSpelling), "int", null)]
        [DataRow(
            "using Int32 = int; Int32 a;", 19,
            nameof(TypeInfo.GetCanonicalType),
            TypeKind.Int, "get_Kind")]
        [DataRow("const int a = 0;", 0, nameof(TypeInfo.IsConstQualified), true, null)]
        [DataRow("volatile int a = 0;", 0, nameof(TypeInfo.IsVolatileQualified), true, null)]
        [DataRow("int* a;", 0, nameof(TypeInfo.GetPointeeType), TypeKind.Int, "get_Kind")]
        [DataRow(
            "class A { }; A a;", 14,
            nameof(TypeInfo.GetTypeDeclaration),
            CursorKind.ClassDecl, "get_Kind")]
        [DataRow("void foo();", 0, nameof(TypeInfo.GetResultType), TypeKind.Void, "get_Kind")]
        [DataRow("void foo();", 0, nameof(TypeInfo.GetNumArgTypes), 0, null)]
        [DataRow("int a;", 0, nameof(TypeInfo.GetNumArgTypes), -1, null)]
        [DataRow("void foo();", 0, nameof(TypeInfo.IsFunctionTypeVariadic), false, null)]
        [DataRow("void foo(int...);", 0, nameof(TypeInfo.IsFunctionTypeVariadic), true, null)]
        [DataRow("class A { };", 0, nameof(TypeInfo.IsPODType), true, null)]
        [DataRow("int a[1];", 0, nameof(TypeInfo.GetArrayElementType), TypeKind.Int, "get_Kind")]
        [DataRow("int a[1];", 0, nameof(TypeInfo.GetArraySize), 1L, null)]
        [DataRow(
            "class A { }; int A::* b;", 18,
            nameof(TypeInfo.GetClassType),
            "A", nameof(TypeInfo.GetSpelling))]
        [DataRow(
            "template<typename A, typename B> class C { }; C<int, int> a;", 47,
            nameof(TypeInfo.GetNumTemplateArguments),
            2, null)]
        [DataRow(
            "struct A { void foo() &; };", 11,
            nameof(TypeInfo.GetCXXRefQualifier),
            RefQualifierKind.LValue, null)]
        [DataRow(
            "struct A { void foo() &&; };", 11,
            nameof(TypeInfo.GetCXXRefQualifier),
            RefQualifierKind.RValue, null)]
        public void TestMethodsNoParameter(
            string source,
            int offset,
            string methodName,
            object expected,
            string resultAdapter)
        {
            TestMethods(source, offset, methodName, null, expected, resultAdapter);
        }

        [DataTestMethod]
        [DataRow(
            "int a;", 0,
            nameof(TypeInfo.TryGetAlignOf), new object[] { null },
            null, null, 4L)]
        [DataRow(
            "int a;", 0,
            nameof(TypeInfo.TryGetSizeOf), new object[] { null },
            null, null, 4L)]
        [DataRow(
            "struct A { int a; int b; };", 0,
            nameof(TypeInfo.TryGetOffsetOf), new object[] { "a", null },
            null, null, 0L)]
        [DataRow(
            "struct A { int a; int b; };", 0,
            nameof(TypeInfo.TryGetOffsetOf), new object[] { "b", null },
            null, null, 32L)]
        public void TestMethodsWithOneOutParameter(
            string source,
            int offset,
            string methodName,
            object[] parameters,
            object expected,
            string resultAdapter,
            object outParameter)
        {
            TestMethods(source, offset, methodName, parameters, expected, resultAdapter);
            Assert.AreEqual(parameters.Last(), outParameter);
        }
    }
}
