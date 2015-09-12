using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    class PropertyInjectionModuleTest
    {
        private PropertyInjectionModule module;

        public PropertyInjectionModuleTest()
        {
            this.module = new PropertyInjectionModule();
        }

        [Fact]
        public void Resolve_RegisterPublicComponentAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicComponentAndInjectNewInstanceInProperty();
            this.module.Resolve();

            Assert.NotNull(this.module.PublicMainComponent1);
            Assert.NotNull(this.module.PublicMainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty();
            this.module.Resolve();

            Assert.NotNull(this.module.PublicMainComponent1);
            Assert.NotNull(this.module.PublicMainComponent1.MainComponent2);
            Assert.Same(this.module.MainComponent2, this.module.PublicMainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicInstanceAndInjectNewInstanceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicInstanceAndInjectNewInstanceInProperty();
            this.module.Resolve();

            Assert.NotNull(this.module.PublicMainComponent1);
            Assert.NotNull(this.module.PublicMainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty_MainComponent2SetInMainComponent1()
        {
            this.module.RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty();
            this.module.Resolve();

            Assert.NotNull(this.module.PublicMainComponent1);
            Assert.NotNull(this.module.PublicMainComponent1.MainComponent2);
            Assert.Same(this.module.MainComponent2, this.module.PublicMainComponent1.MainComponent2);
        }
    }
}
