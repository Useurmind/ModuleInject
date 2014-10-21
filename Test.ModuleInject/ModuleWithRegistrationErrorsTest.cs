using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    using global::ModuleInject.Common.Exceptions;

    [TestFixture]
    public class ModuleWithRegistrationErrorsTest
    {
        private ModuleWithRegistrationErrors _module;

        [SetUp]
        public void Init()
        {
            _module = new ModuleWithRegistrationErrors();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentsProperty_ThrowsException()
        {
            _module.RegisterPublicComponentsProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentInstanceProperty_ThrowsException()
        {
            _module.RegisterPublicComponentInstanceProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentsProperty_ThrowsException()
        {
            _module.RegisterPrivateComponentsProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentInstanceProperty_ThrowsExceptiaon()
        {
            _module.RegisterPrivateComponentsInstanceProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicFactoryOfComponent_ThrowsExceptiaon()
        {
            _module.RegisterPublicFactoryOfComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateFactoryOfComponent_ThrowsExceptiaon()
        {
            _module.RegisterPrivateFactoryOfComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentAsPrivateComponent_ThrowsException()
        {
            _module.RegisterPublicComponentAsPrivateComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicFactoryAsPrivateFactory_ThrowsException()
        {
            _module.RegisterPublicFactoryAsPrivateFactory();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentWithoutAttribute_ThrowsException()
        {
            _module.RegisterPrivateComponentWithoutAttribute();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateFactoryWithoutAttribute_ThrowsException()
        {
            _module.RegisterPrivateFactoryWithoutAttribute();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterWithFancyExpression1_ThrowsException() {
            _module.RegisterWithFancyExpression1();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterWithCastToNonImplementedInterface_ThrowsException()
        {
            _module.RegisterWithCastToNonImplementedInterface();
        }
    }
}
