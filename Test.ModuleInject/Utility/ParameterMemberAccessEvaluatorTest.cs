using ModuleInject.Common.Linq;
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
    class ParameterMemberAccessEvaluatorTest
    {
        private class TestClass
        {
            public string A { get; set; }

            public int B { get; set; }

            public double C { get; set; }

            public string GetString() { return string.Empty; }

            public TestClass2 GetT2() { return null; }

            public TestClass2 T2 { get; set; }
        }

        private class TestClass2
        {
            public TestClass2(string s1, int i, TestClass2 t2)
            {

            }

            public void MethodWithArguments(double d, string s1, TestClass2 t2)
            {

            }

            public string B { get; set; }

            public string GetString() { return string.Empty; }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Evaluate_StringPropertyAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, string>> expression = t => t.A;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Property.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(string), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(false, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_IntPropertyAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, int>> expression = t => t.B;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Property.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(int), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(false, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_DoublePropertyAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, double>> expression = t => t.C;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Property.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(double), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(false, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_CustomClassPropertyAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, TestClass2>> expression = t => t.T2;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Property.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(TestClass2), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(false, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_StringPropertyAccessDepth2_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, string>> expression = t => t.T2.B;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual("T2.B", memberPathInfo.Path);
            Assert.AreEqual(2, memberPathInfo.Depth);
            Assert.AreEqual(typeof(string), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(false, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_StringMethodAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, string>> expression = t => t.GetString();
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Method.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(string), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(true, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(false, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_CustomClassMethodAccessDepth1_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, TestClass2>> expression = t => t.GetT2();
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual((string)Method.Get(expression), memberPathInfo.Path);
            Assert.AreEqual(1, memberPathInfo.Depth);
            Assert.AreEqual(typeof(TestClass2), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(true, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(false, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_StringCombinedAccessDepth2_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, string>> expression = t => t.GetT2().B;
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual("GetT2.B", memberPathInfo.Path);
            Assert.AreEqual(2, memberPathInfo.Depth);
            Assert.AreEqual(typeof(string), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(true, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_StringCombinedAccessDepth2Reverse_ReturnsCorrectMemberPathInfo()
        {
            Expression<Func<TestClass, string>> expression = t => t.T2.GetString();
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfo = evaluator.MemberPaths.ElementAt(0);

            Assert.AreEqual(1, evaluator.MemberPaths.Count());
            Assert.AreEqual("T2.GetString", memberPathInfo.Path);
            Assert.AreEqual(2, memberPathInfo.Depth);
            Assert.AreEqual(typeof(string), memberPathInfo.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfo.RootType);
            Assert.AreEqual(true, memberPathInfo.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfo.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_ConstructorCallWithMultiplePropertyAccesses_ReturnsCorrectMemberPathInfos()
        {
            Expression<Func<TestClass, TestClass2>> expression = t => new TestClass2(t.A, t.B, t.T2);
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfoArgument1 = evaluator.MemberPaths.First(x => x.Path == "A");
            var memberPathInfoArgument2 = evaluator.MemberPaths.First(x => x.Path == "B");
            var memberPathInfoArgument3 = evaluator.MemberPaths.First(x => x.Path == "T2");

            Assert.AreEqual(3, evaluator.MemberPaths.Count());

            Assert.AreEqual(1, memberPathInfoArgument1.Depth);
            Assert.AreEqual(typeof(string), memberPathInfoArgument1.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument1.RootType);
            Assert.AreEqual(false, memberPathInfoArgument1.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfoArgument1.ContainsPropertyAccess);

            Assert.AreEqual(1, memberPathInfoArgument2.Depth);
            Assert.AreEqual(typeof(int), memberPathInfoArgument2.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument2.RootType);
            Assert.AreEqual(false, memberPathInfoArgument2.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfoArgument2.ContainsPropertyAccess);

            Assert.AreEqual(1, memberPathInfoArgument3.Depth);
            Assert.AreEqual(typeof(TestClass2), memberPathInfoArgument3.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument3.RootType);
            Assert.AreEqual(false, memberPathInfoArgument3.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfoArgument3.ContainsPropertyAccess);
        }

        [Test]
        public void Evaluate_MethodCallWithMultipleCombinedAccesses_ReturnsCorrectMemberPathInfos()
        {
            Expression<Action<TestClass, TestClass2>> expression = (t, t2) => t2.MethodWithArguments(t.C, t.GetT2().B, t.GetT2());
            var evaluator = new ParameterMemberAccessEvaluator(expression, 0);
            evaluator.Evaluate();

            var memberPathInfoArgument1 = evaluator.MemberPaths.First(x => x.Path == "C");
            var memberPathInfoArgument2 = evaluator.MemberPaths.First(x => x.Path == "GetT2.B");
            var memberPathInfoArgument3 = evaluator.MemberPaths.First(x => x.Path == "GetT2");

            Assert.AreEqual(3, evaluator.MemberPaths.Count());

            Assert.AreEqual(1, memberPathInfoArgument1.Depth);
            Assert.AreEqual(typeof(double), memberPathInfoArgument1.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument1.RootType);
            Assert.AreEqual(false, memberPathInfoArgument1.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfoArgument1.ContainsPropertyAccess);

            Assert.AreEqual(2, memberPathInfoArgument2.Depth);
            Assert.AreEqual(typeof(string), memberPathInfoArgument2.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument2.RootType);
            Assert.AreEqual(true, memberPathInfoArgument2.ContainsMethodCall);
            Assert.AreEqual(true, memberPathInfoArgument2.ContainsPropertyAccess);

            Assert.AreEqual(1, memberPathInfoArgument3.Depth);
            Assert.AreEqual(typeof(TestClass2), memberPathInfoArgument3.ReturnType);
            Assert.AreEqual(typeof(TestClass), memberPathInfoArgument3.RootType);
            Assert.AreEqual(true, memberPathInfoArgument3.ContainsMethodCall);
            Assert.AreEqual(false, memberPathInfoArgument3.ContainsPropertyAccess);
        }
    }
}
