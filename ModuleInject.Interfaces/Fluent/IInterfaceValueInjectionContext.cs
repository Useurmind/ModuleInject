using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceValueInjectionContext<IComponent, IModule, TModule, TValue>
        where TModule : IModule
        where IModule : IInjectionModule
    {
    }

    public interface IInterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue>
    {
        
    }
}
