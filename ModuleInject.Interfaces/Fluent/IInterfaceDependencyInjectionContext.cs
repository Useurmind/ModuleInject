using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependencyComponent>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
    }

    public interface IInterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent>
    {
        
    }
}
