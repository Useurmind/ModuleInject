﻿using System.Linq;

using ModuleInject.Common.Exceptions;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
    public class PropertyModuleTest
    {
        private PropertyModule _module;

        public PropertyModuleTest()
        {
            this._module = new PropertyModule();
            this._module.SubModule = new Submodule();
        }

        [Fact]
        public void Resolve_ComponentsAllFilled()
        {            
            this._module.Resolve();

            Assert.NotNull(this._module.InstanceRegistrationComponent);
            Assert.NotNull(this._module.InitWithPropertiesComponent);
            Assert.NotNull(this._module.InitWithInitialize1Component);
            Assert.NotNull(this._module.InitWithInitialize1FromSubComponent);
            Assert.NotNull(this._module.InitWithInitialize2Component);
            Assert.NotNull(this._module.InitWithInitialize3Component);
            Assert.NotNull(this._module.InitWithInjectorComponent);
            Assert.NotNull(this._module.Component2);
            Assert.NotNull(this._module.Component22);
            Assert.NotNull(this._module.SubModule.Component1);
            Assert.NotNull(this._module.SubModule.Component2);

            Assert.Same(this._module.FixedInstance, this._module.InstanceRegistrationComponent);
        }

        [Fact]
        public void Resolve_PropertiesOnInstance_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component22, this._module.InstanceRegistrationComponent.MainComponent22);
        }

        [Fact]
        public void Resolve_PropertiesOnInstance_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.SubModule.Component1, this._module.InstanceRegistrationComponent.SubComponent1);
        }

        [Fact]
        public void Resolve_Initialize1OnInstance_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InstanceRegistrationComponent.MainComponent2);
        }

        [Fact]
        public void Resolve_Properties_ValueInjected()
        {
            this._module.Resolve();

            Assert.Equal(this._module.InjectedValue, this._module.InitWithPropertiesComponent.InjectedValue);
        }

        [Fact]
        public void Resolve_Properties_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InitWithPropertiesComponent.MainComponent2);
            Assert.Equal(this._module.Component2, this._module.InitWithPropertiesComponent.ComponentViaSubinterface);
            Assert.Equal(this._module.Component2, this._module.Component22.Component2Sub);
        }

        [Fact]
        public void Resolve_Properties_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Equal(this._module.SubModule.Component1, this._module.InitWithPropertiesComponent.SubComponent1);
        }

        [Fact]
        public void Resolve_Properties_PrivateDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.PrivateComponent);
            Assert.NotNull(this._module.PrivateInstanceComponent);
            Assert.Equal(this._module.PrivateComponent, this._module.InitWithPropertiesComponent.MainComponent22);
            Assert.Equal(this._module.PrivateInstanceComponent, this._module.InitWithPropertiesComponent.MainComponent23);
        }

        [Fact]
        public void Resolve_Initialize1_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InitWithInitialize1Component.MainComponent2);
        }

        [Fact]
        public void Resolve_Initialize1_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Equal(this._module.SubModule.Component1, this._module.InitWithInitialize1FromSubComponent.SubComponent1);
        }

        [Fact]
        public void Resolve_Initialize2_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InitWithInitialize2Component.MainComponent2);
        }

        [Fact]
        public void Resolve_Initialize2_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Equal(this._module.SubModule.Component1, this._module.InitWithInitialize2Component.SubComponent1);
        }

        [Fact]
        public void Resolve_Initialize3_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InitWithInitialize3Component.MainComponent2);
            Assert.Equal(this._module.Component22, this._module.InitWithInitialize3Component.MainComponent22);
        }

        [Fact]
        public void Resolve_Initialize3_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Equal(this._module.SubModule.Component1, this._module.InitWithInitialize3Component.SubComponent1);
        }

        [Fact]
        public void Resolve_Injector_ValuesInjected()
        {
            this._module.Resolve();

            Assert.Equal(this._module.InjectedValue, this._module.InitWithInjectorComponent.InjectedValue);
        }

        [Fact]
        public void Resolve_Injector_InnerModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.Component2, this._module.InitWithInjectorComponent.MainComponent2);
            Assert.Equal(this._module.Component22, this._module.InitWithInjectorComponent.MainComponent22);
        }

        [Fact]
        public void Resolve_Injector_SubModuleDependenciesResolved()
        {
            this._module.Resolve();

            Assert.NotNull(this._module.SubModule.Component1);
            Assert.Equal(this._module.SubModule.Component1, this._module.InitWithInjectorComponent.SubComponent1);
        }

        [Fact]
        public void Resolve_PropertiesOnPrivateComponet_AllDependenciesResolved()
        {
            this._module.Resolve();

            Assert.Equal(this._module.PrivateComponent, this._module.PrivateComponentInjectedProperties.MainComponent2);
            Assert.Equal(this._module.PrivateInstanceComponent, this._module.PrivateComponentInjectedProperties.MainComponent22);
            Assert.Equal(this._module.Component2, this._module.PrivateComponentInjectedProperties.MainComponent23);
        }

        [Fact]
        public void Resolve_PropertiesAlsoRegisteredFor_AreSameAsOriginalComponent()
        {
            this._module.Resolve();

            Assert.Same(this._module.InitWithInitialize1Component, this._module.AlsoRegisterForComponent);
        }
    }
}
