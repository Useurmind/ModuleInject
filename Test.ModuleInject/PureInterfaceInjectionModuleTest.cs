using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    class PureInterfaceInjectionModuleTest
    {
        private PureInterfaceInjectionModule _module;

        [SetUp]
        public void Setup()
        {
            _module = new PureInterfaceInjectionModule();
        }

        [TestCase]
        public void Resolve_ComponentRegistrationWithInjector_InjectorDependenciesResolved()
        {
            _module.RegisterComponentWithInjector();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent1);
            Assert.IsNotNull(_module.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_InstanceRegistrationWithInjector_InjectorDependenciesResolved()
        {
            _module.RegisterInstanceWithInjector();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent1);
            Assert.IsNotNull(_module.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_FactoryRegistrationWithInjector_InjectorDependenciesResolved()
        {
            _module.RegisterInstanceWithInjector();
            _module.RegisterFactoryWithInjector();
            _module.Resolve();

            var mainComponent1 = _module.CreateMainComponent1();
            var mainComponent12 = _module.CreateMainComponent1();

            Assert.IsNotNull(mainComponent1);
            Assert.IsNotNull(mainComponent12);
            Assert.IsNotNull(mainComponent1.MainComponent2);
            Assert.IsNotNull(mainComponent12.MainComponent2);
            Assert.AreNotSame(mainComponent1, mainComponent12);
            Assert.AreSame(_module.MainComponent2, mainComponent1.MainComponent2);
            Assert.AreSame(_module.MainComponent2, mainComponent12.MainComponent2);
        }
    }
}
