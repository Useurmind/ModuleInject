using System.Linq;

namespace Test.ModuleInject.Common.Linq
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Common.Linq;

    using NUnit.Framework;

    [TestFixture]
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

        [TestCase]
        public void Get_MethodWithCastOfParameter_ReturnsProperty()
        {
            Method method = Method.Get((TestClass m) => ((ITestClass)m).GetHashCode());

            Assert.IsNotNull(method);
        }

        [TestCase]
        public void Get_MethodWithInvalidCastOfParameter_ErrorThrown()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass m) => ((ITestClass)m).StringProperty);

                Assert.IsNotNull(method);
            });
        }

        [TestCase]
        public void Get_MethodWithoutParameters_ReturnsMethod()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.IsNotNull(method);
        }

        [TestCase]
        public void Get_PropertyExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((string s) => s.Length);
            });
        }

        [TestCase]
        public void Get_FieldExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass x) => x._a);
            });
        }

        [TestCase]
        public void Get_SubMethodExpression_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                Method method = Method.Get((TestClass x) => x.TestClass2.Func());
            });
        }

        [TestCase]
        public void OperatorString_ValidMemberExpression_ReturnsCorrectName()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.AreEqual("Clone", (string)method);
        }
    }
}
