using System;
using System.Diagnostics;

namespace Core.Clang
{
    /// <summary>
    /// Identifies a specific source location within a translation unit.
    /// </summary>
    public sealed unsafe class SourceLocation : IEquatable<SourceLocation>
    {
        internal CXSourceLocation Struct { get; }

        /// <summary>
        /// The <see cref="SourceFile"/> associated with the <see cref="SourceLocation"/>.
        /// </summary>
        public SourceFile SourceFile { get; }

        /// <summary>
        /// The line to which the source location points.
        /// </summary>
        public uint Line { get; }

        /// <summary>
        /// The column to which the source location points.
        /// </summary>
        public uint Column { get; }

        /// <summary>
        /// The offset into the buffer to which the source location points.
        /// </summary>
        public uint Offset { get; }

        private static CXSourceLocation? nullLocation;

        private static CXSourceLocation NullLocation
        {
            get
            {
                if (nullLocation == null)
                {
                    nullLocation = NativeMethods.clang_getNullLocation();
                }
                return nullLocation.Value;
            }
        }

        private SourceLocation(
            CXSourceLocation cxSourceLocation,
            SourceFile soureFile,
            uint line,
            uint column,
            uint offset)
        {
            Debug.Assert(soureFile != null);
            soureFile.ThrowIfDisposed();

            Struct = cxSourceLocation;
            SourceFile = soureFile;
            Line = line;
            Column = column;
            Offset = offset;
        }

        internal static SourceLocation GetSpellingLocation(
            CXSourceLocation cxSourceLocation,
            TranslationUnit translationUnit)
        {
            Debug.Assert(translationUnit != null);
            translationUnit.ThrowIfDisposed();

            if (NativeMethods.clang_equalLocations(cxSourceLocation, NullLocation) != 0)
            {
                return null;
            }
            else
            {
                CXFileImpl* sourceFilePtr;
                uint line;
                uint column;
                uint offset;
                NativeMethods.clang_getSpellingLocation(
                    cxSourceLocation,
                    &sourceFilePtr,
                    &line,
                    &column,
                    &offset);
                return new SourceLocation(
                    cxSourceLocation,
                    new SourceFile(sourceFilePtr, translationUnit),
                    line,
                    column,
                    offset);
            }
        }

        internal void ThrowIfDisposed()
        {
            SourceFile.ThrowIfDisposed();
        }

        /// <summary>
        /// Determine whether two source locations, which must refer into the same translation unit,
        /// refer to exactly the same point in the source code.
        /// </summary>
        /// <param name="other">
        /// The <see cref="SourceLocation"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// true if the source locations refer to the same location.
        /// </returns>
        public bool Equals(SourceLocation other)
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

            return NativeMethods.clang_equalLocations(Struct, other.Struct) != 0;
        }

        /// <summary>
        /// Determines whether the current instance is equal to another object, which must be of
        /// type <see cref="SourceLocation"/> and represents the same source location.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>
        /// true if the current instance and the <paramref name="obj"/> parameter refer to the same
        /// source location.
        /// </returns>
        public override bool Equals(object obj)
        {
            ThrowIfDisposed();

            var other = obj as SourceLocation;
            return other == null ? false : Equals(other);
        }

        /// <summary>
        /// Gets the hash code for this <see cref="SourceLocation"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="SourceLocation"/>.</returns>
        /// <remarks>
        /// See:
        /// <para>
        /// https://github.com/llvm-mirror/clang/blob/master/tools/libclang/CXSourceLocation.cpp
        /// </para>
        /// </remarks>
        [Unstable]
        public override int GetHashCode()
        {
            ThrowIfDisposed();

            var cxSourceLocation = Struct;
            return
                cxSourceLocation.ptr_data[0].GetHashCode() ^
                cxSourceLocation.ptr_data[1].GetHashCode() ^
                cxSourceLocation.int_data.GetHashCode();
        }

        /// <summary>
        /// Gets a value that indicates whether the source location is in a system header.
        /// </summary>
        /// <returns>true if the source location is in a system header.</returns>
        public bool IsInSystemHeader()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Location_isInSystemHeader(Struct) != 0;
        }

        /// <summary>
        /// Gets a value that indicates whether the source location is in the main file of the
        /// corresponding translation unit.
        /// </summary>
        /// <returns>
        /// true if the source location is in the main file of the corresponding translation unit.
        /// </returns>
        public bool IsFromMainFile()
        {
            ThrowIfDisposed();

            return NativeMethods.clang_Location_isFromMainFile(Struct) != 0;
        }
    }
}
