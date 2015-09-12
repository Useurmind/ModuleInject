using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Test.ModuleInject.Utility
{
    using global::ModuleInject.Common.Linq;
    
    public class LinqHelperTest
    {
        public class TestClass1
        {
            public TestClass2 TestClass2 { get; set; }
        }

        public interface ITestClass2 { }

        public class TestClass2 : ITestClass2
        {
            public int IntProperty { get; set; }
        }

        [Fact]
        public void GetMemberPathAndType_MemberExpressionDepth1_ReturnsPropertyNameAndType()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = (string)Property.Get(exp);

            string actualPath;
            Type actualType;

            LinqHelper.GetMemberPathAndType(exp, out actualPath, out actualType);

            Assert.Equal(expectedPath, actualPath);
            Assert.Equal(typeof(int), actualType);
        }

        [Fact]
        public void GetMemberPathAndType_MemberExpressionDepth1WithConvert_ReturnsPropertyNameAndType()
        {
            Expression<Func<TestClass1, ITestClass2>> exp = x => (ITestClass2)x.TestClass2;
            string expectedPath = "TestClass2";

            string actualPath;
            Type actualType;

            LinqHelper.GetMemberPathAndType(exp, out actualPath, out actualType);

            Assert.Equal(expectedPath, actualPath);
            Assert.Equal(typeof(TestClass2), actualType);
        }

        [Fact]
        public void GetMemberPathAndType_MethodExpressionDepth1_ReturnsMethodNameAndReturnType()
        {
            Expression<Func<TestClass1, int>> exp = x => x.GetHashCode();
            string expectedPath = (string)Method.Get(exp);

            string actualPath;
            Type actualType;

            LinqHelper.GetMemberPathAndType(exp, out actualPath, out actualType);

            Assert.Equal(expectedPath, actualPath);
            Assert.Equal(typeof(int), actualType);
        }

        [Fact]
        public void GetMemberPath_MemberExpressionDepth1_ReturnsPropertyName()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = (string)Property.Get(exp);

            string result = LinqHelper.GetMemberPath(exp);

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void GetMemberPath_MemberExpressionDepth2_ReturnsCorrectPath()
        {
            Expression<Func<TestClass1, int>> exp = x => x.TestClass2.IntProperty;
            string expectedPath = string.Format("{0}.{1}", (string)Property.Get((TestClass1 x) => x.TestClass2),
                                                            (string)Property.Get((TestClass2 x) => x.IntProperty));

            string result = LinqHelper.GetMemberPath(exp);

            Assert.Equal(expectedPath, result);
        }

        [Fact]
        public void GetMemberPathWithDepth_MemberExpressionDepth1_ReturnsCorrectPathAndDepth1()
        {
            Expression<Func<TestClass2, int>> exp = x => x.IntProperty;
            string expectedPath = string.Format("{0}", (string)Property.Get((TestClass2 x) => x.IntProperty));
            int expectedDepth = 1;

            int actualDepth;
            string result = LinqHelper.GetMemberPath(exp, out actualDepth);

            Assert.Equal(expectedPath, result);
            Assert.Equal(expectedDepth, actualDepth);
        }
    }
}
