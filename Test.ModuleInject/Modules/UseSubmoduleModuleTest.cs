using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    class UseSubmoduleModuleTest
    {
        private UseSubmoduleModule _module;

        [SetUp]
        public void Setup()
        {
            this._module = new UseSubmoduleModule();
        }

        [TestCase]
        public void Resolve_InjectingSubmoduleProperty_PropertySetCorrectly()
        {
            this._module.RegisterMainComponent_Injecting_SubmoduleProperty();
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreSame(this._module.SubModule.Component1, this._module.MainComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_InjectingSubmoduleFactory_PropertySetCorrectly()
        {
            this._module.RegisterMainComponent_Injecting_SubmoduleFactory();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent.SubComponent1);
        }
    }
}
