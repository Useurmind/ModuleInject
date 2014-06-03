using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class DependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; internal set; }
        public string DependencyName { get; internal set; }

        public DependencyInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, string name)
        {
            DependencyName = name;
            ComponentContext = componentContext;
        }
    }
}
