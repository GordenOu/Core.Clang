using System;

namespace Core.Clang
{
    /// <summary>
    /// The exception that is thrown when an error occurred in libclang.
    /// </summary>
    public abstract class ClangException : Exception
    {
        internal ClangException(string message)
            : base(message)
        { }
    }
}
