using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class ModuleResolverTest
    {
        private ModuleResolver _moduleResolver;
        private MainModule _module;
        private UnityContainer _container;

        [SetUp]
        public void Init()
        {
            _moduleResolver = new ModuleResolver();
            _module = new MainModule();
            _module.SubModule = new Submodule();
            _container = new UnityContainer();
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InstanceRegistrationComponent));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithPropertiesComponent));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithInitialize1Component));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithInitialize1FromSubComponent));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithInitialize2Component));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithInitialize3Component));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithInjectorComponent));
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((MainModule x) => x.Component2));
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((MainModule x) => x.Component22));
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((MainModule x) => x.PrivateComponent));
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((MainModule x) => x.PrivateInstanceComponent));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.PrivateComponentInjectedProperties));
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NotAllComponentsRegisteredInContainer_ExceptionThrown()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.InitWithPropertiesComponent));

            _moduleResolver.Resolve<IMainModule, MainModule>((MainModule)_module, container); 
        }

        [TestCase]
        public void Resolve_AllComponentRegisteredInContainer_ComponentPropertiesNotNull()
        {
            _moduleResolver.Resolve<IMainModule, MainModule>((MainModule)_module, _container);
            
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
            _moduleResolver.Resolve<MainModule, MainModule>(_module, _container);
        }
    }
}
