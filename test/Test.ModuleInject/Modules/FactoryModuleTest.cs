using System.Linq;

using ModuleInject.Common.Exceptions;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class FactoryModuleTest
    {
        private FactoryModule _module;

        public FactoryModuleTest()
        {
            this._module = new FactoryModule();
        }

        [Fact]
        public void Resolve_Double_ExceptionThrown()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.Resolve();
                this._module.Resolve();
            });
        }

        [Fact]
        public void Resolve_AllPropertiesInitialized()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.RetrievePrivateComponent1());
        }

        [Fact]
        public void CreateComponent1__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent1 componentFirstCall = this._module.CreateComponent1();
            IMainComponent1 componentSecondCall = this._module.CreateComponent1();

            Assert.NotNull(componentFirstCall);
            Assert.NotNull(componentSecondCall);

            Assert.NotSame(componentFirstCall, componentSecondCall);
        }

        [Fact]
        public void CreateComponent2__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent2 componentFirstCall = this._module.CreateComponent2();
            IMainComponent2 componentSecondCall = this._module.CreateComponent2();

            Assert.NotNull(componentFirstCall);
            Assert.NotNull(componentSecondCall);

            Assert.NotSame(componentFirstCall, componentSecondCall);
        }

        [Fact]
        public void CreatePrivateComponent1__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent1 componentFirstCall = this._module.CreatePrivateComponent1();
            IMainComponent1 componentSecondCall = this._module.CreatePrivateComponent1();

            Assert.NotNull(componentFirstCall);
            Assert.NotNull(componentSecondCall);

            Assert.NotSame(componentFirstCall, componentSecondCall);
        }

        [Fact]
        public void CreatePrivateComponent2__CreatesNewInstanceEachTime()
        {
            this._module.Resolve();

            IMainComponent2 componentFirstCall = this._module.CreatePrivateComponent2();
            IMainComponent2 componentSecondCall = this._module.CreatePrivateComponent2();

            Assert.NotNull(componentFirstCall);
            Assert.NotNull(componentSecondCall);

            Assert.NotSame(componentFirstCall, componentSecondCall);
        }

        [Fact]
        public void Resolve_NormalInjection_FactoryDependenciesResolvedAndDifferent()
        {
            this._module.Resolve();

            var factoryComponent1 = this._module.RetrievePrivateComponent1().MainComponent2;
            var factoryComponent2 = this._module.RetrievePrivateComponent1().MainComponent22;
            var factoryComponent3 = this._module.RetrievePrivateComponent1().MainComponent23;
            Assert.NotNull(factoryComponent1);
            Assert.NotNull(factoryComponent2);
            Assert.NotNull(factoryComponent3);
            Assert.NotSame(factoryComponent1, factoryComponent2);
            Assert.NotSame(factoryComponent1, factoryComponent3);
            Assert.NotSame(factoryComponent3, factoryComponent2);
        }

        [Fact]
        public void Resolve_InstancePostInjection_FactoryDependenciesResolvedAndDifferent()
        {
            this._module.Resolve();

            var factoryComponent1 = this._module.InstanceComponent.MainComponent2;
            var factoryComponent2 = this._module.InstanceComponent.MainComponent22;
            var factoryComponent3 = this._module.InstanceComponent.MainComponent23;
            Assert.NotNull(factoryComponent1);
            Assert.NotNull(factoryComponent2);
            Assert.NotNull(factoryComponent3);
            Assert.NotSame(factoryComponent1, factoryComponent2);
            Assert.NotSame(factoryComponent1, factoryComponent3);
            Assert.NotSame(factoryComponent3, factoryComponent2);
        }

        [Fact]
        public void Resolve_FactoryComponentWithDependencies_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.CreateComponent1();

            var factoryComponent1 = factoryComponent.MainComponent2;
            var factoryComponent2 = factoryComponent.MainComponent22;
            var privateComponent = factoryComponent.MainComponent23;

            Assert.NotNull(factoryComponent);
            Assert.NotNull(factoryComponent1);
            Assert.NotNull(factoryComponent2);
            Assert.NotNull(privateComponent);
            Assert.NotSame(factoryComponent1, factoryComponent2);
            Assert.NotSame(factoryComponent1, privateComponent);
            Assert.NotSame(privateComponent, factoryComponent2);
            Assert.Same(this._module.RetrievePrivateComponent2(), privateComponent);
        }
        [Fact]
        public void Resolve_PublicComponentWithDependenciesFromPrivateFactories_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.ComponentWithPrivateComponents;
            var factoryComponent1 = factoryComponent.MainComponent2;
            var factoryComponent2 = factoryComponent.MainComponent22;

            Assert.NotNull(factoryComponent);
            Assert.NotNull(factoryComponent1);
            Assert.NotNull(factoryComponent2);
            Assert.NotSame(factoryComponent1, factoryComponent2);
        }

        [Fact]
        public void Resolve_PrivateFactoryComponentWithDependencies_AllDependenciesResolved()
        {
            this._module.Resolve();

            var factoryComponent = this._module.CreatePrivateComponent1();

            var factoryComponent2 = factoryComponent.MainComponent2;
            var privateComponent2 = factoryComponent.MainComponent22;
            var privateFactoryComponent2 = factoryComponent.MainComponent23;

            Assert.NotNull(factoryComponent);
            Assert.NotNull(factoryComponent2);
            Assert.NotNull(privateComponent2);
            Assert.NotNull(privateFactoryComponent2);
            Assert.NotSame(factoryComponent2, privateComponent2);
            Assert.NotSame(privateComponent2, privateFactoryComponent2);
            Assert.NotSame(factoryComponent2, privateFactoryComponent2);
            Assert.Same(this._module.RetrievePrivateComponent2(), privateComponent2);
        }

        [Fact]
        public void CreateComponent1_BeforeModuleResolve_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.CreateComponent1();
            });
        }

        [Fact]
        public void CreateComponent2_BeforeModuleResolve_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.CreateComponent2();
            });
        }
    }
}
