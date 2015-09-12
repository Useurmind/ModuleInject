using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class PropertyModuleTest
    {
        private PropertyModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new PropertyModule();
            this._module.SubModule = new Submodule();
        }

        [TestCase]
        public void Resolve_ComponentsAllFilled()
        {            
            this._module.Resolve();

            Assert.IsNotNull(this._module.InstanceRegistrationComponent);
            Assert.IsNotNull(this._module.InitWithPropertiesComponent);
            Assert.IsNotNull(this._module.InitWithInitialize1Component);
            Assert.IsNotNull(this._module.InitWithInitialize1FromSubComponent);
            Assert.IsNotNull(this._module.InitWithInitialize2Component);
            Assert.IsNotNull(this._module.InitWithInitialize3Component);
            Assert.IsNotNull(this._module.InitWithInjectorComponent);
            Assert.IsNotNull(this._module.Component2);
            Assert.IsNotNull(this._module.Component22);
            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.IsNotNull(this._module.SubModule.Component2);

            Assert.AreSame(this._module.FixedInstance, this._module.InstanceRegistrationComponent);
        }

        [TestCase]
        public void Resolve_PropertiesOnInstance_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component22, this._module.InstanceRegistrationComponent.MainComponent22);
        }

        [TestCase]
        public void Resolve_PropertiesOnInstance_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.SubModule.Component1, this._module.InstanceRegistrationComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize1OnInstance_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InstanceRegistrationComponent.MainComponent2);
        }

        [TestCase]
        public void Resolve_Properties_ValueInjected()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.InjectedValue, this._module.InitWithPropertiesComponent.InjectedValue);
        }

        [TestCase]
        public void Resolve_Properties_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InitWithPropertiesComponent.MainComponent2);
            Assert.AreEqual(this._module.Component2, this._module.InitWithPropertiesComponent.ComponentViaSubinterface);
            Assert.AreEqual(this._module.Component2, this._module.Component22.Component2Sub);
        }

        [TestCase]
        public void Resolve_Properties_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreEqual(this._module.SubModule.Component1, this._module.InitWithPropertiesComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Properties_PrivateDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.PrivateComponent);
            Assert.IsNotNull(this._module.PrivateInstanceComponent);
            Assert.AreEqual(this._module.PrivateComponent, this._module.InitWithPropertiesComponent.MainComponent22);
            Assert.AreEqual(this._module.PrivateInstanceComponent, this._module.InitWithPropertiesComponent.MainComponent23);
        }

        [TestCase]
        public void Resolve_Initialize1_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InitWithInitialize1Component.MainComponent2);
        }

        [TestCase]
        public void Resolve_Initialize1_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreEqual(this._module.SubModule.Component1, this._module.InitWithInitialize1FromSubComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize2_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InitWithInitialize2Component.MainComponent2);
        }

        [TestCase]
        public void Resolve_Initialize2_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreEqual(this._module.SubModule.Component1, this._module.InitWithInitialize2Component.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize3_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InitWithInitialize3Component.MainComponent2);
            Assert.AreEqual(this._module.Component22, this._module.InitWithInitialize3Component.MainComponent22);
        }

        [TestCase]
        public void Resolve_Initialize3_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreEqual(this._module.SubModule.Component1, this._module.InitWithInitialize3Component.SubComponent1);
        }

        [TestCase]
        public void Resolve_Injector_ValuesInjected()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.InjectedValue, this._module.InitWithInjectorComponent.InjectedValue);
        }

        [TestCase]
        public void Resolve_Injector_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.Component2, this._module.InitWithInjectorComponent.MainComponent2);
            Assert.AreEqual(this._module.Component22, this._module.InitWithInjectorComponent.MainComponent22);
        }

        [TestCase]
        public void Resolve_Injector_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.IsNotNull(this._module.SubModule.Component1);
            Assert.AreEqual(this._module.SubModule.Component1, this._module.InitWithInjectorComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_PropertiesOnPrivateComponet_AllDependenciesResolved()
        {
            this._module.Resolve();

            Assert.AreEqual(this._module.PrivateComponent, this._module.PrivateComponentInjectedProperties.MainComponent2);
            Assert.AreEqual(this._module.PrivateInstanceComponent, this._module.PrivateComponentInjectedProperties.MainComponent22);
            Assert.AreEqual(this._module.Component2, this._module.PrivateComponentInjectedProperties.MainComponent23);
        }

        [TestCase]
        public void Resolve_PropertiesAlsoRegisteredFor_AreSameAsOriginalComponent()
        {
            this._module.Resolve();

            Assert.AreSame(this._module.InitWithInitialize1Component, this._module.AlsoRegisterForComponent);
        }
    }
}
