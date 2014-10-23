using Microsoft.Practices.Unity;
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
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container;
    using global::ModuleInject.Container.Interface;

    [TestFixture]
    public class ModuleResolverTest
    {
        private PropertyModule _module;
        private IDependencyContainer _container;
        private ModuleResolver<IPropertyModule, PropertyModule> _moduleResolver;

        [SetUp]
        public void Init()
        {
            _module = new PropertyModule();
            _module.SubModule = new Submodule();
            _container = new DependencyContainer();
            _moduleResolver = new ModuleResolver<IPropertyModule, PropertyModule>(
                (PropertyModule)_module,
                _container,
                null);
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InstanceRegistrationComponent));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithPropertiesComponent));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize1Component));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize1FromSubComponent));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize2Component));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInitialize3Component));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithInjectorComponent));
            _container.Register<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.Component2));
            _container.Register<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.Component22));
            _container.Register<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.PrivateComponent));
            _container.Register<IMainComponent2, MainComponent2>(Property.Get((PropertyModule x) => x.PrivateInstanceComponent));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.PrivateComponentInjectedProperties));
            _container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.AlsoRegisterForComponent));
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NotAllComponentsRegisteredInContainer_ExceptionThrown()
        {
            IDependencyContainer container = new DependencyContainer();
            container.Register<IMainComponent1, MainComponent1>(Property.Get((PropertyModule x) => x.InitWithPropertiesComponent));

            var moduleResolver = new ModuleResolver<PropertyModule, PropertyModule>(
                (PropertyModule)_module,
                container,
                null);

            moduleResolver.Resolve();
        }

        [TestCase]
        public void Resolve_AllComponentRegisteredInContainer_ComponentPropertiesNotNull()
        {
            _moduleResolver.Resolve();
            
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
            var moduleResolver = new ModuleResolver<PropertyModule, PropertyModule>(
                (PropertyModule)_module,
                _container,
                null);

            moduleResolver.Resolve();
        }
    }
}
