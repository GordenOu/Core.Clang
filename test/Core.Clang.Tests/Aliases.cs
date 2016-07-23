using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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

    public static class Assert
    {
        public static void AreEqual(object expected, object actual)
        {
            Xunit.Assert.Equal(expected, actual);
        }

        public static void AreEqual(string expected, string actual, bool ignoreCase)
        {
            Xunit.Assert.Equal(expected, actual, ignoreCase);
        }

        public static void AreEqual(double expected, double actual, int precision)
        {
            Xunit.Assert.Equal(expected, actual, precision);
        }

        public static void AreEqual(float expected, float actual, int precision)
        {
            Xunit.Assert.Equal(expected, actual, precision);
        }

        public static void AreEqual<T>(T expected, T actual)
        {
            Xunit.Assert.Equal(expected, actual);
        }

        public static void AreNotEqual(object notExpected, object actual)
        {
            Xunit.Assert.NotEqual(notExpected, actual);
        }

        public static void AreNotEqual(double notExpected, double actual, int precision)
        {
            Xunit.Assert.NotEqual(notExpected, actual, precision);
        }

        public static void AreNotEqual(float notExpected, float actual, int precision)
        {
            Xunit.Assert.NotEqual(notExpected, actual, precision);
        }

        public static void AreNotEqual<T>(T notExpected, T actual)
        {
            Xunit.Assert.NotEqual(notExpected, actual);
        }

        public static void Fail()
        {
            throw new XunitException();
        }

        public static void Fail(string message)
        {
            throw new XunitException(message);
        }

        public static void IsFalse(bool condition)
        {
            Xunit.Assert.False(condition);
        }

        public static void IsFalse(bool condition, string message)
        {
            Xunit.Assert.False(condition, message);
        }

        public static void IsInstanceOfType(object value, Type expectedType)
        {
            Xunit.Assert.IsType(expectedType, value);
        }

        public static void IsNotInstanceOfType(object value, Type wrongType)
        {
            Xunit.Assert.IsNotType(wrongType, value);
        }

        public static void IsNotNull(object value)
        {
            Xunit.Assert.NotNull(value);
        }

        public static void IsNull(object value)
        {
            Xunit.Assert.Null(value);
        }

        public static void IsTrue(bool condition)
        {
            Xunit.Assert.True(condition);
        }

        public static void IsTrue(bool condition, string message)
        {
            Xunit.Assert.True(condition, message);
        }

        public static T Throws<T>(Action action)
            where T : Exception
        {
            return Xunit.Assert.Throws<T>(action);
        }

        public static T Throws<T>(Func<object> action)
            where T : Exception
        {
            return Xunit.Assert.Throws<T>(action);
        }

        public static Task<T> ThrowsAsync<T>(Func<Task> action)
            where T : Exception
        {
            return Xunit.Assert.ThrowsAsync<T>(action);
        }
    }
}
