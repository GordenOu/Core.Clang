using System;
using System.Runtime.InteropServices;
using Core.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Provides the contents of a file that has not yet been saved to disk.
    /// </summary>
    /// <remarks>
    /// Each <see cref="UnsavedFile"/> instance provides the name of a file on the system along
    /// with the current contents of that file that have not yet been saved to disk.
    /// </remarks>
    public sealed unsafe class UnsavedFile : IDisposable
    {
        internal CXUnsavedFile Struct { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsavedFile"/> class.
        /// </summary>
        /// <param name="fileName">The file whose contents have not yet been saved.</param>
        /// <param name="contents">A string containing the unsaved contents of this file.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> or <paramref name="contents"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="fileName"/> is empty.
        /// </exception>
        /// <remarks>
        /// This file must already exist in the file system.
        /// </remarks>
        public UnsavedFile(string fileName, string contents)
        {
            Requires.NotNullOrEmpty(fileName, nameof(fileName));
            Requires.NotNull(contents, nameof(contents));

            Struct = new CXUnsavedFile
            {
                Filename = (sbyte*)Marshal.StringToHGlobalAnsi(fileName).ToPointer(),
                Contents = (sbyte*)Marshal.StringToHGlobalAnsi(contents).ToPointer(),
                Length = (uint)contents.Length
            };
        }

        private bool disposed;

        /// <summary>
        /// Disposes the buffer containing the unsaved contents of this file.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                Marshal.FreeHGlobal(new IntPtr(Struct.Filename));
                Marshal.FreeHGlobal(new IntPtr(Struct.Contents));

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the buffer containing the unsaved contents of this file.
        /// </summary>
        ~UnsavedFile()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(UnsavedFile).Name);
            }
        }

        /// <summary>
        /// Gets the name of this file.
        /// </summary>
        /// <returns>The name of this file.</returns>
        public string GetFileName()
        {
            ThrowIfDisposed();

            return Marshal.PtrToStringAnsi(new IntPtr(Struct.Filename));
        }

        /// <summary>
        /// Gets a string containing the unsaved contents of this file.
        /// </summary>
        /// <returns>A string containing the unsaved contents of this file.</returns>
        public string GetContents()
        {
            ThrowIfDisposed();

            return Marshal.PtrToStringAnsi(new IntPtr(Struct.Contents), (int)Struct.Length);
        }
    }
}
