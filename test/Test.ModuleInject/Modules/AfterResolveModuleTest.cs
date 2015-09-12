using System.Linq;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    class AfterResolveModuleTest
    {
        private AfterResolveModule _module;

        public AfterResolveModuleTest()
        {
            this._module = new AfterResolveModule();
        }

        [Fact]
        public void TestSimpleInitializeCallWithOtherComponent_OtherComponentInjected()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
            Assert.Same(this._module.MainComponent2, this._module.MainInstance1.MainComponent2);

            Assert.Equal(3, this._module.MainComponent1List.Count);
            Assert.Same(this._module.MainComponent1, this._module.MainComponent1List[0]);
            Assert.NotNull(this._module.MainComponent1List[1]);
            Assert.NotNull(this._module.MainComponent1List[2]);
        }
    }
}
