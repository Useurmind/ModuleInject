using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class CustomActionModuleTest
    {
        private CustomActionModule _customActionModule;

        [SetUp]
        public void Setup()
        {
            _customActionModule = new CustomActionModule();
        }

        [TestCase]
        public void Resolve_RegisterWithClosure_ConstructFromType_CorrectlyResolved()
        {
            _customActionModule.RegisterWithClosure_ConstructFromType();
            _customActionModule.Resolve();

            Assert.IsNotNull(_customActionModule.MainComponent1);
            Assert.IsNotNull(_customActionModule.MainComponent2);
            Assert.AreSame(_customActionModule.MainComponent2, _customActionModule.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterInInterfaceInjector_ConstructFromInstance_CorrectlyResolved()
        {
            _customActionModule.RegisterInInterfaceInjector_ConstructFromInstance();
            _customActionModule.Resolve();

            Assert.IsNotNull(_customActionModule.MainComponent1);
            Assert.IsNotNull(_customActionModule.MainComponent2);
            Assert.AreSame(_customActionModule.MainComponent2, _customActionModule.MainComponent1.MainComponent2);
        }
    }
}
