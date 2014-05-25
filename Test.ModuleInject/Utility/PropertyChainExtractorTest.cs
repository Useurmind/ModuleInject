using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Test.ModuleInject.Utility
{
    [TestFixture]
    public class PropertyChainExtractorTest
    {
        public class TestClass1
        {
            public TestClass2 TestClass2 { get; set; }

            public int IntFunction() { return 9; }
        }

        public class TestClass2
        {
            public int IntProperty { get; set; }
        }

        private PropertyChainExtractor _extractor;

        [SetUp]
        public void Init()
        {
            _extractor = new PropertyChainExtractor();
        }

        [TestCase]
        public void Extract_ExpressionWithDepth1_ExtractsSingleMember()
        {
            string propertyName = Property.Get((TestClass2 x) => x.IntProperty);
            Expression<Func<TestClass2, int>> exp = (TestClass2 x) => x.IntProperty;

            IList<MemberExpression> memberExps = _extractor.Extract(exp);

            Assert.AreEqual(1, memberExps.Count);
            Assert.AreEqual(propertyName, memberExps[0].Member.Name);
        }

        [TestCase]
        public void Extract_ExpressionWithDepth2_ExtractsAllMembers()
        {
            string propertyName1 = Property.Get((TestClass1 x) => x.TestClass2);
            string propertyName2 = Property.Get((TestClass2 x) => x.IntProperty);
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.TestClass2.IntProperty;

            IList<MemberExpression> memberExps = _extractor.Extract(exp);

            Assert.AreEqual(2, memberExps.Count);
            Assert.AreEqual(propertyName1, memberExps[0].Member.Name);
            Assert.AreEqual(propertyName2, memberExps[1].Member.Name);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Extract_ExpressionWithFunctionCall_ThrowsException()
        {
            Expression<Func<TestClass1, int>> exp = (TestClass1 x) => x.IntFunction();

            IList<MemberExpression> memberExps = _extractor.Extract(exp);
        }
    }
}
