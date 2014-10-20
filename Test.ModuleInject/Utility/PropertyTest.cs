﻿using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class PropertyTest
    {
        private class TestClass
        {
            public int _a = 0;
            public TestClass2 TestClass2 { get; set; }
        }

        private class TestClass2
        {
            public int A { get; set; }
        }

        [TestCase]
        public void Get_MemberExpressionWithCastOfParameter_ReturnsProperty()
        {
            Property prop = Property.Get((MainComponent1 m) => ((IMainComponent1)m).MainComponent2);

            Assert.IsNotNull(prop);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Get_MemberExpressionWithInvalidCastOfParameter_ErrorThrown()
        {
            Property prop = Property.Get((MainComponent1 m) => ((IMainComponent2)m).StringProperty);

            Assert.IsNotNull(prop);
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

        [TestCase]
        public void GetValue_ValidMemberExpression_ReturnsCorrectValue()
        {
            TestClass test = new TestClass()
                                 {
                                     TestClass2 =  new TestClass2()
                                 };
            Property prop = Property.Get((TestClass t) => t.TestClass2);

            TestClass2 result = prop.GetValue<TestClass2>(test);

            Assert.AreSame(test.TestClass2, result);
        }

        [TestCase]
        public void SetValue_ValidMemberExpression_ValueIsSet()
        {
            TestClass2 test2 = new TestClass2();
            TestClass test = new TestClass();

            Property prop = Property.Get((TestClass t) => t.TestClass2);

            prop.SetValue(test, test2);

            Assert.AreSame(test2, test.TestClass2);
        }
    }
}
