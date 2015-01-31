using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceRegistrationContext<IComponent, IModuleBase, TModule> : IOuterRegistrationContext
        where TModule : IModuleBase
        where IModuleBase : IModule
    {
    }

    public interface IInterfaceRegistrationContext<IComponentBase, IModuleBase> : IOuterRegistrationContext
    {

    }
}
