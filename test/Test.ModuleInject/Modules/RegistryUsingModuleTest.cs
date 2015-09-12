using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class RegistryUsingModuleTest
    {
        private RegistryUsingModule _module;

        private PropertyModule PropertyModule
        {
            get
            {
                return (PropertyModule)this._module.PropertyModule;
            }
        }

        private PropertyModule PrivatePropertyModule
        {
            get
            {
                return (PropertyModule)this._module.PrivatePropertyModule;
            }
        }

        [SetUp]
        public void Setup()
        {
            this._module = new RegistryUsingModule();
        }

        [Test]
        public void Resolve_NoRegistrySet_ExceptionForMissingSubmodule()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.RegisterComponentFromPublicSubmodule();
                this._module.Resolve();
            });
        }

        [Test]
        public void Resolved_AfterApplyingRegistryWithSubmodule_SubmoduleIsResolved()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            var registryInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(this.PropertyModule.SubModule);
            Assert.AreSame(registryInstance, this.PropertyModule.SubModule);
        }

        [Test]
        public void Resolved_AfterApplyingRegistry_RegistryComponentResolved()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)this._module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(this.PrivatePropertyModule);
            Assert.AreSame(registryInstance, this.PrivatePropertyModule);
            Assert.IsNotNull(this.PrivatePropertyModule.SubModule);
            Assert.AreSame(registrySubInstance, this.PrivatePropertyModule.SubModule);
            Assert.IsNotNull(this._module.MainComponent1);
            Assert.AreSame(registryInstance.Component2, this._module.MainComponent1.MainComponent2);
        }

        [Test]
        public void Resolved_AfterRegisteringPrivateComponentAndApplyingRegistry_RegistryComponentWinsOVerRegistryComponent()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.RegisterPrivateWithRegistryComponentOverlap();
            this._module.Resolve();

            var registryInstance = this._module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(this.PrivatePropertyModule);
            Assert.AreNotSame(registryInstance, this.PrivatePropertyModule);
            Assert.IsNotNull(this.PrivatePropertyModule.SubModule);
            Assert.AreSame(registrySubInstance, this.PrivatePropertyModule.SubModule);
        }
        
        [Test]
        public void Resolved_AfterRegisteringPropertyOfPrivateComponentFromPrivateSubmodule_ComponentOfSubmoduleInjected()
        {
            this._module.RegisterComponentFromPrivateSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)this._module.Registry.GetComponent(typeof(IPropertyModule));

            Assert.AreSame(registryInstance.Component2, this._module.MainComponent1.MainComponent2);
        }

        [Test]
        public void Dispose__RegistryIsDisposed()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            this._module.Dispose();

            Assert.IsTrue(this._module.Registry.IsDisposed);
        }
    }
}
