using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    class PureInterfaceInjectionModuleTest
    {
        private PureInterfaceInjectionModule _module;

        [SetUp]
        public void Setup()
        {
            this._module = new PureInterfaceInjectionModule();
        }

        [TestCase]
        public void Resolve_ComponentRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterComponentWithInjector();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent1);
            Assert.IsNotNull(this._module.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_InstanceRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterInstanceWithInjector();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent1);
            Assert.IsNotNull(this._module.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_FactoryRegistrationWithInjector_InjectorDependenciesResolved()
        {
            this._module.RegisterInstanceWithInjector();
            this._module.RegisterFactoryWithInjector();
            this._module.Resolve();

            var mainComponent1 = this._module.CreateMainComponent1();
            var mainComponent12 = this._module.CreateMainComponent1();

            Assert.IsNotNull(mainComponent1);
            Assert.IsNotNull(mainComponent12);
            Assert.IsNotNull(mainComponent1.MainComponent2);
            Assert.IsNotNull(mainComponent12.MainComponent2);
            Assert.AreNotSame(mainComponent1, mainComponent12);
            Assert.AreSame(this._module.MainComponent2, mainComponent1.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, mainComponent12.MainComponent2);
        }
    }
}
