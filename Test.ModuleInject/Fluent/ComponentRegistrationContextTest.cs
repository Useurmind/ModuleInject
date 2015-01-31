using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Modules.Fluent;
using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Fluent
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container;
    using global::ModuleInject.Container.Interface;

    [TestFixture]
    public class ComponentRegistrationContextTest
    {
        private string _componentName;
        private IDependencyContainer _container;
        private ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule> _context;
        private RegistrationTypes _types;
        private RegistrationContext _contextUntyped;

        [SetUp]
        public void Init()
        {
            _types = new RegistrationTypes()
            {
                IComponent = typeof(IMainComponent1),
                TComponent = typeof(MainComponent1),
                IModule = typeof(IPropertyModule),
                TModule = typeof(PropertyModule)
            };
            _componentName = Property.Get((IPropertyModule x) => x.InitWithPropertiesComponent);
            _container = new DependencyContainer();
            _contextUntyped = new RegistrationContext(_componentName, null, _container, _types, false);
            _context = new ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(
                _contextUntyped
                );
        }

        [TestCase]
        public void Constructor_NameSetCorrectly()
        {
            Assert.AreEqual(_componentName, _context.Context.RegistrationName);
        }

        [TestCase]
        public void Inject_DependencyContextValuesSetCorrectly()
        {
            string depPropName = Property.Get((IPropertyModule x) => x.Component2);
            var depContext = (DependencyInjectionContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule, IMainComponent2>)_context.Inject(x => x.Component2);

            Assert.AreSame(_context, depContext.ComponentContext);
        }
    }
}
