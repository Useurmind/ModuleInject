using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class InterfaceInjector<IComponent, IModule, TModule> :
        IInterfaceInjector<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> _injectInto;

        public InterfaceInjector(Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(IInterfaceRegistrationContext<IComponent, IModule, TModule> context)
        {
            _injectInto(context);
        }
    }

    public class InterfaceInjector<IComponentBase, IModuleBase> :
        IInterfaceInjector<IComponentBase, IModuleBase>
    {
        private Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> _injectInto;

        public InterfaceInjector(Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
        {
            _injectInto = injectInto;
        }

        public void InjectInto(IInterfaceRegistrationContext<IComponentBase, IModuleBase> context)
        {
            _injectInto(context);
        }
    }
}
