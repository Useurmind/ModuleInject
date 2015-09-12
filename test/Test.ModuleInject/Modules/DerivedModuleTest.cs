using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class DerivedModuleTest
    {
        private DerivedModule _derivedModule;

        public DerivedModuleTest()
        {
            this._derivedModule = new DerivedModule();
        }

        [Fact]
        public void Resolve__CorrectlyResolved()
        {
            this._derivedModule.Resolve();

            Assert.NotNull(this._derivedModule.MainComponent1);
            Assert.NotNull(this._derivedModule.MainComponent2);
            Assert.NotNull(this._derivedModule.MainComponent1Private);
            Assert.NotNull(this._derivedModule.MainComponent2Private);
        }
    }
}
