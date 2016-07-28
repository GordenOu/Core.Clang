using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Clang.Tests
{
    [TestClass]
    public class EnumMappingTests
    {
        [TestMethod]
        public void DefinesAllValuesAndMatchesNames()
        {
            foreach (var type in typeof(Index).GetTypeInfo().Assembly.GetTypes())
            {
                var attribute = type.GetTypeInfo().GetCustomAttribute<EnumMappingAttribute>();
                if (attribute == null)
                {
                    continue;
                }
                var names = Enum.GetNames(attribute.Type);
                var values = Enum.GetValues(attribute.Type);
                Assert.IsTrue(names.Length == values.Length);
                foreach (var value in values)
                {
                    Assert.IsTrue(Enum.IsDefined(type, Enum.ToObject(type, value)));
                }
                foreach (var name in Enum.GetNames(type))
                {
                    CollectionAssert.Contains(names, attribute.Prefix + name);
                }
            }
        }
    }
}
