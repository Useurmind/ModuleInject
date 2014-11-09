using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    internal class DependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent> : IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        internal DependencyInjectionContext Context { get; private set; }
        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }
        public string DependencyName { get { return Context.DependencyPath; } }

        internal DependencyInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, 
            DependencyInjectionContext context)
        {
            Context = context;
            ComponentContext = componentContext;
        }
    }
}
