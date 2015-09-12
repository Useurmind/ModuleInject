using System.Linq;

using ModuleInject.Common.Exceptions;

using Xunit;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    
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

        public RegistryUsingModuleTest()
        {
            this._module = new RegistryUsingModule();
        }

        [Fact]
        public void Resolve_NoRegistrySet_ExceptionForMissingSubmodule()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this._module.RegisterComponentFromPublicSubmodule();
                this._module.Resolve();
            });
        }

        [Fact]
        public void Resolved_AfterApplyingRegistryWithSubmodule_SubmoduleIsResolved()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            var registryInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.NotNull(this.PropertyModule.SubModule);
            Assert.Same(registryInstance, this.PropertyModule.SubModule);
        }

        [Fact]
        public void Resolved_AfterApplyingRegistry_RegistryComponentResolved()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)this._module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.NotNull(this.PrivatePropertyModule);
            Assert.Same(registryInstance, this.PrivatePropertyModule);
            Assert.NotNull(this.PrivatePropertyModule.SubModule);
            Assert.Same(registrySubInstance, this.PrivatePropertyModule.SubModule);
            Assert.NotNull(this._module.MainComponent1);
            Assert.Same(registryInstance.Component2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Resolved_AfterRegisteringPrivateComponentAndApplyingRegistry_RegistryComponentWinsOVerRegistryComponent()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.RegisterPrivateWithRegistryComponentOverlap();
            this._module.Resolve();

            var registryInstance = this._module.Registry.GetComponent(typeof(IPropertyModule));
            var registrySubInstance = this._module.Registry.GetComponent(typeof(ISubModule));

            Assert.NotNull(this.PrivatePropertyModule);
            Assert.NotSame(registryInstance, this.PrivatePropertyModule);
            Assert.NotNull(this.PrivatePropertyModule.SubModule);
            Assert.Same(registrySubInstance, this.PrivatePropertyModule.SubModule);
        }
        
        [Fact]
        public void Resolved_AfterRegisteringPropertyOfPrivateComponentFromPrivateSubmodule_ComponentOfSubmoduleInjected()
        {
            this._module.RegisterComponentFromPrivateSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            IPropertyModule registryInstance = (IPropertyModule)this._module.Registry.GetComponent(typeof(IPropertyModule));

            Assert.Same(registryInstance.Component2, this._module.MainComponent1.MainComponent2);
        }

        [Fact]
        public void Dispose__RegistryIsDisposed()
        {
            this._module.RegisterComponentFromPublicSubmodule();
            this._module.ApplyRegistry();
            this._module.Resolve();

            this._module.Dispose();

            Assert.True(this._module.Registry.IsDisposed);
        }
    }
}
