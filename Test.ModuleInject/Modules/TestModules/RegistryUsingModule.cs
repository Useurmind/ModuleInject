using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Injection;
using ModuleInject.Interfaces;
using ModuleInject.Modularity.Registry;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IRegistryUsingModule : IModule
    {
        IPropertyModule PropertyModule { get; }
    }

    public class RegistryUsingModule : InjectionModule<RegistryUsingModule>, IRegistryUsingModule
    {
        public IPropertyModule PropertyModule { get { return GetSingleInstance<PropertyModule>(); } }
        
        public IMainComponent1 MainComponent1 { get { return Get<IMainComponent1>(); } }
        
        [FromRegistry]
        public IPropertyModule PrivatePropertyModule { get; set; }

        [FromRegistry]
        private IPropertyModule PrivatePrivatePropertyModule { get; set; }
        
        public void RegisterComponentFromPublicSubmodule()
        {
            this.SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((m,c ) => c.MainComponent2 = m.PrivatePropertyModule.Component2);
        }

        public void RegisterComponentFromPrivateSubmodule()
        {
            this.SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = m.PrivatePrivatePropertyModule.Component2);
        }

        public void RegisterPrivateWithRegistryComponentOverlap()
        {
            this.PrivatePropertyModule = new PropertyModule();
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
