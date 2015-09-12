using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class MethodCallModuleTest
    {
        private MethodCallModule _module;

        public MethodCallModuleTest()
        {
            this._module = new MethodCallModule();
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithPrivateComponentByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Equal(5, this._module.MainComponent1.InjectedValue);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Equal(5, this._module.MainComponent1.InjectedValue);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithPrivateComponentAndDeepCallByMethodCall_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithSubmoduleComponentByMethodCall();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Same(this._module.SubModule.Component1, this._module.MainComponent1.SubComponent1);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithPropertyOfThis_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithPropertyOfThis();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            // everything can happen in such a case
            //Assert.IsNull(_module.MainComponent1.MainComponent2);
            Assert.Equal(5, this._module.MainComponent1.InjectedValue);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithStackVariable_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithStackVariable();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent1.MainComponent2);
            Assert.Equal(5, this._module.MainComponent1.InjectedValue);
        }

        [Fact]
        public void Resolve_RegisterPublicComponentWithInlineNew_FunctionCalledWithArguments()
        {
            this._module.RegisterPublicComponentWithInlineNew();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent1.MainComponent2);
            Assert.Equal(5, this._module.MainComponent1.InjectedValue);
        }
    }
}
