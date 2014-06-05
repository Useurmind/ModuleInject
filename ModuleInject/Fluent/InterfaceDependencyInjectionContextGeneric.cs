using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class InterfaceDependencyInjectionContext<IComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal DependencyInjectionContext Context { get; private set; }
        public InterfaceRegistrationContext<IComponent, IModule, TModule> ComponentContext { get; private set; }
        public string DependencyName { get { return Context.DependencyName; } }

        internal InterfaceDependencyInjectionContext(InterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, 
            DependencyInjectionContext context)
        {
            Context = context;
            ComponentContext = componentContext;
        }
    }
}
