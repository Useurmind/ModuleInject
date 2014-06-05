using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInterfaceInjector<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        void InjectInto(InterfaceRegistrationContext<IComponent, IModule, TModule> context);
    }
}
