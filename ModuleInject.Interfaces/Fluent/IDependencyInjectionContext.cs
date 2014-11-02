using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IDependencyInjectionContext<IComponent, TComponent, IModule, TModule, IDependencyComponent>
        where TModule : IModule
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
    }
}
