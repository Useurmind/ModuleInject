using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class MethodCallModuleTest
    {
        private MethodCallModule _module;

        [SetUp]
        public void Init()
        {
            _module = new MethodCallModule();
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentByMethodCall_FunctionCalledWithArguments()
        {
            _module.RegisterPublicComponentWithPrivateComponentByMethodCall();
            _module.Resolve();

            Assert.NotNull(_module.MainComponent1);
            Assert.NotNull(_module.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall_FunctionCalledWithArguments()
        {
            _module.RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall();
            _module.Resolve();

            Assert.NotNull(_module.MainComponent1);
            Assert.NotNull(_module.MainComponent2);
            Assert.AreEqual(5, _module.MainComponent1.InjectedValue);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall_FunctionCalledWithArguments()
        {
            _module.RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall();
            _module.Resolve();

            Assert.NotNull(_module.MainComponent1);
            Assert.NotNull(_module.MainComponent2);
            Assert.AreEqual(5, _module.MainComponent1.InjectedValue);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndDeepCallByMethodCall_FunctionCalledWithArguments()
        {
            _module.RegisterPublicComponentWithSubmoduleComponentByMethodCall();
            _module.Resolve();

            Assert.NotNull(_module.MainComponent1);
            Assert.NotNull(_module.SubModule.Component1);
            Assert.AreSame(_module.SubModule.Component1, _module.MainComponent1.SubComponent1);
        }
    }
}
