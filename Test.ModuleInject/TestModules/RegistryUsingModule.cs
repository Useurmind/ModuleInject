using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modularity.Registry;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using System.Runtime.Remoting.Messaging;

    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IRegistryUsingModule : IModule
    {
        IPropertyModule PropertyModule { get; }
    }

    public class RegistryUsingModule : InjectionModule<IRegistryUsingModule, RegistryUsingModule>, IRegistryUsingModule
    {
        public IPropertyModule PropertyModule { get; set; }

        [PrivateComponent]
        public IMainComponent1 MainComponent1 { get; set; }

        [PrivateComponent]
        [RegistryComponent]
        public IPropertyModule PrivatePropertyModule { get; set; }

        [RegistryComponent]
        private IPropertyModule PrivatePrivatePropertyModule { get; set; }

        public RegistryUsingModule()
        {
            RegisterPublicComponent(x => x.PropertyModule).Construct <PropertyModule>();
        }

        public void RegisterComponentFromPublicSubmodule()
        {
            this.RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.PrivatePropertyModule.Component2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterComponentFromPrivateSubmodule()
        {
            this.RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.PrivatePrivatePropertyModule.Component2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPrivateWithRegistryComponentOverlap()
        {
            RegisterPrivateComponent(x => x.PrivatePropertyModule).Construct<PropertyModule>();
        }

        public void ApplyRegistry()
        {
            StandardRegistry registry = new StandardRegistry();

            registry.RegisterModule<ISubModule, Submodule>();
            registry.RegisterModule<IPropertyModule, PropertyModule>();

            this.Registry = registry;
        }
    }
}
