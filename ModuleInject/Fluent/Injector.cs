using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class Injector<IComponent, TComponent, IModule, TModule> : IInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> _injectInto;

        public Injector(Action<ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context)
        {
            _injectInto(context);
        }
    }
}
