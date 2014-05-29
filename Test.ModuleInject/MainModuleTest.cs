using ModuleInject;
using ModuleInject.Utility;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class MainModuleTest
    {
        private MainModule _module;

        [SetUp]
        public void Init()
        {
            _module = new MainModule();
            _module.SubModule = new Submodule();
        }

        [TestCase]
        public void Resolve_ComponentsAllFilled()
        {            
            _module.Resolve();

            Assert.IsNotNull(_module.InstanceRegistrationComponent);
            Assert.IsNotNull(_module.InitWithPropertiesComponent);
            Assert.IsNotNull(_module.InitWithInitialize1Component);
            Assert.IsNotNull(_module.InitWithInitialize1FromSubComponent);
            Assert.IsNotNull(_module.InitWithInitialize2Component);
            Assert.IsNotNull(_module.InitWithInitialize3Component);
            Assert.IsNotNull(_module.InitWithInjectorComponent);
            Assert.IsNotNull(_module.Component2);
            Assert.IsNotNull(_module.Component22);
            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.IsNotNull(_module.SubModule.Component2);

            Assert.AreSame(_module.FixedInstance, _module.InstanceRegistrationComponent);
        }

        [TestCase]
        public void Resolve_PropertiesOnInstance_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component22, _module.InstanceRegistrationComponent.MainComponent22);
        }

        [TestCase]
        public void Resolve_PropertiesOnInstance_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.SubModule.Component1, _module.InstanceRegistrationComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize1OnInstance_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InstanceRegistrationComponent.MainComponent2);
        }

        [TestCase]
        public void Resolve_Properties_ValueInjected()
        {
            _module.Resolve();

            Assert.AreEqual(_module.InjectedValue, _module.InitWithPropertiesComponent.InjectedValue);
        }

        [TestCase]
        public void Resolve_Properties_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InitWithPropertiesComponent.MainComponent2);
        }

        [TestCase]
        public void Resolve_Properties_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreEqual(_module.SubModule.Component1, _module.InitWithPropertiesComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Properties_PrivateDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.PrivateComponent);
            Assert.IsNotNull(_module.PrivateInstanceComponent);
            Assert.AreEqual(_module.PrivateComponent, _module.InitWithPropertiesComponent.MainComponent22);
            Assert.AreEqual(_module.PrivateInstanceComponent, _module.InitWithPropertiesComponent.MainComponent23);
        }

        [TestCase]
        public void Resolve_Initialize1_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InitWithInitialize1Component.MainComponent2);
        }

        [TestCase]
        public void Resolve_Initialize1_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreEqual(_module.SubModule.Component1, _module.InitWithInitialize1FromSubComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize2_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InitWithInitialize2Component.MainComponent2);
        }

        [TestCase]
        public void Resolve_Initialize2_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreEqual(_module.SubModule.Component1, _module.InitWithInitialize2Component.SubComponent1);
        }

        [TestCase]
        public void Resolve_Initialize3_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InitWithInitialize3Component.MainComponent2);
            Assert.AreEqual(_module.Component22, _module.InitWithInitialize3Component.MainComponent22);
        }

        [TestCase]
        public void Resolve_Initialize3_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreEqual(_module.SubModule.Component1, _module.InitWithInitialize3Component.SubComponent1);
        }

        [TestCase]
        public void Resolve_Injector_ValuesInjected()
        {
            _module.Resolve();

            Assert.AreEqual(_module.InjectedValue, _module.InitWithInjectorComponent.InjectedValue);
        }

        [TestCase]
        public void Resolve_Injector_InnerModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.Component2, _module.InitWithInjectorComponent.MainComponent2);
            Assert.AreEqual(_module.Component22, _module.InitWithInjectorComponent.MainComponent22);
        }

        [TestCase]
        public void Resolve_Injector_SubModuleDependenciesResolved()
        {
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreEqual(_module.SubModule.Component1, _module.InitWithInjectorComponent.SubComponent1);
        }

        [TestCase]
        public void Resolve_PropertiesOnPrivateComponet_AllDependenciesResolved()
        {
            _module.Resolve();

            Assert.AreEqual(_module.PrivateComponent, _module.PrivateComponentInjectedProperties.MainComponent2);
            Assert.AreEqual(_module.PrivateInstanceComponent, _module.PrivateComponentInjectedProperties.MainComponent22);
            Assert.AreEqual(_module.Component2, _module.PrivateComponentInjectedProperties.MainComponent23);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponent_NotMarkedWithAttribute_ExceptionThrown()
        {
            _module.RegisterUnattributedPrivateProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponent_PartOfPublicInterface_ExceptionThrown()
        {
            _module.RegisterInterfacePropertyAsPrivate();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponent_WithInstanceNotMarkedWithAttribute_ExceptionThrown()
        {
            _module.RegisterUnattributedPrivatePropertyWithInstance();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponent_WithInstancePartOfPublicInterface_ExceptionThrown()
        {
            _module.RegisterInterfacePropertyAsPrivateWithInstance();
        }

        [TestCase]
        public void FactoryMethod__CreatesNewInstanceEachTime()
        {
            _module.Resolve();

            IMainComponent2 componentFirstCall = _module.CreateComponent2();
            IMainComponent2 componentSecondCall = _module.CreateComponent2();

            Assert.IsNotNull(componentFirstCall);
            Assert.IsNotNull(componentSecondCall);

            Assert.AreNotSame(componentFirstCall, componentSecondCall);
        }
    }
}
