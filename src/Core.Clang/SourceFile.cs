using System;
using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A particular source file that is part of a translation unit.
    /// </summary>
    public sealed unsafe class SourceFile : IEquatable<SourceFile>
    {
        internal CXFileImpl* Ptr { get; }

        /// <summary>
        /// The <see cref="TranslationUnit"/> associated with the <see cref="SourceFile"/>.
        /// </summary>
        public TranslationUnit TranslationUnit { get; }

        internal SourceFile(CXFileImpl* ptr, TranslationUnit translationUnit)
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
        /// Determines whether the current instance is equal to another <see cref="SourceFile"/>
        /// object.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SourceFile"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// true if the current instance and the <paramref name="other"/> parameter represents the
        /// same file; otherwise, false.
        /// </returns>
        public bool Equals(SourceFile other)
        {
            ThrowIfDisposed();

            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            other.ThrowIfDisposed();

            return NativeMethods.clang_File_isEqual(Ptr, other.Ptr) != 0;
        }

        /// <summary>
        /// Determines whether the current instance is equal to another object, which must be of
        /// type <see cref="SourceFile"/> and represents the same file.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>
        /// true if the current instance and the <paramref name="obj"/> parameter represents the
        /// same file; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            ThrowIfDisposed();

            var other = obj as SourceFile;
            return other == null ? false : Equals(other);
        }

        private CXFileUniqueID? id;

        /// <summary>
        /// Gets the hash code for this <see cref="SourceFile"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="SourceFile"/>.</returns>
        /// <remarks>
        /// See:
        /// <para>
        /// https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CIndex.cpp
        /// </para>
        /// <para>
        /// https://github.com/llvm-mirror/llvm/blob/master/include/llvm/Support/FileSystem.h
        /// </para>
        /// </remarks>
        [Unstable]
        public override int GetHashCode()
        {
            ThrowIfDisposed();

            if (id == null)
            {
                CXFileUniqueID cxFileUniqueID;
                if (NativeMethods.clang_getFileUniqueID(Ptr, &cxFileUniqueID) != 0)
                {
                    throw new InvalidOperationException();
                }
                id = cxFileUniqueID;
            }

            var value = id.Value;
            return value.data[0].GetHashCode() ^ value.data[1].GetHashCode();
        }

        /// <summary>
        /// Gets the complete file and path name of the given file.
        /// </summary>
        /// <returns>The complete file and path name.</returns>
        public string GetName()
        {
            ThrowIfDisposed();

            var cxString = NativeMethods.clang_getFileName(Ptr);
            using (var str = new String(cxString))
            {
                return str.ToString();
            }
        }

        /// <summary>
        /// Determines whether the header is guarded against multiple inclusions, either with the
        /// conventional #ifndef/#define/#endif macro guards or with #pragma once.
        /// </summary>
        /// <returns>
        /// true if the header is guarded against multiple inclusions; otherwise false.
        /// </returns>
        public bool IsMultipleIncludeGuarded()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isFileMultipleIncludeGuarded(TranslationUnit.Ptr, Ptr) != 0;
        }
    }
}
