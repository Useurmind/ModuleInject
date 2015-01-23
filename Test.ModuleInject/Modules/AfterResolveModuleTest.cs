using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    class AfterResolveModuleTest
    {
        private AfterResolveModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new AfterResolveModule();
        }

        [TestCase]
        public void TestSimpleInitializeCallWithOtherComponent_OtherComponentInjected()
        {
            this._module.RegisterComponentsByInitializeWithOtherComponent();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainInstance1.MainComponent2);

            Assert.AreEqual(3, this._module.MainComponent1List.Count);
            Assert.AreSame(this._module.MainComponent1, this._module.MainComponent1List[0]);
            Assert.NotNull(this._module.MainComponent1List[1]);
            Assert.NotNull(this._module.MainComponent1List[2]);
        }
    }
}
