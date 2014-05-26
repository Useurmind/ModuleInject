using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public interface IInjector<IComponent, TComponent, IModule>
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        void InjectInto(ComponentRegistrationContext<IComponent, TComponent, IModule> context);
    }
}
