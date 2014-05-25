using ModuleInject;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class InjectionModuleTest
    {
        private IMainModule _module;

        [SetUp]
        public void Init()
        {
            _module = new MainModule(); 
        }

        [TestCase]
        public void Resolve_ComponentsAllFilled()
        {            
            _module.Resolve();

            Assert.IsNotNull(_module.Component1);
            Assert.IsNotNull(_module.SecondComponent1);
        }

        [TestCase]
        public void Resolve_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.Component1.MainComponent2);
        }

        [TestCase]
        public void Resolve_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.SubModule.Component1, _module.Component1.SubComponent1);
        }
    }
}
