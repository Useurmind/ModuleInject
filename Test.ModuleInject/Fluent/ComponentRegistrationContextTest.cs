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

        [SetUp]
        public void Init()
        {
            _componentName = Property.Get((IPropertyModule x) => x.InitWithPropertiesComponent);
            _container = new UnityContainer();
            _context = new ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(
                _componentName,
                _container
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
            string depPropName = Property.Get((IPropertyModule x)=> x.Component2);
            var depContext = _context.Inject(x => x.Component2);

            Assert.AreSame(_context, depContext.ComponentContext);
            Assert.AreEqual(depPropName, depContext.DependencyName);
        }
    }
}
