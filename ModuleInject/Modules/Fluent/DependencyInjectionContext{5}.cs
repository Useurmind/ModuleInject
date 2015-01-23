using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class DependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent> : IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : Interfaces.IModule
    {
        internal DependencyInjectionContext Context { get; private set; }
        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }

        internal DependencyInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, 
            DependencyInjectionContext context)
        {
            this.Context = context;
            this.ComponentContext = componentContext;
        }
    }
}
