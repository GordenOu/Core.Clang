using System;

namespace Core.Clang
{
    /// <summary>
    /// Indicates that a method depends on clang's internal implementation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnstableAttribute
        : Attribute
    { }
}
