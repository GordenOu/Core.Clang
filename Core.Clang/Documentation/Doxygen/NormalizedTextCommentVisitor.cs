using System;
using System.Diagnostics;
using System.Text;

namespace Core.Clang.Documentation.Doxygen
{
    internal class NormalizedTextCommentVisitor : CommentVisitor
    {
        private StringBuilder builder;

        public NormalizedTextCommentVisitor()
        {
            builder = new StringBuilder();
        }

        public void Reset()
        {
            builder = new StringBuilder();
        }

        public string GetNormalizedText()
        {
            return builder.ToString();
        }

        public override void Visit(TextComment comment)
        {
            if (comment.IsWhiteSpace())
            {
                return;
            }
            else
            {
                builder.Append(comment.GetText());
                base.Visit(comment);
            }
        }

        public override void Visit(InlineCommandComment comment)
        {
            string commandName = comment.GetCommandName();
            string[] arguments = new string[comment.GetNumArgs()];
            for (uint i = 0; i < arguments.Length; i++)
            {
                arguments[i] = comment.GetArgText(i);
            }
            if (arguments.Length == 0)
            {
                builder.Append($@"\{commandName}");
            }
            else
            {
                builder.Append($@"\{commandName} {string.Join(" ", arguments)}");
            }
            base.Visit(comment);
        }

        public override void Visit(HTMLTagComment comment)
        {
            builder.Append(comment.GetAsString());
            base.Visit(comment);
        }

        public override void Visit(ParagraphComment comment)
        {
            if (comment.IsWhiteSpace())
            {
                return;
            }
            else
            {
                base.Visit(comment);
            }
        }

        public override void Visit(BlockCommandComment comment)
        {
            if (comment.GetKind() == CommentKind.BlockCommand)
            {
                string commandName = comment.GetCommandName();
                string[] arguments = new string[comment.GetNumArgs()];
                for (uint i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = comment.GetArgText(i);
                }
                if (arguments.Length == 0)
                {
                    builder.Append($@"\{commandName}");
                }
                else
                {
                    builder.Append($@"\{commandName} {string.Join(" ", arguments)}");
                }
            }
            base.Visit(comment);
        }

        public override void Visit(ParamCommandComment comment)
        {
            string commandName = comment.GetCommandName();
            string paramName = comment.GetParamName();
            if (comment.IsDirectionExplicit())
            {
                var direction = comment.GetDirection();
                switch (direction)
                {
                    case CommentParamPassDirection.In:
                        builder.Append($@"\{commandName} [in] {paramName}");
                        break;
                    case CommentParamPassDirection.Out:
                        builder.Append($@"\{commandName} [out] {paramName}");
                        break;
                    case CommentParamPassDirection.InOut:
                        builder.Append($@"\{commandName} [in, out] {paramName}");
                        break;
                    default:
                        Debug.Fail("Unreachable.");
                        throw new NotImplementedException();
                }
            }
            else
            {
                builder.Append($@"\{commandName} {paramName}");
            }
            base.Visit(comment);
        }

        public override void Visit(TParamCommandComment comment)
        {
            string commandName = comment.GetCommandName();
            string paramName = comment.GetParamName();
            builder.Append($@"\{commandName} {paramName}");
            base.Visit(comment);
        }

        public override void Visit(VerbatimBlockLineComment comment)
        {
            builder.AppendLine(comment.GetText());
            base.Visit(comment);
        }

        public override void Visit(VerbatimBlockComment comment)
        {
            builder.AppendLine(@"\verbatim");
            base.Visit(comment);
            builder.Append(@"\endverbatim");
        }

        public override void Visit(VerbatimLineComment comment)
        {
            string commandName = comment.GetCommandName();
            builder.Append('\\' + commandName + comment.GetText());
            base.Visit(comment);
        }

        public override void Visit(FullComment comment)
        {
            uint count = comment.GetNumChildren();
            for (uint i = 0; i < count; i++)
            {
                var child = comment.GetChild(i);
                if (child is ParagraphComment paragraphComment)
                {
                    if (paragraphComment.IsWhiteSpace())
                    {
                        continue;
                    }
                }
                Visit(child);
                if (i != count - 1)
                {
                    builder.AppendLine();
                    builder.AppendLine();
                }
            }
        }
    }
}
