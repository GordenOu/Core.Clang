using System.Runtime.InteropServices;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A class that visits the fields of a particular type.
    /// </summary>
    public abstract unsafe class FieldVisitor
    {
        private delegate CXVisitorResult CXFieldVisitor(CXCursor C, CXClientDataImpl client_data);

        /// <summary>
        /// Method invoked for each field found by a traversal.
        /// </summary>
        /// <param name="cursor">The cursor being visited.</param>
        /// <returns>true to continue the cursor traversal.</returns>
        protected abstract bool VisitField(Cursor cursor);

        /// <summary>
        /// Visits the fields of a particular type.
        /// </summary>
        /// <param name="typeInfo">The record type whose field may be visited.</param>
        /// <returns>
        /// true if the traversal was terminated prematurely by <see cref="VisitField(Cursor)"/>
        /// returning false.
        /// </returns>
        public bool VisitFields(TypeInfo typeInfo)
        {
            Requires.NotNull(typeInfo, nameof(typeInfo));

            return NativeMethods.clang_Type_visitFields(
                typeInfo.Struct,
                Marshal.GetFunctionPointerForDelegate<CXFieldVisitor>(
                    (C, client_data) =>
                    {
                        if (VisitField(Cursor.Create(C, typeInfo.TranslationUnit)))
                        {
                            return CXVisitorResult.CXVisit_Continue;
                        }
                        else
                        {
                            return CXVisitorResult.CXVisit_Break;
                        }
                    }),
                null) != 0;
        }
    }
}
