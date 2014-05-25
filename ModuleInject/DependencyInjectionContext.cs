using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject
{
    public class DependencyInjectionContext<IComponent, TComponent, IModule, IDependencyComponent>
        where TComponent : IComponent
    {
        public ComponentRegistrationContext<IComponent, TComponent, IModule> ComponentContext { get; set; }
        public string DependencyName { get; set; }
    }
}
