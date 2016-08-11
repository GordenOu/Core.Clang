using System;
using System.Runtime.InteropServices;

namespace Core.Clang
{
    /// <summary>
    /// The memory usage of a category of a <see cref="TranslationUnit"/>.
    /// </summary>
    public unsafe struct TUResourceUsageEntry
    {
        /// <summary>
        /// The memory usage category.
        /// </summary>
        public TUResourceUsageKind Kind { get; }

        /// <summary>
        /// Amount of resources used. The units will depend on the resource kind.
        /// </summary>
        public ulong Amount { get; }

        internal TUResourceUsageEntry(TUResourceUsageKind kind, ulong amount)
        {
            Kind = kind;
            Amount = amount;
        }

        /// <summary>
        /// Gets the human-readable string that represents the name of the memory category.
        /// </summary>
        /// <param name="kind">The memory category.</param>
        /// <returns>
        /// The human-readable string that represents the name of the memory category, or empty if
        /// the parameter is invalid.
        /// </returns>
        public static string GetResourceUsageName(TUResourceUsageKind kind)
        {
            var cString = NativeMethods.clang_getTUResourceUsageName(
                (CXTUResourceUsageKind)kind);
            return Marshal.PtrToStringAnsi(new IntPtr(cString));
        }
    }
}
