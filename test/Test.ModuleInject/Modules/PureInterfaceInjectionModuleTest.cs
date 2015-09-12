using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    class PureInterfaceInjectionModuleTest
    {
        private PureInterfaceInjectionModule _module;

        public PureInterfaceInjectionModuleTest()
        {
            this._module = new PureInterfaceInjectionModule();
        }

        [Fact]
        public void Resolve_ComponentRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterComponentWithInjector();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_InstanceRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterInstanceWithInjector();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_FactoryRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterInstanceWithInjector();
            this._module.RegisterFactoryWithInjector();
            this._module.Resolve();

            var mainComponent1 = this._module.CreateMainComponent1();
            var mainComponent12 = this._module.CreateMainComponent1();

            Assert.NotNull(mainComponent1);
            Assert.NotNull(mainComponent12);
            Assert.NotNull(mainComponent1.MainComponent2);
            Assert.NotNull(mainComponent12.MainComponent2);
            Assert.NotSame(mainComponent1, mainComponent12);
            Assert.Same(this._module.MainComponent2, mainComponent1.MainComponent2);
            Assert.Same(this._module.MainComponent2, mainComponent12.MainComponent2);
        }
    }
}
