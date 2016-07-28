using System;
using System.Runtime.InteropServices;

namespace Core.Clang
{
    internal sealed unsafe class CString : IDisposable
    {
        internal sbyte* Ptr { get; }

        public CString(string str)
        {
            Ptr = (sbyte*)Marshal.StringToHGlobalAnsi(str).ToPointer();
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                Marshal.FreeHGlobal(new IntPtr(Ptr));

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~CString()
        {
            Dispose();
        }
    }
}
