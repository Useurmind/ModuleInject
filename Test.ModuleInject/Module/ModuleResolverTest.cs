﻿using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Module;
using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject.Module
{
    [TestFixture]
    public class ModuleResolverTest
    {
        private PropertyModule _module;
        private UnityContainer _container;

        [SetUp]
        public void Init()
        {
            _module = new PropertyModule();
            _module.SubModule = new Submodule();
            _container = new UnityContainer();
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InstanceRegistrationComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithPropertiesComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize1Component), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize1FromSubComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize2Component), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize3Component), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInjectorComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.Component2), new InjectionConstructor());
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.Component22), new InjectionConstructor());
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.PrivateComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.PrivateInstanceComponent), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.PrivateComponentInjectedProperties), new InjectionConstructor());
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.AlsoRegisterForComponent), new InjectionConstructor());
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NotAllComponentsRegisteredInContainer_ExceptionThrown()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithPropertiesComponent));

            ModuleResolver.Resolve<IPropertyModule, PropertyModule>((PropertyModule)_module, container); 
        }

        [TestCase]
        public void Resolve_AllComponentRegisteredInContainer_ComponentPropertiesNotNull()
        {
            ModuleResolver.Resolve<IPropertyModule, PropertyModule>((PropertyModule)_module, _container);
            
            Assert.IsNotNull(_module.InitWithPropertiesComponent);
            Assert.IsNotNull(_module.InitWithInitialize1Component);
            Assert.IsNotNull(_module.InitWithInitialize1FromSubComponent);
            Assert.IsNotNull(_module.InitWithInitialize2Component);
            Assert.IsNotNull(_module.InitWithInitialize3Component);
            Assert.IsNotNull(_module.InitWithInjectorComponent);
            Assert.IsNotNull(_module.InstanceRegistrationComponent);
            Assert.IsNotNull(_module.Component2);
            Assert.IsNotNull(_module.Component22);
            Assert.IsNotNull(_module.PrivateComponent);
            Assert.IsNotNull(_module.PrivateInstanceComponent);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonInterface_ExpectException()
        {
            ModuleResolver.Resolve<PropertyModule, PropertyModule>(_module, _container);
        }
    }
}
