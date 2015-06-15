using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modularity;
using ModuleInject.Modularity.Registry;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modularity
{
    [TestFixture]
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

            [TestCase]
            public void Resolve_RegistyModuleResolvedCorrectly()
            {
                var testModule = new TestModule();
                var propertyModule = (PropertyModule)testModule.Registry.GetComponent(typeof(IPropertyModule));
                var subModule = (Submodule)testModule.Registry.GetComponent(typeof(ISubModule));

                testModule.Resolve();

                Assert.IsNotNull(testModule.MainComponent1);
                Assert.IsNotNull(propertyModule);
                Assert.IsNotNull(subModule);
                Assert.AreSame(subModule, propertyModule.SubModule);
                Assert.AreSame(propertyModule.Component2, testModule.MainComponent1.MainComponent2);
            }
        }
    }
}
