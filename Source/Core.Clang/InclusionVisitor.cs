using System;
using System.Runtime.InteropServices;
using Core.Diagnostics;
using Core.Linq;

namespace Core.Clang
{
    internal unsafe delegate void CXInclusionVisitor(
        CXFileImpl* included_file,
        CXSourceLocation* inclusion_stack,
        uint include_len,
        CXClientDataImpl* client_data);

    /// <summary>
    /// A class that visits the set of preprocessor inclusions in a translation unit.
    /// </summary>
    public abstract unsafe class InclusionVisitor
    {
        /// <summary>
        /// Method invoked for each file in a translation unit
        /// (used with <see cref="Visit(TranslationUnit)"/>).
        /// </summary>
        /// <param name="includedFile">The file being included.</param>
        /// <param name="inclusionStack">
        /// The inclusion stack. The array is sorted in order of immediate inclusion. For example,
        /// the first element refers to the location that included <paramref name="includedFile"/>.
        /// </param>
        protected abstract void VisitInclusion(
            SourceFile includedFile,
            SourceLocation[] inclusionStack);

        /// <summary>
        /// Visits the set of preprocessor inclusions in a translation unit. The
        /// <see cref="VisitInclusion(SourceFile, SourceLocation[])"/> method is called with the
        /// provided data for every included file. This does not include headers included by the
        /// PCH file (unless one is inspecting the inclusions in the PCH file itself).
        /// </summary>
        /// <param name="translationUnit">The translation unit to visit.</param>
        public void Visit(TranslationUnit translationUnit)
        {
            Requires.NotNull(translationUnit, nameof(translationUnit));
            translationUnit.ThrowIfDisposed();

            CXInclusionVisitor visitor =
                (included_file, inclusion_stack, include_len, client_data) =>
                {
                    var inclusionStack = new SourceLocation[include_len];
                    inclusionStack.SetValues(i => SourceLocation.GetSpellingLocation(
                        inclusion_stack[i],
                        translationUnit));
                    VisitInclusion(new SourceFile(included_file, translationUnit), inclusionStack);
                };
            try
            {
                NativeMethods.clang_getInclusions(
                    translationUnit.Ptr,
                    Marshal.GetFunctionPointerForDelegate(visitor),
                    null);
            }
            finally
            {
                GC.KeepAlive(visitor);
            }
        }
    }
}
