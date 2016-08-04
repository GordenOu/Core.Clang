using System;

namespace Core.Clang
{
    /// <summary>
    /// Indicates that a method depends on Clang's internal implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UnstableAttribute
        : Attribute
    { }
}
