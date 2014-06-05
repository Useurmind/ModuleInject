using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class InterfaceInjector<IComponent, IModule, TModule> :
        IInterfaceInjector<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<InterfaceRegistrationContext<IComponent, IModule, TModule>> _injectInto;

        public InterfaceInjector(Action<InterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(InterfaceRegistrationContext<IComponent, IModule, TModule> context)
        {
            _injectInto(context);
        }
    }
}
