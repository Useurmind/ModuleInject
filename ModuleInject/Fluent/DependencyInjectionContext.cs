using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class DependencyInjectionContext<IComponent, TComponent, IModule, IDependencyComponent>
        where TComponent : IComponent
    {
        public ComponentRegistrationContext<IComponent, TComponent, IModule> ComponentContext { get; internal set; }
        public string DependencyName { get; internal set; }

        public DependencyInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule> componentContext, string name)
        {
            DependencyName = name;
            ComponentContext = componentContext;
        }
    }
}
