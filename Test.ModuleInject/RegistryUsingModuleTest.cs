using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Utility;

    using NUnit.Framework;
    using Test.ModuleInject.TestModules;

    [TestFixture]
    public class RegistryUsingModuleTest
    {
        private RegistryUsingModule _module;

        [SetUp]
        public void Setup()
        {
            _module = new RegistryUsingModule();
        }

        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NoRegistrySet_ExceptionForMissingSubmodule()
        {
            _module.RegisterComponentFromPublicSubmodule();
            _module.Resolve();
        }

        [Test]
        public void Resolved_AfterApplyingRegistryWithSubmodule_SubmoduleIsResolved()
        {
            _module.RegisterComponentFromPublicSubmodule();
            _module.ApplyRegistry();
            _module.Resolve();

            var registryInstance = _module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(_module.PropertyModule.SubModule);
            Assert.AreSame(registryInstance, _module.PropertyModule.SubModule);
        }

        [Test]
        public void Resolved_AfterApplyingRegistry_RegistryComponentResolved()
        {
            _module.RegisterComponentFromPublicSubmodule();
            _module.ApplyRegistry();
            _module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)_module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = _module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(_module.PrivatePropertyModule);
            Assert.AreSame(registryInstance, _module.PrivatePropertyModule);
            Assert.IsNotNull(_module.PrivatePropertyModule.SubModule);
            Assert.AreSame(registrySubInstance, _module.PrivatePropertyModule.SubModule);
            Assert.IsNotNull(_module.MainComponent1);
            Assert.AreSame(registryInstance.Component2, _module.MainComponent1.MainComponent2);
        }

        [Test]
        public void Resolved_AfterRegisteringPrivateComponentAndApplyingRegistry_PrivateComponentWinsOVerRegistryComponent()
        {
            _module.RegisterComponentFromPublicSubmodule();
            _module.ApplyRegistry();
            _module.RegisterPrivateWithRegistryComponentOverlap();
            _module.Resolve();

            var registryInstance = _module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = _module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(_module.PrivatePropertyModule);
            Assert.AreNotSame(registryInstance, _module.PrivatePropertyModule);
            Assert.IsNotNull(_module.PrivatePropertyModule.SubModule);
            Assert.AreSame(registrySubInstance, _module.PrivatePropertyModule.SubModule);
        }
        
        [Test]
        public void Resolved_AfterRegisteringPropertyOfPrivateComponentFromPrivateSubmodule_ComponentOfSubmoduleInjected()
        {
            _module.RegisterComponentFromPrivateSubmodule();
            _module.ApplyRegistry();
            _module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)_module.Registry.GetComponent(typeof(IPropertyModule));

            Assert.AreSame(registryInstance.Component2, _module.MainComponent1.MainComponent2);
        }

        [Test]
        public void Dispose__RegistryIsDisposed()
        {
            _module.RegisterComponentFromPublicSubmodule();
            _module.ApplyRegistry();
            _module.Resolve();

            _module.Dispose();

            Assert.IsTrue(_module.Registry.IsDisposed);
        }
    }
}
