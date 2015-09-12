using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class InjectorModuleTest
    {
        private InjectorModule _module;

        public InjectorModuleTest()
        {
            this._module = new InjectorModule();
        }

        [Fact]
        public void Resolve_RegisterClassInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            this._module.RegisterClassInjectorWithPropertyValueAndInitialize1();

            this._module.Resolve();

            Assert.NotNull(this._module.Component1);
            Assert.NotNull(this._module.Component1.MainComponent2);
            Assert.NotNull(this._module.Component1.MainComponent22);
            Assert.NotNull(this._module.Component1.MainComponent23);

            Assert.Equal(this._module.Component2, this._module.Component1.MainComponent2);
            Assert.Equal(this._module.Component22, this._module.Component1.MainComponent22);
        }

        [Fact]
        public void Resolve_RegisterInterfaceInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            this._module.RegisterInterfaceInjectorWithPropertyValueAndInitialize1();

            this._module.Resolve();

            Assert.NotNull(this._module.Component1);
            Assert.NotNull(this._module.Component1.MainComponent2);
            Assert.NotNull(this._module.Component1.MainComponent22);
            Assert.NotNull(this._module.Component1.MainComponent23);

            Assert.Equal(this._module.Component2, this._module.Component1.MainComponent2);
            Assert.Equal(this._module.Component22, this._module.Component1.MainComponent22);
        }

        [Fact]
        public void Resolve_GetWrappedComponent_EverythingIsCorrect()
        {
            this._module.Resolve();

            var wrappedComponent = _module.WrappedComponent as MainComponent2Wrapper;

            Assert.NotNull(wrappedComponent);
            Assert.Equal(2, wrappedComponent.IntProperty);
            Assert.Same(_module.Component2, wrappedComponent.WrappedComponent);
        }
    }
}
