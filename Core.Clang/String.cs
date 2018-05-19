using System;
using System.Runtime.InteropServices;

namespace Core.Clang
{
    internal sealed unsafe class String : IDisposable
    {
        public CXString Struct { get; }

        public String(CXString cxString)
        {
            Struct = cxString;
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.clang_disposeString(Struct);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~String()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(String).Name);
            }
        }

        public override string ToString()
        {
            ThrowIfDisposed();

            sbyte* cString = NativeMethods.clang_getCString(Struct);
            return Marshal.PtrToStringAnsi(new IntPtr(cString));
        }
    }
}
