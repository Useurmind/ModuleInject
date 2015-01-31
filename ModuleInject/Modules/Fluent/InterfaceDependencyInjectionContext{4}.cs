using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public class InterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        public IDependencyInjectionContext Context { get; private set; }
        public IInterfaceRegistrationContext<IComponent, IModule, TModule> RegistrationContext { get; private set; }

        internal InterfaceDependencyInjectionContext(IInterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, 
            IDependencyInjectionContext context)
        {
            this.Context = context;
            this.RegistrationContext = componentContext;
        }
    }

    public class InterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent>
    {
        public IDependencyInjectionContext Context { get; private set; }
        public IInterfaceRegistrationContext<IComponentBase, IModuleBase> RegistrationContext { get; private set; }

        internal InterfaceDependencyInjectionContext(IInterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext,
            IDependencyInjectionContext context)
        {
            this.Context = context;
            this.RegistrationContext = componentContext;
        }
    }
}
