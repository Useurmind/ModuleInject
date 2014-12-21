using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class SubPropertyInjectionModuleTest
    {
        private SubPropertyInjectionModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new SubPropertyInjectionModule();
        }

        [TestCase]
        public void TestSubComponent2OfMainComponent2IsInjectedIntoSubComponent1()
        {
            this._module.SetupSubComponent2OfMainComponent2IsInjectedIntoSubComponent1();

            Assert.IsNotNull(this._module.SubComponent1);
            Assert.IsNotNull(this._module.SubComponent2);
            Assert.IsNotNull(this._module.MainComponent2);
            Assert.AreEqual(this._module.SubComponent2, this._module.MainComponent2.SubComponent2);
            Assert.AreEqual(this._module.SubComponent2, this._module.SubComponent1.SubComponent2);
        }
    }
}
