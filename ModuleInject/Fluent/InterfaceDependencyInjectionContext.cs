using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class InterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal DependencyInjectionContext Context { get; private set; }
        internal InterfaceRegistrationContext<IComponent, IModule, TModule> ComponentContext { get; private set; }
        public string DependencyName { get { return Context.DependencyPath; } }

        internal InterfaceDependencyInjectionContext(InterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, 
            DependencyInjectionContext context)
        {
            Context = context;
            ComponentContext = componentContext;
        }
    }

    public class InterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent> : IInterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent>
    {
        internal DependencyInjectionContext Context { get; private set; }
        internal InterfaceRegistrationContext<IComponentBase, IModuleBase> ComponentContext { get; private set; }
        public string DependencyName { get { return Context.DependencyPath; } }

        internal InterfaceDependencyInjectionContext(InterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext,
            DependencyInjectionContext context)
        {
            Context = context;
            ComponentContext = componentContext;
        }
    }
}
