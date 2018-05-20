using Core.Diagnostics;

namespace Core.Clang.Documentation.Doxygen
{
    /// <summary>
    /// Visitor for <see cref="Comment"/>.
    /// </summary>
    public abstract class CommentVisitor
    {
        /// <summary>
        /// Visit <see cref="Comment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public void Visit(Comment comment)
        {
            Requires.NotNull(comment, nameof(comment));

            comment.Accept(this);
        }

        private void VisitChilren(Comment comment)
        {
            Requires.NotNull(comment, nameof(comment));

            uint count = comment.GetNumChildren();
            for (uint i = 0; i < count; i++)
            {
                var child = comment.GetChild(i);
                Visit(child);
            }
        }

        /// <summary>
        /// Visit <see cref="InlineContentComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(InlineContentComment comment)
        { }

        /// <summary>
        /// Visit <see cref="TextComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(TextComment comment)
        {
            Visit((InlineContentComment)comment);
        }

        /// <summary>
        /// Visit <see cref="InlineCommandComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(InlineCommandComment comment)
        {
            Visit((InlineContentComment)comment);
        }

        /// <summary>
        /// Visit <see cref="HTMLTagComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(HTMLTagComment comment)
        {
            Visit((InlineContentComment)comment);
        }

        /// <summary>
        /// Visit <see cref="HTMLStartTagComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(HTMLStartTagComment comment)
        {
            Visit((HTMLTagComment)comment);
        }

        /// <summary>
        /// Visit <see cref="HTMLEndTagComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(HTMLEndTagComment comment)
        {
            Visit((HTMLTagComment)comment);
        }

        /// <summary>
        /// Visit <see cref="BlockContentComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(BlockContentComment comment)
        {
            switch (comment.GetKind())
            {
                case CommentKind.VerbatimLine:
                    return;
                default:
                    VisitChilren(comment);
                    break;
            }
        }

        /// <summary>
        /// Visit <see cref="ParagraphComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(ParagraphComment comment)
        {
            Visit((BlockContentComment)comment);
        }

        /// <summary>
        /// Visit <see cref="BlockCommandComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(BlockCommandComment comment)
        {
            Visit((BlockContentComment)comment);
        }

        /// <summary>
        /// Visit <see cref="ParamCommandComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(ParamCommandComment comment)
        {
            Visit((BlockCommandComment)comment);
        }

        /// <summary>
        /// Visit <see cref="TParamCommandComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(TParamCommandComment comment)
        {
            Visit((BlockCommandComment)comment);
        }

        /// <summary>
        /// Visit <see cref="VerbatimBlockLineComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(VerbatimBlockLineComment comment)
        { }

        /// <summary>
        /// Visit <see cref="VerbatimBlockComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(VerbatimBlockComment comment)
        {
            Visit((BlockCommandComment)comment);
        }

        /// <summary>
        /// Visit <see cref="VerbatimLineComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(VerbatimLineComment comment)
        { }

        /// <summary>
        /// Visit <see cref="FullComment"/>.
        /// </summary>
        /// <param name="comment">The comment to visit.</param>
        public virtual void Visit(FullComment comment)
        {
            VisitChilren(comment);
        }
    }
}
