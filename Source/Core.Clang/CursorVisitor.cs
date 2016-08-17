using System;
using System.Runtime.InteropServices;
using Core.Diagnostics;

namespace Core.Clang
{
    internal unsafe delegate CXChildVisitResult CXCursorVisitor(
        CXCursor cursor,
        CXCursor parent,
        CXClientDataImpl* client_data);

    /// <summary>
    /// A class that does depth-first traversal on the Clang AST and visits each cursor.
    /// </summary>
    public abstract unsafe class CursorVisitor
    {
        /// <summary>
        /// Method invoked for each cursor found by a traversal.
        /// </summary>
        /// <param name="cursor">The cursor being visited.</param>
        /// <param name="parent">The parent cursor for the cursor being visited.</param>
        /// <returns>
        /// One of the <see cref="ChildVisitResult"/> values to direct
        /// <see cref="VisitChildren(Cursor)"/>.
        /// </returns>
        /// <remarks>
        /// This method will be invoked for each cursor found by
        /// <see cref="VisitChildren(Cursor)"/>.
        /// </remarks>
        protected abstract ChildVisitResult Visit(Cursor cursor, Cursor parent);

        /// <summary>
        /// Visits the children of a particular cursor.
        /// </summary>
        /// <param name="cursor">
        /// the cursor whose child may be visited. All kinds of cursors can be visited, including
        /// invalid cursors (which, by definition, have no children).
        /// </param>
        /// <returns>
        /// true if the traversal was terminated prematurely by the visitor returning
        /// <see cref="ChildVisitResult.Break"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cursor"/> is null.
        /// </exception>
        public bool VisitChildren(Cursor cursor)
        {
            Requires.NotNull(cursor, nameof(cursor));
            cursor.ThrowIfDisposed();

            return NativeMethods.clang_visitChildren(
                cursor.Struct,
                Marshal.GetFunctionPointerForDelegate<CXCursorVisitor>(
                    (cxCursor, parent, client_data) => (CXChildVisitResult)Visit(
                        Cursor.Create(cxCursor, cursor.TranslationUnit),
                        Cursor.Create(parent, cursor.TranslationUnit))),
                null) != 0;
        }
    }
}
