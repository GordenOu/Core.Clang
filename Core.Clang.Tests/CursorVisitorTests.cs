using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public class DelegatingCursorVisitor : CursorVisitor
    {
        private readonly Func<Cursor, Cursor, ChildVisitResult> visitor;

        public DelegatingCursorVisitor(Func<Cursor, Cursor, ChildVisitResult> visitor)
        {
            Assert.IsNotNull(visitor);

            this.visitor = visitor;
        }

        protected override ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            return visitor(cursor, parent);
        }
    }

    [TestClass]
    public class CursorVisitorTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public CursorVisitorTests()
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

        [TestMethod]
        public void VisitFunctionDeclaration()
        {
            string source = "void foo();";

            string spelling = null;
            var visitor = new DelegatingCursorVisitor((cursor, parent) =>
            {
                if (cursor.Kind == CursorKind.FunctionDecl)
                {
                    spelling = cursor.GetSpelling();
                }

                return ChildVisitResult.Recurse;
            });

            using (var empty = disposables.WriteToEmpty(source))
            {
                Assert.AreEqual(false, visitor.VisitChildren(empty.GetCursor()));
                Assert.AreEqual("foo", spelling);
            }
        }

        [TestMethod]
        public void TerminateTraversalPrematurely()
        {
            string source = "class A { class B { class C { }; }; class D { }; class E { }; };";

            var spellings = new List<string>();
            var visitor = new DelegatingCursorVisitor((cursor, parent) =>
            {
                if (cursor.Kind != CursorKind.ClassDecl)
                {
                    return ChildVisitResult.Recurse;
                }

                string spelling = cursor.GetSpelling();
                spellings.Add(spelling);

                if (spelling == "B")
                {
                    return ChildVisitResult.Continue;
                }
                else if (spelling == "D")
                {
                    return ChildVisitResult.Break;
                }
                else
                {
                    return ChildVisitResult.Recurse;
                }
            });

            using (var empty = disposables.WriteToEmpty(source))
            {
                Assert.AreEqual(true, visitor.VisitChildren(empty.GetCursor()));
                CollectionAssert.AreEqual(new[] { "A", "B", "D" }, spellings);
            }
        }
    }
}
