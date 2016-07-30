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
            ThreadBackgroundPriorityForIndexing |
            ThreadBackgroundPriorityForEditing
    }
}
