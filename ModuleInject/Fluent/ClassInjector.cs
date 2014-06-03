using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class ClassInjector<IComponent, TComponent, IModule, TModule> :
        IClassInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> _injectInto;

        public ClassInjector(Action<ComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context)
        {
            _injectInto(context);
        }
    }
}
