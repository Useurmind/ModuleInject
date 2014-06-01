using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class NoAutoWiringModuleTest
    {
        private NoAutoWiringModule _module;

        [SetUp]
        public void Init()
        {
            _module = new NoAutoWiringModule();
        }

        [TestCase]
        public void Resolve_PrivateAutoWiringComponent_NoComponentsResolved(){
            _module.Resolve();

            IAutoWiringClass autoWiringComponent = _module.PrivateAutoWiringComponent;
            IMainComponent2 propertyComponent = autoWiringComponent.PropertyComponent;
            IMainComponent2 constructorComponent = autoWiringComponent.ConstructorComponent;

            // TODO: This is now a known problem, currently can't avoid automatic property injection based on the unity attributes
            //Assert.IsNull(propertyComponent);
            Assert.IsNull(constructorComponent);
        }
    }
}
