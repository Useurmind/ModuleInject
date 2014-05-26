using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class Injector<IComponent, TComponent, IModule> : IInjector<IComponent, TComponent, IModule>
        where TComponent : IComponent
        where IModule : IInjectionModule
    {
        private Action<ComponentRegistrationContext<IComponent, TComponent, IModule>> _injectInto;

        public Injector(Action<ComponentRegistrationContext<IComponent, TComponent, IModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(ComponentRegistrationContext<IComponent, TComponent, IModule> context)
        {
            _injectInto(context);
        }
    }
}
