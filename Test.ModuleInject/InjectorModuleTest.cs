using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class InjectorModuleTest
    {
        private InjectorModule _module;
        [SetUp]
        public void Init()
        {
            _module = new InjectorModule();
        }

        [TestCase]
        public void Resolve_RegisterClassInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            _module.RegisterClassInjectorWithPropertyValueAndInitialize1();

            _module.Resolve();

            Assert.IsNotNull(_module.Component1);
            Assert.IsNotNull(_module.Component1.MainComponent2);
            Assert.IsNotNull(_module.Component1.MainComponent22);
            Assert.IsNotNull(_module.Component1.MainComponent23);

            Assert.AreEqual(_module.Component2, _module.Component1.MainComponent2);
            Assert.AreEqual(_module.Component22, _module.Component1.MainComponent22);
        }

        [TestCase]
        public void Resolve_RegisterInterfaceInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            _module.RegisterInterfaceInjectorWithPropertyValueAndInitialize1();

            _module.Resolve();

            Assert.IsNotNull(_module.Component1);
            Assert.IsNotNull(_module.Component1.MainComponent2);
            Assert.IsNotNull(_module.Component1.MainComponent22);
            Assert.IsNotNull(_module.Component1.MainComponent23);

            Assert.AreEqual(_module.Component2, _module.Component1.MainComponent2);
            Assert.AreEqual(_module.Component22, _module.Component1.MainComponent22);
        }
    }
}
