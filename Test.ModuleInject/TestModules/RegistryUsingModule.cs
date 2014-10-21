using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using System.Runtime.Remoting.Messaging;

    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;
    using global::ModuleInject.Registry;

    public interface IRegistryUsingModule : IInjectionModule
    {
        IPropertyModule PropertyModule { get; }
    }

    public class RegistryUsingModule : InjectionModule<IRegistryUsingModule, RegistryUsingModule>, IRegistryUsingModule
    {
        public IPropertyModule PropertyModule { get; set; }

        [PrivateComponent]
        [RegistryComponent]
        public IPropertyModule PrivatePropertyModule { get; set; }

        public RegistryUsingModule()
        {
            RegisterPublicComponent<IPropertyModule, PropertyModule>(x => x.PropertyModule);
        }

        public void RegisterPrivateWithRegistryComponentOverlap()
        {
            RegisterPrivateComponent<IPropertyModule, PropertyModule>(x => x.PrivatePropertyModule);
        }

        public void ApplyRegistry()
        {
            RegistryModule registryModule = new RegistryModule();

            registryModule.RegisterModule<ISubModule, Submodule>();
            registryModule.RegisterModule<IPropertyModule, PropertyModule>();

            this.Registry = registryModule;
        }
    }
}
