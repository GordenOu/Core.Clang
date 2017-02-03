using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// A particular source file that is part of a translation unit.
    /// </summary>
    public sealed unsafe class SourceFile : IEquatable<SourceFile>
    {
        internal CXFileImpl* Ptr { get; }

        internal TranslationUnit TranslationUnit { get; }

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
        /// same file.
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
        /// same file.
        /// </returns>
        public override bool Equals(object obj)
        {
            ThrowIfDisposed();

            return Equals(obj as SourceFile);
        }

        private CXFileUniqueID? id;

        /// <summary>
        /// Gets the hash code for this <see cref="SourceFile"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="SourceFile"/>.</returns>
        [Unstable(version: "3.9.1", seealso: new[]
        {
            "https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CIndex.cpp",
            "https://github.com/llvm-mirror/llvm/blob/master/include/llvm/Support/FileSystem.h"
        })]
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
        /// Gets the complete file and path name of the file.
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
        /// true if the header is guarded against multiple inclusions.
        /// </returns>
        public bool IsMultipleIncludeGuarded()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_isFileMultipleIncludeGuarded(TranslationUnit.Ptr, Ptr) != 0;
        }

        /// <summary>
        /// Gets the source location associated with the given line/column.
        /// </summary>
        /// <param name="line">The line to which the source location points.</param>
        /// <param name="column">The column to which the source location points.</param>
        /// <returns>
        /// The source location associated with the given line/column, or null if the parameters
        /// are invalid.
        /// </returns>
        public SourceLocation GetLocation(uint line, uint column)
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getLocation(
                TranslationUnit.Ptr,
                Ptr,
                line,
                column);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets the source location associated with a given character offset.
        /// </summary>
        /// <param name="offset">
        /// The offset into the buffer to which the source location points.
        /// </param>
        /// <returns>
        /// The source location associated with a given character offset, or null if
        /// <paramref name="offset"/> is invalid.
        /// </returns>
        public SourceLocation GetLocationFromOffset(uint offset)
        {
            ThrowIfDisposed();

            var cxSourceLocation = NativeMethods.clang_getLocationForOffset(
                TranslationUnit.Ptr,
                Ptr,
                offset);
            return SourceLocation.GetSpellingLocation(cxSourceLocation, TranslationUnit);
        }

        /// <summary>
        /// Gets the module that contains the header file, if one exists.
        /// </summary>
        /// <returns>The module that contains the header file, if one exists.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Module GetModule()
        {
            ThrowIfDisposed();

            var ptr = NativeMethods.clang_getModuleForFile(TranslationUnit.Ptr, Ptr);
            return ptr == null ? null : new Module(ptr, TranslationUnit);
        }
    }
}
