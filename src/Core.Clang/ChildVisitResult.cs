namespace Core.Clang
{
    /// <summary>
    /// Describes how the traversal of the children of a particular cursor should proceed after
    /// visiting a particular child cursor.
    /// </summary>
    /// <remarks>
    /// A value of this enumeration type should be returned by each <see cref="CursorVisitor"/> to
    /// indicate how <see cref="CursorVisitor.VisitChildren(Cursor)"/> proceed.
    /// </remarks>
    [EnumMapping(typeof(CXChildVisitResult), Prefix = "CXChildVisit_")]
    public enum ChildVisitResult
    {
        /// <summary>
        /// Terminates the cursor traversal.
        /// </summary>
        Break,

        /// <summary>
        /// Continues the cursor traversal with the next sibling of the cursor just visited,
        /// without visiting its children.
        /// </summary>
        Continue,

        /// <summary>
        /// Recursively traverse the children of this cursor, using the same visitor.
        /// </summary>
        Recurse
    }
}
