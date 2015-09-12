using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class SubPropertyInjectionModuleTest
    {
        private SubPropertyInjectionModule _module;

        public SubPropertyInjectionModuleTest()
        {
            this._module = new SubPropertyInjectionModule();
        }

        [Fact]
        public void TestSubComponent2OfMainComponent2IsInjectedIntoSubComponent1()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubComponent1);
            Assert.NotNull(this._module.SubComponent2);
            Assert.NotNull(this._module.MainComponent2);
            Assert.Equal(this._module.SubComponent2, this._module.MainComponent2.SubComponent2);
            Assert.Equal(this._module.SubComponent2, this._module.SubComponent1.SubComponent2);
        }
    }
}
