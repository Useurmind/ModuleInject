using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class MethodCallModuleTest
    {
        private MethodCallModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new MethodCallModule();
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.AreEqual(5, this._module.MainComponent1.InjectedValue);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.AreEqual(5, this._module.MainComponent1.InjectedValue);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndDeepCallByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithSubmoduleComponentByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.SubModule.Component1);
            Assert.AreSame(this._module.SubModule.Component1, this._module.MainComponent1.SubComponent1);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithPropertyOfThis_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPropertyOfThis();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            // everything can happen in such a case
            //Assert.IsNull(_module.MainComponent1.MainComponent2);
            Assert.AreEqual(5, this._module.MainComponent1.InjectedValue);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithStackVariable_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithStackVariable();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent1.MainComponent2);
            Assert.AreEqual(5, this._module.MainComponent1.InjectedValue);
        }

        [TestCase]
        public void Resolve_RegisterPublicComponentWithInlineNew_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithInlineNew();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent1.MainComponent2);
            Assert.AreEqual(5, this._module.MainComponent1.InjectedValue);
        }
    }
}
