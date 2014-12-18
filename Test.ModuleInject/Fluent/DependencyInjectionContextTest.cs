﻿using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Modules.Fluent;
using ModuleInject.Utility;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject.Fluent
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container.Interface;
    using System.Linq.Expressions;

    [TestFixture]
    public class DependencyInjectionContextTest
    {
        string _propertyName;
        Mock<IDependencyContainer> _containerMock;
        ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule> _componentContext;
        DependencyInjectionContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule, IMainComponent2> _depContext;
        private RegistrationTypes _types;
        private RegistrationContext registraitonContextUntyped;
        private DependencyInjectionContext _depContextUntyped;

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

            Expression<Func<PropertyModule, IMainComponent2>> _depSourceExpression = m => m.Component2;

            _propertyName = Property.Get((IPropertyModule x) => x.InitWithPropertiesComponent);
            _containerMock = new Mock<IDependencyContainer>();
            this.registraitonContextUntyped = new RegistrationContext(_propertyName, null, _containerMock.Object, _types, false);
            _componentContext = new ComponentRegistrationContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(
                this.registraitonContextUntyped);
            _depContextUntyped = new DependencyInjectionContext(this.registraitonContextUntyped, _depSourceExpression);
            _depContext = new DependencyInjectionContext<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule, IMainComponent2>(
                _componentContext, _depContextUntyped);
        }

        [TestCase]
        public void Constructor_FieldsSetCorrectly()
        {
            Assert.AreSame(_componentContext, _depContext.ComponentContext);
        }

        [TestCase]
        public void IntoProperty_MainComponent2_ReturnsSameComponentContext()
        {
            var componentContext  = _depContext.IntoProperty(x => x.MainComponent2);

            Assert.AreSame(_componentContext, componentContext);
        }

        //[TestCase]
        //public void IntoProperty_MainComponent2_RegisterTypeCalledOnContainer()
        //{
        //    var registrationContext = _depContext.IntoProperty(x => x.MainComponent2);

        //    this is not possible due to limitations of mock
        //    _containerMock.Verify(x => x.RegisterType<IMainComponent1, MainComponent1>(_propertyName, It.IsAny<InjectionProperty>()), Times.Once);
        //}
    }
}
