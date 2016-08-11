using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Core.Clang.Tests
{
    public sealed class TestMethodAttribute
        : FactAttribute
    { }

    public sealed class DataTestMethodAttribute
        : TheoryAttribute
    { }

    public class DataRowAttribute : DataAttribute
    {
        public object[] Data { get; }

        public DataRowAttribute(params object[] data)
        {
            Data = data ?? Array.Empty<object>();
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return Data;
        }
    }
}
