using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    class PropertyInjectionModuleTest
    {
        private PropertyInjectionModule module;

        [SetUp]
        public void Init()
        {
            this.module = new PropertyInjectionModule();
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicComponentAndInjectNewInstanceInProperty();
            this.module.Resolve();

            Assert.IsNotNull(this.module.PublicMainComponent1);
            Assert.IsNotNull(this.module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty();
            this.module.Resolve();

            Assert.IsNotNull(this.module.PublicMainComponent1);
            Assert.IsNotNull(this.module.PublicMainComponent1.MainComponent2);
            Assert.AreSame(this.module.MainComponent2, this.module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicInstanceAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicInstanceAndInjectNewInstanceInProperty();
            this.module.Resolve();

            Assert.IsNotNull(this.module.PublicMainComponent1);
            Assert.IsNotNull(this.module.PublicMainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty();
            this.module.Resolve();

            Assert.IsNotNull(this.module.PublicMainComponent1);
            Assert.IsNotNull(this.module.PublicMainComponent1.MainComponent2);
            Assert.AreSame(this.module.MainComponent2, this.module.PublicMainComponent1.MainComponent2);
        }
    }
}
