using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class SuperModuleTest
    {
        private TestSuperModule _superModule;

        public SuperModuleTest()
        {
            this._superModule = new TestSuperModule();
        }

        [Fact]
        public void Resolve_SubModuleDistributed()
        {
            this._superModule.Resolve();

            Assert.NotNull(this._superModule.MainModule);
            Assert.NotNull(this._superModule.SubModule);
            Assert.Same(this._superModule.SubModule, this._superModule.MainModule.SubModule);
        }

        [Fact]
        public void Resolve_SubModulePropertiesAppliedInMainModule()
        {
            this._superModule.Resolve();

            Assert.Same(this._superModule.SubModule.Component1, this._superModule.MainModule.InstanceRegistrationComponent.SubComponent1);
        }
    }
}
