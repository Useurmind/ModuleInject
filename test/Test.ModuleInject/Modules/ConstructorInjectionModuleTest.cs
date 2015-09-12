using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class ConstructorInjectionModuleTest
    {
        private ConstructorInjectionModule _module;

        public ConstructorInjectionModuleTest()
        {
            this._module = new ConstructorInjectionModule();
        }

        [Fact]
        public void Resolved_RegisterWithDefaultConstructor_MainComponent2Empty()
        {
            this._module.RegisterWithDefaultConstructor();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Null(this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolved_RegisterWithArgumentsInConstructor_MainComponent2Set()
        {
            this._module.RegisterWithArgumentsInConstructor();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolved_RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis_MainComponent2Set()
        {
            this._module.RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent1);
            Assert.NotNull(this._module.MainComponent3);
            Assert.Same(this._module.MainComponent3, this._module.MainComponent1.MainComponent2);
        }
    }
}
