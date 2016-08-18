using System.Diagnostics;
using Core.Diagnostics;

namespace Core.Clang.Diagnostics
{
    /// <summary>
    /// A group of <see cref="Diagnostic"/>s.
    /// </summary>
    public sealed unsafe class DiagnosticSet
    {
        internal CXDiagnosticSetImpl* Ptr { get; }

        internal TranslationUnit TranslationUnit { get; }

        internal DiagnosticSet(CXDiagnosticSetImpl* ptr, TranslationUnit translationUnit)
        {
            Debug.Assert(ptr != null);
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            Ptr = ptr;
            TranslationUnit = translationUnit;
        }

        internal void ThrowIfDisposed()
        {
            TranslationUnit.ThrowIfDisposed();
        }

        /// <summary>
        /// Retrieves the complete set of diagnostics associated with a translation unit.
        /// </summary>
        /// <param name="translationUnit">The translation unit to query.</param>
        /// <returns>
        /// The complete set of diagnostics associated with the translation unit.
        /// </returns>
        public static DiagnosticSet FromTranslationUnit(TranslationUnit translationUnit)
        {
            Requires.NotNull(translationUnit, nameof(translationUnit));
            translationUnit.ThrowIfDisposed();

            var ptr = NativeMethods.clang_getDiagnosticSetFromTU(translationUnit.Ptr);
            return new DiagnosticSet(ptr, translationUnit);
        }

        /// <summary>
        /// Gets the number of diagnostics in the <see cref="DiagnosticSet"/>.
        /// </summary>
        /// <returns>The number of diagnostics in the <see cref="DiagnosticSet"/>.</returns>
        public uint GetNumDiagnostics()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_getNumDiagnosticsInSet(Ptr);
        }

        /// <summary>
        /// Gets a diagnostic associated with the <see cref="DiagnosticSet"/>.
        /// </summary>
        /// <param name="index">The zero-based diagnostic number to retrieve.</param>
        /// <returns>
        /// The requested diagnostic, or null if <paramref name="index"/> is invalid.
        /// </returns>
        public Diagnostic GetDiagnostic(uint index)
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_getDiagnosticInSet(Ptr, index);
            return ptr == null ? null : new Diagnostic(ptr, TranslationUnit);
        }
    }
}
