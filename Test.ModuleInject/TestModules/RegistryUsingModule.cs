using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using System.Runtime.Remoting.Messaging;

    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Fluent;
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
        public IMainComponent1 MainComponent1 { get; set; }

        [PrivateComponent]
        [RegistryComponent]
        public IPropertyModule PrivatePropertyModule { get; set; }

        [RegistryComponent]
        private IPropertyModule PrivatePrivatePropertyModule { get; set; }

        public RegistryUsingModule()
        {
            RegisterPublicComponent<IPropertyModule, PropertyModule>(x => x.PropertyModule);
        }

        public void RegisterComponentFromPublicSubmodule()
        {
            this.RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .Inject(x => x.PrivatePropertyModule.Component2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterComponentFromPrivateSubmodule()
        {
            this.RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .Inject(x => x.PrivatePrivatePropertyModule.Component2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPrivateWithRegistryComponentOverlap()
        {
            RegisterPrivateComponent<IPropertyModule, PropertyModule>(x => x.PrivatePropertyModule);
        }

        public void ApplyRegistry()
        {
            Registry registry = new Registry();

            registry.RegisterModule<ISubModule, Submodule>();
            registry.RegisterModule<IPropertyModule, PropertyModule>();

            this.Registry = registry;
        }
    }
}
