using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject.Fluent
{
    [TestFixture]
    public class ComponentRegistrationContextTest
    {
        private string _componentName;
        private IUnityContainer _container;
        private ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule> _context;
        private ComponentRegistrationTypes _types;
        private ComponentRegistrationContext _contextUntyped;

        [SetUp]
        public void Init()
        {
            _types = new ComponentRegistrationTypes()
            {
                IComponent = typeof(IMainComponent1),
                TComponent = typeof(MainComponent1),
                IModule = typeof(IPropertyModule),
                TModule = typeof(PropertyModule)
            };
            _componentName = Property.Get((IPropertyModule x) => x.InitWithPropertiesComponent);
            _container = new UnityContainer();
            _contextUntyped = new ComponentRegistrationContext(_componentName, _container, _types, false);
            _context = new ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(
                _contextUntyped
                );
        }

        [TestCase]
        public void Constructor_NameSetCorrectly()
        {
            Assert.AreEqual(_componentName, _context.ComponentName);
        }

        [TestCase]
        public void Inject_DependencyContextValuesSetCorrectly()
        {
            string depPropName = Property.Get((IPropertyModule x) => x.Component2);
            var depContext = _context.Inject(x => x.Component2);

            Assert.AreSame(_context, depContext.ComponentContext);
            Assert.AreEqual(depPropName, depContext.DependencyName);
        }
    }
}
