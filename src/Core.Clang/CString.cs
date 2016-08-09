using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Core.Linq;

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

    internal class CStrings : IDisposable, IReadOnlyList<CString>
    {
        public CString[] Structs { get; }

        public int Count => Structs.Length;

        public CString this[int index] => Structs[index];

        public CStrings(IEnumerable<string> strings)
        {
            Structs = strings?.ToArray(str => new CString(str)) ?? Array.Empty<CString>();
        }

        public void Dispose()
        {
            Structs.DisposeMany();
        }

        public IEnumerator<CString> GetEnumerator()
        {
            return ((IReadOnlyCollection<CString>)Structs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyCollection<CString>)Structs).GetEnumerator();
        }
    }
}
