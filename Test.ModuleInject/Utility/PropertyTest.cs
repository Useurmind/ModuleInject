using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class PropertyTest
    {
        private class TestClass
        {
            public int _a = 0;
            public TestClass2 TestClass2 { get;set;}
        }

        private class TestClass2
        {
            public int A { get; set; }
        }

        [TestCase]
        public void Get_ValidMemberExpression_ReturnsProperty()
        {
            Property prop = Property.Get((string s) => s.Length);

            Assert.IsNotNull(prop);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_FunctionExpression_ThrowsException()
        {
            Property prop = Property.Get((string s) => s.LongCount());
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_FieldExpression_ThrowsException()
        {
            Property prop = Property.Get((TestClass x) => x._a);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_SubPropertyExpression_ThrowsException()
        {
            Property prop = Property.Get((TestClass x) => x.TestClass2.A);
        }

        [TestCase]
        public void OperatorString_ValidMemberExpression_ReturnsCorrectName()
        {
            Property prop = Property.Get((string s) => s.Length);

            Assert.AreEqual("Length", (string)prop);
        }
    }
}
