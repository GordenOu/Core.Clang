using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Core.Diagnostics;
using Core.Linq;

namespace Core.Clang
{
    /// <summary>
    /// Provides the contents of a file that has not yet been saved to disk.
    /// </summary>
    /// <remarks>
    /// Each <see cref="UnsavedFile"/> instance provides the name of a file on the system along
    /// with the current contents of that file that have not yet been saved to disk.
    /// </remarks>
    public class UnsavedFile
    {
        /// <summary>
        /// Gets the name of this file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets a string containing the unsaved contents of this file.
        /// </summary>
        public string Contents { get; }

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

            FileName = fileName;
            Contents = contents;
        }
    }

    internal unsafe class CXUnsavedFiles : IDisposable, IReadOnlyList<CXUnsavedFile>
    {
        internal CXUnsavedFile[] Structs { get; }

        public int Count => Structs.Length;

        public CXUnsavedFile this[int index] => Structs[index];

        public CXUnsavedFiles(IEnumerable<UnsavedFile> unsavedFiles)
        {
            unsavedFiles = unsavedFiles ?? Array.Empty<UnsavedFile>();

            Structs = unsavedFiles
                ?.Where(file => file != null)
                .ToArray(file => new CXUnsavedFile
                {
                    Filename = (sbyte*)Marshal.StringToHGlobalAnsi(file.FileName).ToPointer(),
                    Contents = (sbyte*)Marshal.StringToHGlobalAnsi(file.Contents).ToPointer(),
                    Length = (uint)file.Contents.Length
                }) ?? Array.Empty<CXUnsavedFile>();
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                foreach (var file in Structs)
                {
                    Marshal.FreeHGlobal(new IntPtr(file.Filename));
                    Marshal.FreeHGlobal(new IntPtr(file.Contents));
                }

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~CXUnsavedFiles()
        {
            Dispose();
        }

        public IEnumerator<CXUnsavedFile> GetEnumerator()
        {
            return ((IReadOnlyCollection<CXUnsavedFile>)Structs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyCollection<CXUnsavedFile>)Structs).GetEnumerator();
        }
    }
}
