using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    using global::ModuleInject.Common.Exceptions;

    [TestFixture]
    public class ConstructorInjectionModuleTest
    {
        private ConstructorInjectionModule _module;

        [SetUp]
        public void Init()
        {
            _module = new ConstructorInjectionModule();
        }

        [TestCase]
        public void Resolved_RegisterWithDefaultConstructor_MainComponent2Empty()
        {
            _module.RegisterWithDefaultConstructor();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent1);
            Assert.IsNotNull(_module.MainComponent2);
            Assert.IsNull(_module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolved_RegisterWithArgumentsInConstructor_MainComponent2Set()
        {
            _module.RegisterWithArgumentsInConstructor();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent1);
            Assert.IsNotNull(_module.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolved_RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis_MainComponent2Set()
        {
            _module.RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent1);
            Assert.IsNotNull(_module.MainComponent3);
            Assert.AreSame(_module.MainComponent3, _module.MainComponent1.MainComponent2);
        }
    }
}
