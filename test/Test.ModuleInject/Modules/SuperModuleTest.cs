using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class SuperModuleTest
    {
        private TestSuperModule _superModule;

        [SetUp]
        public void Init()
        {
            this._superModule = new TestSuperModule();
        }

        [TestCase]
        public void Resolve_SubModuleDistributed()
        {
            this._superModule.Resolve();

            Assert.IsNotNull(this._superModule.MainModule);
            Assert.IsNotNull(this._superModule.SubModule);
            Assert.AreSame(this._superModule.SubModule, this._superModule.MainModule.SubModule);
        }

        [TestCase]
        public void Resolve_SubModulePropertiesAppliedInMainModule()
        {
            this._superModule.Resolve();

            Assert.AreSame(this._superModule.SubModule.Component1, this._superModule.MainModule.InstanceRegistrationComponent.SubComponent1);
        }
    }
}
