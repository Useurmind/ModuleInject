using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class ClassInjector<IComponent, TComponent, IModule, TModule> :
        IClassInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> _injectInto;

        public ClassInjector(Action<IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context)
        {
            _injectInto(context);
        }
    }
}
