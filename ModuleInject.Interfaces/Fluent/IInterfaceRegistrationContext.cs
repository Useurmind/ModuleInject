using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceRegistrationContext<IComponent, IModuleBase, TModule>
        where TModule : IModuleBase
        where IModuleBase : IInjectionModule
    {
    }

    public interface IInterfaceRegistrationContext<IComponentBase, IModuleBase>
    {
        
    }
}
