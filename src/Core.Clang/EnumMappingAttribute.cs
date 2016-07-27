using System;

namespace Core.Clang
{
    [AttributeUsage(AttributeTargets.Enum)]
    internal sealed class EnumMappingAttribute : Attribute
    {
        public Type Type { get; }

        public string Prefix { get; set; }

        public EnumMappingAttribute(Type type)
        {
            Type = type;
        }
    }
}
