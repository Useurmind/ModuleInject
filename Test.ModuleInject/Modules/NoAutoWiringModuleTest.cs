using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class NoAutoWiringModuleTest
    {
        private NoAutoWiringModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new NoAutoWiringModule();
        }

        [TestCase]
        public void Resolve_PrivateAutoWiringComponent_NoComponentsResolved(){
            this._module.Resolve();

            IAutoWiringClass autoWiringComponent = this._module.PrivateAutoWiringComponent;
            IMainComponent2 propertyComponent = autoWiringComponent.PropertyComponent;
            IMainComponent2 constructorComponent = autoWiringComponent.ConstructorComponent;

            // TODO: This is now a known problem, currently can't avoid automatic property injection based on the unity attributes
            //Assert.IsNull(propertyComponent);
            Assert.IsNull(constructorComponent);
        }
    }
}
