using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class FactoryModuleTest
    {
        private FactoryModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new FactoryModule();
        }

        [TestCase]
        public void Resolve_Double_ExceptionThrown()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.Resolve();
                this._module.Resolve();
            });
        }

        [TestCase]
        public void Resolve_AllPropertiesInitialized()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.RetrievePrivateComponent1());
        }

        [TestCase]
        public void CreateComponent1__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent1 componentFirstCall = this._module.CreateComponent1();
            IMainComponent1 componentSecondCall = this._module.CreateComponent1();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreateComponent2__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent2 componentFirstCall = this._module.CreateComponent2();
            IMainComponent2 componentSecondCall = this._module.CreateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreatePrivateComponent1__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent1 componentFirstCall = this._module.CreatePrivateComponent1();
            IMainComponent1 componentSecondCall = this._module.CreatePrivateComponent1();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreatePrivateComponent2__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent2 componentFirstCall = this._module.CreatePrivateComponent2();
            IMainComponent2 componentSecondCall = this._module.CreatePrivateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void Resolve_NormalInjection_FactoryDependenciesResolvedAndDifferent()
        {
            this._module.Resolve();

            var factoryComponent1 = this._module.RetrievePrivateComponent1().MainComponent2;
            var factoryComponent2 = this._module.RetrievePrivateComponent1().MainComponent22;
            var factoryComponent3 = this._module.RetrievePrivateComponent1().MainComponent23;
            Assert.IsNotNull(factoryComponent1);
            Assert.IsNotNull(factoryComponent2);
            Assert.IsNotNull(factoryComponent3);
            Assert.AreNotSame(factoryComponent1, factoryComponent2);
            Assert.AreNotSame(factoryComponent1, factoryComponent3);
            Assert.AreNotSame(factoryComponent3, factoryComponent2);
        }

        [TestCase]
        public void Resolve_InstancePostInjection_FactoryDependenciesResolvedAndDifferent()
        {
            this._module.Resolve();

            var factoryComponent1 = this._module.InstanceComponent.MainComponent2;
            var factoryComponent2 = this._module.InstanceComponent.MainComponent22;
            var factoryComponent3 = this._module.InstanceComponent.MainComponent23;
            Assert.IsNotNull(factoryComponent1);
            Assert.IsNotNull(factoryComponent2);
            Assert.IsNotNull(factoryComponent3);
            Assert.AreNotSame(factoryComponent1, factoryComponent2);
            Assert.AreNotSame(factoryComponent1, factoryComponent3);
            Assert.AreNotSame(factoryComponent3, factoryComponent2);
        }

        [TestCase]
        public void Resolve_FactoryComponentWithDependencies_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.CreateComponent1();

            var factoryComponent1 = factoryComponent.MainComponent2;
            var factoryComponent2 = factoryComponent.MainComponent22;
            var privateComponent = factoryComponent.MainComponent23;

            Assert.IsNotNull(factoryComponent);
            Assert.IsNotNull(factoryComponent1);
            Assert.IsNotNull(factoryComponent2);
            Assert.IsNotNull(privateComponent);
            Assert.AreNotSame(factoryComponent1, factoryComponent2);
            Assert.AreNotSame(factoryComponent1, privateComponent);
            Assert.AreNotSame(privateComponent, factoryComponent2);
            Assert.AreSame(this._module.RetrievePrivateComponent2(), privateComponent);
        }
        [TestCase]
        public void Resolve_PublicComponentWithDependenciesFromPrivateFactories_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.ComponentWithPrivateComponents;
            var factoryComponent1 = factoryComponent.MainComponent2;
            var factoryComponent2 = factoryComponent.MainComponent22;

            Assert.IsNotNull(factoryComponent);
            Assert.IsNotNull(factoryComponent1);
            Assert.IsNotNull(factoryComponent2);
            Assert.AreNotSame(factoryComponent1, factoryComponent2);
        }

        [TestCase]
        public void Resolve_PrivateFactoryComponentWithDependencies_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.CreatePrivateComponent1();

            var factoryComponent2 = factoryComponent.MainComponent2;
            var privateComponent2 = factoryComponent.MainComponent22;
            var privateFactoryComponent2 = factoryComponent.MainComponent23;

            Assert.IsNotNull(factoryComponent);
            Assert.IsNotNull(factoryComponent2);
            Assert.IsNotNull(privateComponent2);
            Assert.IsNotNull(privateFactoryComponent2);
            Assert.AreNotSame(factoryComponent2, privateComponent2);
            Assert.AreNotSame(privateComponent2, privateFactoryComponent2);
            Assert.AreNotSame(factoryComponent2, privateFactoryComponent2);
            Assert.AreSame(this._module.RetrievePrivateComponent2(), privateComponent2);
        }

        [TestCase]
        public void CreateComponent1_BeforeModuleResolve_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.CreateComponent1();
            });
        }

        [TestCase]
        public void CreateComponent2_BeforeModuleResolve_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.CreateComponent2();
            });
        }
    }
}
