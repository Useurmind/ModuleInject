using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceValueInjectionContext<IComponent, IModule, TModule, TValue> : IOuterValueInjectionContext
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        IInterfaceRegistrationContext<IComponent, IModule, TModule> RegistrationContext { get; }
    }

    public interface IInterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue> : IOuterValueInjectionContext
    {
        IInterfaceRegistrationContext<IComponentBase, IModuleBase> RegistrationContext { get; }
    }
}
