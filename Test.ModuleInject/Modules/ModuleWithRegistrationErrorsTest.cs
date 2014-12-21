using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class ModuleWithRegistrationErrorsTest
    {
        private ModuleWithRegistrationErrors _module;

        [SetUp]
        public void Init()
        {
            this._module = new ModuleWithRegistrationErrors();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentsProperty_ThrowsException()
        {
            this._module.RegisterPublicComponentsProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentInstanceProperty_ThrowsException()
        {
            this._module.RegisterPublicComponentInstanceProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentsProperty_ThrowsException()
        {
            this._module.RegisterPrivateComponentsProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentInstanceProperty_ThrowsExceptiaon()
        {
            this._module.RegisterPrivateComponentsInstanceProperty();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicFactoryOfComponent_ThrowsExceptiaon()
        {
            this._module.RegisterPublicFactoryOfComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateFactoryOfComponent_ThrowsExceptiaon()
        {
            this._module.RegisterPrivateFactoryOfComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicComponentAsPrivateComponent_ThrowsException()
        {
            this._module.RegisterPublicComponentAsPrivateComponent();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPublicFactoryAsPrivateFactory_ThrowsException()
        {
            this._module.RegisterPublicFactoryAsPrivateFactory();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateComponentWithoutAttribute_ThrowsException()
        {
            this._module.RegisterPrivateComponentWithoutAttribute();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterPrivateFactoryWithoutAttribute_ThrowsException()
        {
            this._module.RegisterPrivateFactoryWithoutAttribute();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void RegisterWithFancyExpression1_ThrowsException() {
            this._module.RegisterWithFancyExpression1();
        }

        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_RegisterWithCastToNonImplementedInterface_ThrowsException()
        {
            this._module.RegisterWithCastToNonImplementedInterface();
            this._module.Resolve();
        }
    }
}
