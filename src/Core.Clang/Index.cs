using System;

namespace Core.Clang
{
    /// <summary>
    /// General options associated with a <see cref="Index"/>.
    /// </summary>
    [EnumMapping(typeof(CXGlobalOptFlags), Prefix = "CXGlobalOpt_")]
    [Flags]
    public enum GlobalOptions
    {
        /// <summary>
        /// Used to indicate that no special <see cref="Index"/> options are needed.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Used to indicate that threads that libclang creates for indexing purposes should use
        /// background priority.
        /// </summary>
        ThreadBackgroundPriorityForIndexing = 0x1,

        /// <summary>
        /// Used to indicate that threads that libclang creates for editing purposes should use
        /// background priority.
        /// </summary>
        ThreadBackgroundPriorityForEditing = 0x2,

        /// <summary>
        /// Used to indicate that all threads that libclang creates should use background priority.
        /// </summary>
        ThreadBackgroundPriorityForAll =
            ThreadBackgroundPriorityForIndexing | ThreadBackgroundPriorityForEditing
    }

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
        /// <param name="displayDiagnostics"></param>
        public Index(bool excludeDeclarationsFromPCH, bool displayDiagnostics)
        {
            Ptr = NativeMethods.clang_createIndex(
                Convert.ToInt32(excludeDeclarationsFromPCH),
                Convert.ToInt32(displayDiagnostics));
        }

        private bool disposed;

        /// <summary>
        /// Destroy the given index.
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
        /// Destroy the given index.
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
