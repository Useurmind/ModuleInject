using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IInterfaceDependencyInjectionContext<IComponent, IModule, TModule, TDependencyComponent> : IOuterDependencyInjectionContext
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        IInterfaceRegistrationContext<IComponent, IModule, TModule> RegistrationContext { get; }
    }

    public interface IInterfaceDependencyInjectionContext<IComponentBase, IModuleBase, IDependencyComponent> : IOuterDependencyInjectionContext
    {
        IInterfaceRegistrationContext<IComponentBase, IModuleBase> RegistrationContext { get; }
    }
}
