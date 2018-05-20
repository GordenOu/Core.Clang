using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using Core.Clang.Documentation.Doxygen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests.Documentation
{
    [TestClass]
    public class DoxygenTests : ClangTests, IDisposable
    {
        private Disposables disposables;

        public DoxygenTests()
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

        private class GetNormailizedStringVisitor : NormalizedTextCommentVisitor
        {
            public ImmutableHashSet<CommentKind> VisitedCommentKinds { get; private set; }

            public IImmutableList<ParamCommandComment> ParamCommandComments { get; private set; }

            public IImmutableList<TParamCommandComment> TParamCommandComments { get; private set; }

            public GetNormailizedStringVisitor()
            {
                VisitedCommentKinds = ImmutableHashSet<CommentKind>.Empty;
                ParamCommandComments = ImmutableList<ParamCommandComment>.Empty;
                TParamCommandComments = ImmutableList<TParamCommandComment>.Empty;
            }

            public override void Visit(InlineContentComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(HTMLStartTagComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(HTMLEndTagComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(BlockContentComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(TextComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(InlineCommandComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(HTMLTagComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(ParagraphComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(BlockCommandComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(ParamCommandComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                ParamCommandComments = ParamCommandComments.Add(comment);
                base.Visit(comment);
            }

            public override void Visit(TParamCommandComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                TParamCommandComments = TParamCommandComments.Add(comment);
                base.Visit(comment);
            }

            public override void Visit(VerbatimBlockLineComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(VerbatimBlockComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(VerbatimLineComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }

            public override void Visit(FullComment comment)
            {
                VisitedCommentKinds = VisitedCommentKinds.Add(comment.GetKind());
                base.Visit(comment);
            }
        }

        [TestMethod]
        public void GetNormailizedString()
        {
            using (var documentation = disposables.Index.ParseTranslationUnit(
                TestFiles.Doxygen.Documentation,
                null,
                options: TranslationUnitCreationOptions.DetailedPreprocessingRecord))
            {
                var file = documentation.GetFile(TestFiles.Doxygen.Documentation);
                var location = file.GetLocation(24, 1);
                var cursor = documentation.GetCursor(location);
                var comment = Comment.FromCursor(cursor);
                Assert.IsNotNull(comment);
                string normalizedText = comment.GetNormalizedText();
                string text = File.ReadAllText(TestFiles.Doxygen.DoxygenNormalizedDocumentText);
                Assert.AreEqual(text, normalizedText);

                var visitor = new GetNormailizedStringVisitor();
                visitor.Visit(comment);
                normalizedText = visitor.GetNormalizedText();
                Assert.AreEqual(text, normalizedText);
                var visitedCommentKinds = visitor.VisitedCommentKinds;
                Assert.AreEqual(12, visitedCommentKinds.Count);
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.Text));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.InlineCommand));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.HTMLStartTag));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.HTMLEndTag));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.Paragraph));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.BlockCommand));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.ParamCommand));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.TParamCommand));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.VerbatimBlockCommand));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.VerbatimBlockLine));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.VerbatimLine));
                Assert.IsTrue(visitedCommentKinds.Contains(CommentKind.FullComment));
                Assert.AreEqual(3, visitor.ParamCommandComments.Count);
                Assert.AreEqual(0u, visitor.ParamCommandComments[0].GetParamIndex());
                Assert.AreEqual(1u, visitor.ParamCommandComments[1].GetParamIndex());
                Assert.AreEqual(null, visitor.ParamCommandComments[2].GetParamIndex());
                Assert.AreEqual(1, visitor.TParamCommandComments.Count);
            }
        }
    }
}
