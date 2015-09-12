using ModuleInject.Utility;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Test.ModuleInject.Utility
{
    using global::ModuleInject.Common.Linq;

    
    public class MemberChainEvaluatorTest
    {
        public class TestClass1
        {
            public TestClass2 TestClass2 { get; set; }

            public TestClass2 TestClass2Function() { return null; }

            public int IntFunction() { return 9; }
        }

        public class TestClass2
        {
            public int IntFunction() { return 0; }
            public int IntProperty { get; set; }
        }

        private MemberChainEvaluator _extractor;

        public MemberChainEvaluatorTest()
        {
            _extractor = new MemberChainEvaluator();
        }

        [Fact]
        public void Extract_ExpressionWithDepth1_ExtractsSingleMember()
        {
            string propertyName = Property.Get((TestClass2 x) => x.IntProperty);
            Expression<Func<TestClass2, int>> exp = (TestClass2 x) => x.IntProperty;

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(1, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass2), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(propertyName, ((MemberExpression)memberExps[0]).Member.Name);
            Assert.Equal(propertyName, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithDepth2_ExtractsAllMembers()
        {
            string propertyName1 = Property.Get((TestClass1 x) => x.TestClass2);
            string propertyName2 = Property.Get((TestClass2 x) => x.IntProperty);
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.TestClass2.IntProperty;

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(2, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass1), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(propertyName1, ((MemberExpression)memberExps[0]).Member.Name);
            Assert.Equal(propertyName2, ((MemberExpression)memberExps[1]).Member.Name);
            Assert.Equal(propertyName1 + "." + propertyName2, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithFunctionCall_CorrectExpressionsInList()
        {
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.IntFunction();
            string methodName1 = Method.Get((TestClass1 x) => x.IntFunction());

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(1, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass1), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(methodName1, ((MethodCallExpression)memberExps[0]).Method.Name);
            Assert.Equal(methodName1, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithDualFunctionCall_CorrectExpressionsInList()
        {
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.TestClass2Function().IntFunction();
            string methodName1 = Method.Get((TestClass1 x) => x.TestClass2Function());
            string methodName2 = Method.Get((TestClass2 x) => x.IntFunction());

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(2, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass1), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(methodName1, ((MethodCallExpression)memberExps[0]).Method.Name);
            Assert.Equal(methodName2, ((MethodCallExpression)memberExps[1]).Method.Name);
            Assert.Equal(methodName1 + "." + methodName2, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithPropertyThenFunctionCall_CorrectExpressionsInList()
        {
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.TestClass2.IntFunction();
            string propertyName1 = Property.Get((TestClass1 x) => x.TestClass2);
            string methodName2 = Method.Get((TestClass2 x) => x.IntFunction());

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(2, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass1), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(propertyName1, ((MemberExpression)memberExps[0]).Member.Name);
            Assert.Equal(methodName2, ((MethodCallExpression)memberExps[1]).Method.Name);
            Assert.Equal(propertyName1 + "." + methodName2, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithFunctionCallThenProperty_CorrectExpressionsInList()
        {
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.TestClass2Function().IntProperty;
            string methodName1 = Method.Get((TestClass1 x) => x.TestClass2Function());
            string propertyName2 = Property.Get((TestClass2 x) => x.IntProperty);

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(2, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(int), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass1), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(methodName1, ((MethodCallExpression)memberExps[0]).Method.Name);
            Assert.Equal(propertyName2, ((MemberExpression)memberExps[1]).Member.Name);
            Assert.Equal(methodName1 + "." + propertyName2, _extractor.MemberPath);
        }

        [Fact]
        public void Extract_ExpressionWithPropertyAndCast_CorrectExpressionsInList()
        {
            Expression<Func<TestClass2, double>> exp = (TestClass2 x) => (double)x.IntProperty;
            string propertyName2 = Property.Get((TestClass2 x) => x.IntProperty);

            IList<Expression> memberExps = _extractor.Extract(exp.Body);

            Assert.Equal(1, memberExps.Count);
            Assert.Equal(typeof(int), _extractor.MemberType);
            Assert.Equal(typeof(double), _extractor.ReturnType);
            Assert.Equal(typeof(TestClass2), _extractor.RootType);
            Assert.Equal("x", _extractor.RootName);
            Assert.Equal(propertyName2, ((MemberExpression)memberExps[0]).Member.Name);
            Assert.Equal( propertyName2, _extractor.MemberPath);
        }
    }
}
