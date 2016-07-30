using System;

namespace Core.Clang
{
    /// <summary>
    /// An "index" that consists of a set of translation units that would typically be linked
    /// together into an executable or library.
    /// </summary>
    public sealed unsafe class Index : IDisposable
    {
        internal CXIndexImpl* Ptr { get; }

        /// <summary>
        /// Provides a shared context for creating translation units.
        /// </summary>
        /// <param name="excludeDeclarationsFromPCH">
        /// When true, allows enumeration of "local" declarations (when loading any new
        /// translation units). A "local" declaration is one that belongs in the translation unit
        /// itself and not in a precompiled header that was used by the translation unit. If false,
        /// all declarations will be enumerated.
        /// </param>
        /// <param name="displayDiagnostics">
        /// true to print diagnostics to standard error.
        /// </param>
        public Index(bool excludeDeclarationsFromPCH, bool displayDiagnostics)
        {
            Ptr = NativeMethods.clang_createIndex(
                Convert.ToInt32(excludeDeclarationsFromPCH),
                Convert.ToInt32(displayDiagnostics));
        }

        private bool disposed;

        /// <summary>
        /// Destroys the index.
        /// </summary>
        /// <remarks>
        /// The index must not be destroyed until all of the translation units created within that
        /// index have been destroyed.
        /// </remarks>
        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.clang_disposeIndex(Ptr);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroys the index.
        /// </summary>
        ~Index()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(Index).Name);
            }
        }

        /// <summary>
        /// Sets general options associated with a <see cref="Index"/>.
        /// </summary>
        /// <param name="options">
        /// A bitmask of options, a bitwise OR of <see cref="GlobalOptions"/>.
        /// </param>
        public void SetGlobalOptions(GlobalOptions options)
        {
            ThrowIfDisposed();

            NativeMethods.clang_CXIndex_setGlobalOptions(Ptr, (uint)options);
        }

        /// <summary>
        /// Gets the general options associated with an <see cref="Index"/>.
        /// </summary>
        /// <returns>
        /// A bitmask of options, a bitwise OR of <see cref="GlobalOptions"/> flags that are
        /// associated with the given <see cref="Index"/> object.
        /// </returns>
        public GlobalOptions GetGlobalOptions()
        {
            ThrowIfDisposed();

            return (GlobalOptions)NativeMethods.clang_CXIndex_getGlobalOptions(Ptr);
        }
    }
}
