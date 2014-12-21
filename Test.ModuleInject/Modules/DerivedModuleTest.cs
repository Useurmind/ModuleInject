using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class DerivedModuleTest
    {
        private DerivedModule _derivedModule;

        [SetUp]
        public void Setup()
        {
            this._derivedModule = new DerivedModule();
        }

        [TestCase]
        public void Resolve__CorrectlyResolved()
        {
            this._derivedModule.Resolve();

            Assert.IsNotNull(this._derivedModule.MainComponent1);
            Assert.IsNotNull(this._derivedModule.MainComponent2);
            Assert.IsNotNull(this._derivedModule.MainComponent1Private);
            Assert.IsNotNull(this._derivedModule.MainComponent2Private);
        }
    }
}
