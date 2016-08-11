using Core.Clang;
using Core.Diagnostics;

namespace Playground
{
    public class IndentedCursorVisitor : CursorVisitor
    {
        private readonly IndentedStringBuilder builder;

        public IndentedCursorVisitor(IndentedStringBuilder builder)
        {
            Requires.NotNull(builder, nameof(builder));

            this.builder = builder;
        }

        protected override ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            builder.AppendLine(cursor.Kind.ToString());
            new IndentedCursorVisitor(builder.IncreaseIndent()).VisitChildren(cursor);
            return ChildVisitResult.Continue;
        }
    }
}
