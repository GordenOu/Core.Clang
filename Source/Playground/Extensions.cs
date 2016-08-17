using System;
using System.Collections.Generic;
using System.IO;
using Core.Clang;
using Core.Diagnostics;

namespace Playground
{
    internal static class Extensions
    {
        private class NotifyingVisitor : CursorVisitor
        {
            public event EventHandler<Cursor> VisitingCursor;

            protected override ChildVisitResult Visit(Cursor cursor, Cursor parent)
            {
                VisitingCursor?.Invoke(this, cursor);
                return ChildVisitResult.Continue;
            }
        }

        public static Cursor[] GetChildren(this Cursor cursor)
        {
            Requires.NotNull(cursor, nameof(cursor));

            var visitor = new NotifyingVisitor();
            var children = new List<Cursor>();
            visitor.VisitingCursor += (sender, e) => children.Add(e);
            visitor.VisitChildren(cursor);
            return children.ToArray();
        }

        public static string GetText(this SourceRange range)
        {
            Requires.NotNull(range, nameof(range));

            var start = range.GetStart();
            var end = range.GetEnd();
            string text = File.ReadAllText(start.SourceFile.GetName());
            return text.Substring((int)start.Offset, (int)(end.Offset - start.Offset));
        }
    }
}
