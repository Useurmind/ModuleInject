using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class CustomActionModuleTest
    {
        private CustomActionModule _customActionModule;

        public CustomActionModuleTest()
        {
            this._customActionModule = new CustomActionModule();
        }

        [Fact]
        public void Resolve_RegisterWithClosure_ConstructFromType_CorrectlyResolved()
        {
            this._customActionModule.RegisterWithClosure_ConstructFromType();
            this._customActionModule.Resolve();

            Assert.NotNull(this._customActionModule.MainComponent1);
            Assert.NotNull(this._customActionModule.MainComponent2);
            Assert.Same(this._customActionModule.MainComponent2, this._customActionModule.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolve_RegisterInInterfaceInjector_ConstructFromInstance_CorrectlyResolved()
        {
            this._customActionModule.RegisterInInterfaceInjector_ConstructFromInstance();
            this._customActionModule.Resolve();

            Assert.NotNull(this._customActionModule.MainComponent1);
            Assert.NotNull(this._customActionModule.MainComponent2);
            Assert.Same(this._customActionModule.MainComponent2, this._customActionModule.MainComponent1.MainComponent2);
        }
    }
}
