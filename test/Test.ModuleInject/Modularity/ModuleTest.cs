using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Interfaces;
using ModuleInject.Modularity;
using ModuleInject.Modularity.Registry;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modularity
{
    
    public class ModuleTest
    {
        private interface ITestModule : IModule
        {
            IMainComponent1 MainComponent1 { get; }
        }

        private class TestModule : Module, ITestModule
        {
            public IMainComponent1 MainComponent1 { get; private set; }

            [FromRegistry]
            private IPropertyModule PropertyModule { get; set; }

            public TestModule()
            {
                ApplyRegistry();
            }

            protected override void OnRegistryResolved(IRegistry usedRegistry)
            {
                this.MainComponent1 = new MainComponent1();
                this.MainComponent1.MainComponent2 = this.PropertyModule.Component2;
            }

            private void ApplyRegistry()
            {
                var registry = new StandardRegistry();

                registry.RegisterModule<ISubModule, Submodule>();
                registry.RegisterModule<IPropertyModule, PropertyModule>();

                this.Registry = registry;
            }

            [Fact]
            public void Resolve_RegistyModuleResolvedCorrectly()
            {
                var testModule = new TestModule();
                var propertyModule = (PropertyModule)testModule.Registry.GetComponent(typeof(IPropertyModule));
                var subModule = (Submodule)testModule.Registry.GetComponent(typeof(ISubModule));

                testModule.Resolve();

                Assert.NotNull(testModule.MainComponent1);
                Assert.NotNull(propertyModule);
                Assert.NotNull(subModule);
                Assert.Same(subModule, propertyModule.SubModule);
                Assert.Same(propertyModule.Component2, testModule.MainComponent1.MainComponent2);
            }
        }
    }
}
