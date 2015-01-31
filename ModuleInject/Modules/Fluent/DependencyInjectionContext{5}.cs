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
        public IDependencyInjectionContext Context { get; private set; }
        public IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }

        internal DependencyInjectionContext(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, 
            IDependencyInjectionContext context)
        {
            this.Context = context;
            this.ComponentContext = componentContext;
        }
    }
}
