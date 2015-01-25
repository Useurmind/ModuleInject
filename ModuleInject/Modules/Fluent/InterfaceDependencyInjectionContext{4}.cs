using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public class InterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        internal IDependencyInjectionContext Context { get; private set; }
        internal InterfaceRegistrationContext<IComponent, IModule, TModule> ComponentContext { get; private set; }

        internal InterfaceDependencyInjectionContext(InterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, 
            IDependencyInjectionContext context)
        {
            this.Context = context;
            this.ComponentContext = componentContext;
        }
    }

    public class InterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent>
    {
        internal IDependencyInjectionContext Context { get; private set; }
        internal InterfaceRegistrationContext<IComponentBase, IModuleBase> ComponentContext { get; private set; }

        internal InterfaceDependencyInjectionContext(InterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext,
            IDependencyInjectionContext context)
        {
            this.Context = context;
            this.ComponentContext = componentContext;
        }
    }
}
