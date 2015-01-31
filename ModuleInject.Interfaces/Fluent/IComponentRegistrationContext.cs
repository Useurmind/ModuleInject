using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> : IOuterRegistrationContext
        where TModule : IModule
        where TComponent : IComponent
        where IModule : Interfaces.IModule
    {
    }
}
