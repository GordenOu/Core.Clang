using System;

namespace Core.Clang
{
    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    internal sealed class EnumMappingAttribute : Attribute
    {
        public Type Type { get; }

        public string Prefix { get; set; }

        public object[] Excluded { get; set; }

        public EnumMappingAttribute(Type type)
        {
            Type = type;
        }
    }
}
