using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    class UseSubmoduleModuleTest
    {
        private UseSubmoduleModule _module;

        public UseSubmoduleModuleTest()
        {
            this._module = new UseSubmoduleModule();
        }

        [Fact]
        public void Resolve_InjectingSubmoduleProperty_PropertySetCorrectly()
        {
            this._module.RegisterMainComponent_Injecting_SubmoduleProperty();
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Same(this._module.SubModule.Component1, this._module.MainComponent.SubComponent1);
        }

        [Fact]
        public void Resolve_InjectingSubmoduleFactory_PropertySetCorrectly()
        {
            this._module.RegisterMainComponent_Injecting_SubmoduleFactory();
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent.SubComponent1);
        }
    }
}
