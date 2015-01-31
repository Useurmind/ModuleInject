using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IValueInjectionContext<IComponent, TComponent, IModule, TModule, TDependency> : IOuterValueInjectionContext
        where TComponent : IComponent
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; }
    }
}
