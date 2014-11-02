using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInstanceDependencyInjectionContext<IComponent, TComponent, IModule, TModule, TProperty>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
    }
}
