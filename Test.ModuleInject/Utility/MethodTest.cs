using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class MethodTest
    {
        private class TestClass
        {
            public int _a = 0;
            public int Func() { return 0; }
            public TestClass2 TestClass2 { get; set; }
        }

        private class TestClass2
        {
            public int Func() { return 0; }
        }

        [TestCase]
        public void Get_MethodWithoutParameters_ReturnsMethod()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.IsNotNull(method);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_PropertyExpression_ThrowsException()
        {
            Method method = Method.Get((string s) => s.Length);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_FieldExpression_ThrowsException()
        {
            Method method = Method.Get((TestClass x) => x._a);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_SubMethodExpression_ThrowsException()
        {
            Method method = Method.Get((TestClass x) => x.TestClass2.Func());
        }

        [TestCase]
        public void OperatorString_ValidMemberExpression_ReturnsCorrectName()
        {
            Method method = Method.Get((string s) => s.Clone());

            Assert.AreEqual("Clone", (string)method);
        }
    }
}
