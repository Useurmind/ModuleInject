using Microsoft.Practices.Unity;
using ModuleInject;
using ModuleInject.Fluent;
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
    [TestFixture]
    public class DependencyInjectionContextTest
    {
        string _propertyName;
        string _depPropertyName;
        Mock<IUnityContainer> _containerMock;
        ComponentRegistrationContext<IMainComponent1, MainComponent1, IMainModule> _componentContext;
        DependencyInjectionContext<IMainComponent1, MainComponent1, IMainModule, IMainComponent2> _depContext;

        [SetUp]
        public void Init()
        {
            _propertyName = Property.Get((IMainModule x) => x.InitWithPropertiesComponent);
            _depPropertyName = Property.Get((IMainModule x) => x.Component2);
            _containerMock = new Mock<IUnityContainer>();
            _componentContext = new ComponentRegistrationContext<IMainComponent1, MainComponent1, IMainModule>(
                _propertyName, _containerMock.Object);
            _depContext = new DependencyInjectionContext<IMainComponent1, MainComponent1, IMainModule, IMainComponent2>(
                _componentContext, _depPropertyName);
        }

        [TestCase]
        public void Constructor_FieldsSetCorrectly()
        {
            Assert.AreSame(_componentContext, _depContext.ComponentContext);
            Assert.AreEqual(_depPropertyName, _depContext.DependencyName);
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
        //    var componentContext = _depContext.IntoProperty(x => x.MainComponent2);

        //    this is not possible due to limitations of mock
        //    _containerMock.Verify(x => x.RegisterType<IMainComponent1, MainComponent1>(_propertyName, It.IsAny<InjectionProperty>()), Times.Once);
        //}
    }
}
