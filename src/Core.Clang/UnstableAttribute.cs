using System;
using System.Diagnostics;

namespace Core.Clang
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class UnstableAttribute
        : Attribute
    {
        public string Version { get; }

        public string[] Urls { get; }

        public UnstableAttribute(string version, string[] seealso)
        {
            Debug.Assert(version != null);
            Debug.Assert(seealso != null);

            Version = version;
            Urls = seealso;
        }
    }
}
