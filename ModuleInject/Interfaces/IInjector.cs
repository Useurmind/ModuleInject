using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        void InjectInto(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context);
    }
}
