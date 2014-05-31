using ModuleInject;
using ModuleInject.Utility;
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
    public class FactoryModuleTest
    {
        private FactoryModule _module;

        [SetUp]
        public void Init()
        {
            _module = new FactoryModule();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_Double_ExceptionThrown()
        {
            _module.Resolve();
            _module.Resolve();
        }

        [TestCase]
        public void Resolve_AllPropertiesInitialized()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.RetrievePrivateComponent1());
        }

        [TestCase]
        public void CreateComponent1__CreatesNewInstanceEachTime()
        {
            _module.Resolve();

            IMainComponent1 componentFirstCall = _module.CreateComponent1();
            IMainComponent1 componentSecondCall = _module.CreateComponent1();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreateComponent2__CreatesNewInstanceEachTime()
        {
            _module.Resolve();

            IMainComponent2 componentFirstCall = _module.CreateComponent2();
            IMainComponent2 componentSecondCall = _module.CreateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreatePrivateComponent1__CreatesNewInstanceEachTime()
        {
            _module.Resolve();

            IMainComponent1 componentFirstCall = _module.CreatePrivateComponent1();
            IMainComponent1 componentSecondCall = _module.CreatePrivateComponent1();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreatePrivateComponent2__CreatesNewInstanceEachTime()
        {
            _module.Resolve();

            IMainComponent2 componentFirstCall = _module.CreatePrivateComponent2();
            IMainComponent2 componentSecondCall = _module.CreatePrivateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void Resolve_NormalInjection_FactoryDependenciesResolvedAndDifferent()
        {
            _module.Resolve();

            var factoryComponent1 = _module.RetrievePrivateComponent1().MainComponent2;
            var factoryComponent2 = _module.RetrievePrivateComponent1().MainComponent22;
            var factoryComponent3 = _module.RetrievePrivateComponent1().MainComponent23;
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
            _module.Resolve();

            var factoryComponent1 = _module.InstanceComponent.MainComponent2;
            var factoryComponent2 = _module.InstanceComponent.MainComponent22;
            var factoryComponent3 = _module.InstanceComponent.MainComponent23;
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
            _module.Resolve();

            var factoryComponent = _module.CreateComponent1();

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
            Assert.AreSame(_module.RetrievePrivateComponent2(), privateComponent);
        }
        [TestCase]
        public void Resolve_PublicComponentWithDependenciesFromPrivateFactories_AllDependenciesResolved()
        {
            _module.Resolve();

            var factoryComponent = _module.ComponentWithPrivateComponents;
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
            _module.Resolve();

            var factoryComponent = _module.CreatePrivateComponent1();

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
            Assert.AreSame(_module.RetrievePrivateComponent2(), privateComponent2);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void CreateComponent1_BeforeModuleResolve_ThrowsException()
        {
            _module.CreateComponent1();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void CreateComponent2_BeforeModuleResolve_ThrowsException()
        {
            _module.CreateComponent2();
        }
    }
}
