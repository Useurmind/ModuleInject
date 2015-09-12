using System.Linq;

namespace Test.ModuleInject.Common.Linq
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Common.Linq;
    using Xunit;

    public class MethodTest
    {
        private interface ITestClass
        {
            string StringProperty { get; set; }
        }

        private class TestClass : ITestClass
        {
            public int _a = 0;
            public int Func() { return 0; }
            public TestClass2 TestClass2 { get; set; }
            public string StringProperty { get; set; }
        }

        private class TestClass2
        {
            public int Func() { return 0; }
        }

        [Fact]
        public void Get_MethodWithCastOfParameter_ReturnsProperty()
        {
            Method method = Method.Get((TestClass m) => ((ITestClass)m).GetHashCode());

            Assert.NotNull(method);
        }

        [Fact]
        public void Get_MethodWithInvalidCastOfParameter_ErrorThrown()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass m) => ((ITestClass)m).StringProperty);

                Assert.NotNull(method);
            });
        }

        [Fact]
        public void Get_MethodWithoutParameters_ReturnsMethod()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.NotNull(method);
        }

        [Fact]
        public void Get_PropertyExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((string s) => s.Length);
            });
        }

        [Fact]
        public void Get_FieldExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass x) => x._a);
            });
        }

        [Fact]
        public void Get_SubMethodExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass x) => x.TestClass2.Func());
            });
        }

        [Fact]
        public void OperatorString_ValidMemberExpression_ReturnsCorrectName()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.Equal("Clone", (string)method);
        }
    }
}
