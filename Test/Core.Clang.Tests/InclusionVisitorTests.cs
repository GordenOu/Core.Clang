using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    public class DelegatingInclusionVisitor : InclusionVisitor
    {
        private Action<SourceFile, SourceLocation[]> visitor;

        public DelegatingInclusionVisitor(Action<SourceFile, SourceLocation[]> visitor)
        {
            Assert.IsNotNull(visitor);

            this.visitor = visitor;
        }

        protected override void VisitInclusion(
            SourceFile includedFile,
            SourceLocation[] inclusionStack)
        {
            visitor(includedFile, inclusionStack);
        }
    }

    [TestClass]
    public class InclusionVisitorTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public InclusionVisitorTests()
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
        public void GetInclusionStacks()
        {
            var stacks = new Dictionary<SourceFile, SourceLocation[]>();
            var visitor = new DelegatingInclusionVisitor(
                (includedFile, inclusionStack) => stacks.Add(includedFile, inclusionStack));

            var add = disposables.Add;
            visitor.Visit(add);

            SourceLocation[] stack;
            Assert.IsTrue(stacks.TryGetValue(add.GetFile(TestFiles.CommonHeader), out stack));
            Assert.AreEqual(1, stack.Length);
            Assert.IsTrue(stacks.TryGetValue(add.GetFile(TestFiles.AddHeader), out stack));
            Assert.AreEqual(2, stack.Length);
            Assert.IsTrue(stacks.TryGetValue(add.GetFile(TestFiles.MultiplyHeader), out stack));
            Assert.AreEqual(2, stack.Length);
        }
    }
}
