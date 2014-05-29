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
        public void Resolve_AllPropertiesInitialized()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.RetrievePrivateComponent1());
        }

        [TestCase]
        public void CreateComponent1__CreatesNewInstanceEachTime()
        {
            IMainComponent1 componentFirstCall = _module.CreateComponent1();
            IMainComponent1 componentSecondCall = _module.CreateComponent1();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void CreateComponent2__CreatesNewInstanceEachTime()
        {
            IMainComponent2 componentFirstCall = _module.CreateComponent2();
            IMainComponent2 componentSecondCall = _module.CreateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }

        [TestCase]
        public void Resolve_PropertyInjection_FactoryDependenciesResolvedAndDifferent()
        {
            _module.Resolve();

            var factoryComponent1 = _module.RetrievePrivateComponent1().MainComponent2;
            var factoryComponent2 = _module.RetrievePrivateComponent1().MainComponent22;
            Assert.IsNotNull(factoryComponent1);
            Assert.IsNotNull(factoryComponent2);
            Assert.AreNotSame(factoryComponent1, factoryComponent2);
        }
    }
}
