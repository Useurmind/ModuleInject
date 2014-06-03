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
        public void GetMemberPathAndType_MemberExpressionDepth1_ReturnsPropertyNameAndType()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = (string)Property.Get(exp);

            string actualPath;
            Type actualType;

            LinqHelper.GetMemberPathAndType(exp, out actualPath, out actualType);

            Assert.AreEqual(expectedPath, actualPath);
            Assert.AreEqual(typeof(int), actualType);
        }

        [TestCase]
        public void GetMemberPathAndType_MethodExpressionDepth1_ReturnsMethodNameAndReturnType()
        {
            Expression<Func<TestClass1, int>> exp = x => x.GetHashCode();
            string expectedPath = (string)Method.Get(exp);

            string actualPath;
            Type actualType;

            LinqHelper.GetMemberPathAndType(exp, out actualPath, out actualType);

            Assert.AreEqual(expectedPath, actualPath);
            Assert.AreEqual(typeof(int), actualType);
        }

        [TestCase]
        public void GetMemberPath_MemberExpressionDepth1_ReturnsPropertyName()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = (string)Property.Get(exp);

            string result = LinqHelper.GetMemberPath(exp);

            Assert.AreEqual(expectedPath, result);
        }

        [TestCase]
        public void GetMemberPath_MemberExpressionDepth2_ReturnsCorrectPath()
        {
            Expression<Func<TestClass1, int>> exp = x => x.TestClass2.IntProperty;
            string expectedPath = string.Format("{0}.{1}", (string)Property.Get((TestClass1 x) => x.TestClass2),
                                                            (string)Property.Get((TestClass2 x) => x.IntProperty));

            string result = LinqHelper.GetMemberPath(exp);

            Assert.AreEqual(expectedPath, result);
        }

        [TestCase]
        public void GetMemberPathWithDepth_MemberExpressionDepth1_ReturnsCorrectPathAndDepth1()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = string.Format("{0}", (string)Property.Get((TestClass2 x) => x.IntProperty));
            int expectedDepth = 1;

            int actualDepth;
            string result = LinqHelper.GetMemberPath(exp, out actualDepth);

            Assert.AreEqual(expectedPath, result);
            Assert.AreEqual(expectedDepth, actualDepth);
        }
    }
}
