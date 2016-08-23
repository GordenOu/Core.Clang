using System;
using System.Runtime.InteropServices;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A class that will receive pairs of Cursor/SourceRange for each reference/directive found.
    /// </summary>
    public abstract unsafe class CursorAndRangeVisitor
    {
        private delegate CXVisitorResult visit(void* context, CXCursor cursor, CXSourceRange range);

        /// <summary>
        /// Visitor callback that will receive pairs of Cursor/SourceRange for each 
        /// reference/directive found.
        /// </summary>
        /// <param name="cursor">The Cursor for a reference/directive.</param>
        /// <param name="range">The SourceRange for each reference/directive.</param>
        /// <returns>true to continue the cursor traversal.</returns>
        protected abstract bool Visit(Cursor cursor, SourceRange range);

        /// <summary>
        /// Find references of a declaration in a specific file.
        /// </summary>
        /// <param name="cursor">Cursor pointing to a declaration or a reference of one.</param>
        /// <param name="file">File to search for references.</param>
        /// <returns>
        /// true if the function was terminated by a callback
        /// (e.g., <see cref="Visit(Cursor, SourceRange)"/> returned false).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cursor"/> or <paramref name="file"/> is null.
        /// </exception>
        public bool FindReferencesInFile(Cursor cursor, SourceFile file)
        {
            Requires.NotNull(cursor, nameof(cursor));
            Requires.NotNull(file, nameof(file));
            cursor.ThrowIfDisposed();

            var result = NativeMethods.clang_findReferencesInFile(
                cursor.Struct,
                file.Ptr,
                new CXCursorAndRangeVisitor
                {
                    visit = Marshal.GetFunctionPointerForDelegate<visit>(
                        (context, arg2, arg3) =>
                        {
                            if (Visit(
                                Cursor.Create(arg2, cursor.TranslationUnit),
                                SourceRange.Create(arg3, cursor.TranslationUnit)))
                            {
                                return CXVisitorResult.CXVisit_Continue;
                            }
                            else
                            {
                                return CXVisitorResult.CXVisit_Break;
                            }
                        })
                });

            switch (result)
            {
                case CXResult.CXResult_Success:
                    return false;
                case CXResult.CXResult_Invalid:
                    throw new InvalidOperationException(result.ToString());
                case CXResult.CXResult_VisitBreak:
                    return true;
                default:
                    goto case CXResult.CXResult_Invalid;
            }
        }

        /// <summary>
        /// Find #import/#include directives in a specific file.
        /// </summary>
        /// <param name="translationUnit">Translation unit containing the file to query.</param>
        /// <param name="file">file to search for #import/#include directives.</param>
        /// <returns>
        /// true if the function was terminated by a callback
        /// (e.g., <see cref="Visit(Cursor, SourceRange)"/> returned false).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="translationUnit"/> or <paramref name="file"/> is null.
        /// </exception>
        public bool FindIncludesInFile(TranslationUnit translationUnit, SourceFile file)
        {
            Requires.NotNull(translationUnit, nameof(translationUnit));
            Requires.NotNull(file, nameof(file));
            translationUnit.ThrowIfDisposed();

            var result = NativeMethods.clang_findIncludesInFile(
                translationUnit.Ptr,
                file.Ptr,
                new CXCursorAndRangeVisitor
                {
                    visit = Marshal.GetFunctionPointerForDelegate<visit>(
                        (context, arg2, arg3) =>
                        {
                            if (Visit(
                                Cursor.Create(arg2, translationUnit),
                                SourceRange.Create(arg3, translationUnit)))
                            {
                                return CXVisitorResult.CXVisit_Continue;
                            }
                            else
                            {
                                return CXVisitorResult.CXVisit_Break;
                            }
                        })
                });

            switch (result)
            {
                case CXResult.CXResult_Success:
                    return false;
                case CXResult.CXResult_Invalid:
                    throw new InvalidOperationException(result.ToString());
                case CXResult.CXResult_VisitBreak:
                    return true;
                default:
                    goto case CXResult.CXResult_Invalid;
            }
        }
    }
}
