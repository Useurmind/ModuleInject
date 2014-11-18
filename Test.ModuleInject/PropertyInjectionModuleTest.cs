using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using NUnit.Framework;
    using Test.ModuleInject.TestModules;

    [TestFixture]
    class PropertyInjectionModuleTest
    {
        private PropertyInjectionModule module;

        [SetUp]
        public void Init()
        {
            module = new PropertyInjectionModule();
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            module.RegisterPublicComponentAndInjectNewInstanceInProperty();
            module.Resolve();

            Assert.IsNotNull(module.PublicMainComponent1);
            Assert.IsNotNull(module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            module.RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty();
            module.Resolve();

            Assert.IsNotNull(module.PublicMainComponent1);
            Assert.IsNotNull(module.PublicMainComponent1.MainComponent2);
            Assert.AreSame(module.MainComponent2, module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicInstanceAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            module.RegisterPublicInstanceAndInjectNewInstanceInProperty();
            module.Resolve();

            Assert.IsNotNull(module.PublicMainComponent1);
            Assert.IsNotNull(module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            module.RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty();
            module.Resolve();

            Assert.IsNotNull(module.PublicMainComponent1);
            Assert.IsNotNull(module.PublicMainComponent1.MainComponent2);
            Assert.AreSame(module.MainComponent2, module.PublicMainComponent1.MainComponent2);
        }
    }
}
