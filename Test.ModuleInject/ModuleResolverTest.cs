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
            _container = new UnityContainer();
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.Component1));
            _container.RegisterType<IMainComponent2, MainComponent2>(Property.Get((MainModule x) => x.Component2));
            _container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.SecondComponent1));
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NotAllComponentsRegisteredInContainer_ExceptionThrown()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMainComponent1, MainComponent1>(Property.Get((MainModule x) => x.Component1));

            _moduleResolver.Resolve<IMainModule>((IMainModule)_module, container);

            Assert.IsNotNull(_module.Component1);
            Assert.IsNotNull(_module.SecondComponent1);            
        }

        [TestCase]
        public void Resolve_AllComponentRegisteredInContainer_ComponentPropertiesNotNull()
        {
            _moduleResolver.Resolve<IMainModule>((IMainModule)_module, _container);

            Assert.IsNotNull(_module.Component1);
            Assert.IsNotNull(_module.SecondComponent1);
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonInterface_ExpectException()
        {
            _moduleResolver.Resolve<MainModule>(_module, _container);
        }
    }
}
