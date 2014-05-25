using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class LinqHelperTest
    {
        public class TestClass1
        {
            public TestClass2 TestClass2 { get; set; }
        }

        public class TestClass2
        {
            public int IntProperty { get; set; }
        }

        [TestCase]
        public void GetMemberPath_MemberExpressionDepth1_ReturnsPropertyName()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = (string)Property.Get(exp);

            string result = LinqHelper.GetMemberPath(exp);

            Assert.AreEqual(expectedPath, result);
        }
    }
}
