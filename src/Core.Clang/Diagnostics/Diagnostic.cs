using System.Diagnostics;

namespace Core.Clang.Diagnostics
{
    /// <summary>
    /// A single diagnostic, containing the diagnostic's severity, location, text, source ranges,
    /// and fix-it hints.
    /// </summary>
    public sealed unsafe class Diagnostic
    {
        internal CXDiagnosticImpl* Ptr { get; }

        internal DiagnosticSet DiagnosticSet { get; }

        internal TranslationUnit TranslationUnit => DiagnosticSet.TranslationUnit;

        internal Diagnostic(CXDiagnosticImpl* ptr, DiagnosticSet diagnosticSet)
        {
            Debug.Assert(ptr != null);
            Debug.Assert(diagnosticSet != null);
            diagnosticSet.ThrowIfDisposed();

            Ptr = ptr;
            DiagnosticSet = diagnosticSet;
        }

        internal void ThrowIfDisposed()
        {
            DiagnosticSet.ThrowIfDisposed();
        }

        private string ToString(uint options)
        {
            var cxString = NativeMethods.clang_formatDiagnostic(Ptr, options);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Formats the diagnostic using display options most similar to the default behavior of
        /// the clang compiler.
        /// </summary>
        /// <returns>A new string containing for formatted diagnostic.</returns>
        public override string ToString()
        {
            uint options = NativeMethods.clang_defaultDiagnosticDisplayOptions();
            return ToString(options);
        }

        /// <summary>
        /// Formats the diagnostic in a manner that is suitable for display.
        /// </summary>
        /// <param name="options">
        /// A set of options that control the diagnostic display, created by combining
        /// <see cref="DiagnosticDisplayOptions"/> values.
        /// </param>
        /// <returns>A new string containing the formatted diagnostic.</returns>
        public string ToString(DiagnosticDisplayOptions options)
        {
            ThrowIfDisposed();

            return ToString((uint)options);
        }

        /// <summary>
        /// Gets the child diagnostics of the <see cref="Diagnostic"/>. 
        /// </summary>
        /// <returns>
        /// The child diagnostics of the <see cref="Diagnostic"/>, or null if there is no child
        /// diagnostic. This <see cref="Diagnostics.DiagnosticSet"/> does not need to be disposed.
        /// </returns>
        public DiagnosticSet GetChildDiagnostics()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_getChildDiagnostics(Ptr);
            return ptr == null ? null : new DiagnosticSet(ptr, TranslationUnit);
        }

        /// <summary>
        /// Gets the severity of the diagnostic.
        /// </summary>
        /// <returns>The severity of the diagnostic.</returns>
        public DiagnosticSeverity GetSeverity()
        {
            ThrowIfDisposed();

            return (DiagnosticSeverity)NativeMethods.clang_getDiagnosticSeverity(Ptr);
        }

        /// <summary>
        /// Gets the source location of the diagnostic.
        /// </summary>
        /// <returns>The source location of the diagnostic.</returns>
        /// <remarks>
        /// This location is where Clang would print the caret ('^') when displaying the diagnostic
        /// on the command line.
        /// </remarks>
        public SourceLocation GetLocation()
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getDiagnosticLocation(Ptr);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets the text of the diagnostic.
        /// </summary>
        /// <returns>
        /// The text of the diagnostic.
        /// </returns>
        public string GetSpelling()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getDiagnosticSpelling(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the name of the command-line option that enabled this diagnostic.
        /// </summary>
        /// <returns>
        /// A string that contains the command-line option used to enable this warning, such as
        /// "-Wconversion" or "-pedantic". 
        /// </returns>
        public string GetEnablingOption()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getDiagnosticOption(Ptr, null);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Gets the name of the command-line option that disables this diagnostic.
        /// </summary>
        /// <returns>
        /// A string that contains the command-line option to disable this warning.
        /// </returns>
        public string GetDisablingOption()
        {
            ThrowIfDisposed();

            CXString disable;
            var cxString = NativeMethods.clang_getDiagnosticOption(Ptr, &disable);
            try
            {
                using (var str = new String(disable))
                {
                    return str.ToString();
                }
            }
            finally
            {
                NativeMethods.clang_disposeString(cxString);
            }
        }

        /// <summary>
        /// Gets the category number for this diagnostic.
        /// </summary>
        /// <returns>
        /// The number of the category that contains this diagnostic, or zero if this diagnostic
        /// is uncategorized.
        /// </returns>
        /// <remarks>
        /// Diagnostics can be categorized into groups along with other, related diagnostics
        /// (e.g., diagnostics under the same warning flag). This routine retrieves the category
        /// number for the diagnostic.
        /// </remarks>
        public uint GetCategory()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getDiagnosticCategory(Ptr);
        }

        /// <summary>
        /// Gets the diagnostic category text for the diagnostic.
        /// </summary>
        /// <returns>The text of the diagnostic category.</returns>
        public string GetCategoryText()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getDiagnosticCategoryText(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Determines the number of source ranges associated with the diagnostic.
        /// </summary>
        /// <returns>
        /// The number of source ranges associated with the diagnostic.
        /// </returns>
        public uint GetNumRanges()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getDiagnosticNumRanges(Ptr);
        }

        /// <summary>
        /// Gets a source range associated with the diagnostic.
        /// </summary>
        /// <param name="index">The zero-based index specifying which range to extract.</param>
        /// <returns>
        /// The requested source range, or null if <paramref name="index"/> is invalid.
        /// </returns>
        /// <remarks>
        /// A diagnostic's source ranges highlight important elements in the source code. On the
        /// command line, Clang displays source ranges by underlining them with '~' characters.
        /// </remarks>
        public SourceRange GetRange(uint index)
        {
            ThrowIfDisposed();

            var cxSourceRange = NativeMethods.clang_getDiagnosticRange(Ptr, index);
            return SourceRange.Create(cxSourceRange, TranslationUnit);
        }

        /// <summary>
        /// Determine the number of fix-it hints associated with the diagnostic.
        /// </summary>
        /// <returns>The number of fix-it hints associated with the diagnostic.</returns>
        public uint GetNumFixIts()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getDiagnosticNumFixIts(Ptr);
        }

        /// <summary>
        /// Gets the replacement information for a given fix-it.
        /// </summary>
        /// <param name="index">The zero-based index of the fix-it.</param>
        /// <param name="replacementRange">
        /// The source range whose contents will be replaced with the returned replacement string.
        /// Note that source ranges are half-open ranges [a, b), so the source code should be
        /// replaced from a and up to (but not including) b.
        /// </param>
        /// <returns>
        /// A string containing text that should be replace the source code indicated by the
        /// <paramref name="replacementRange"/>.
        /// </returns>
        /// <remarks>
        /// Fix-its are described in terms of a source range whose contents should be replaced by a
        /// string. This approach generalizes over three kinds of operations: removal of source
        /// code (the range covers the code to be removed and the replacement string is empty),
        /// replacement of source code (the range covers the code to be replaced and the
        /// replacement string provides the new code), and insertion (both the start and end of the
        /// range point at the insertion location, and the replacement string provides the text to
        /// insert).
        /// </remarks>
        public string GetFixIt(uint index, out SourceRange replacementRange)
        {
            ThrowIfDisposed();

            CXSourceRange cxSourceRange;
            var cxString = NativeMethods.clang_getDiagnosticFixIt(
                Ptr,
                index,
                &cxSourceRange);
            replacementRange = SourceRange.Create(cxSourceRange, TranslationUnit);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }
    }
}
