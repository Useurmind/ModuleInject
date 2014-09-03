using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class SubPropertyInjectionModuleTest
    {
        private SubPropertyInjectionModule _module;

        [SetUp]
        public void Init()
        {
            _module = new SubPropertyInjectionModule();
        }

        [TestCase]
        public void TestSubComponent2OfMainComponent2IsInjectedIntoSubComponent1()
        {
            _module.SetupSubComponent2OfMainComponent2IsInjectedIntoSubComponent1();

            Assert.IsNotNull(_module.SubComponent1);
            Assert.IsNotNull(_module.SubComponent2);
            Assert.IsNotNull(_module.MainComponent2);
            Assert.AreEqual(_module.SubComponent2, _module.MainComponent2.SubComponent2);
            Assert.AreEqual(_module.SubComponent2, _module.SubComponent1.SubComponent2);
        }
    }
}
