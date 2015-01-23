using System;
using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    public class InterfaceInjector<IComponent, IModule, TModule> :
        IInterfaceInjector<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        private Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> _injectInto;

        public InterfaceInjector(Action<IInterfaceRegistrationContext<IComponent, IModule, TModule>> injectInto)
        {
            this._injectInto = injectInto;
        }

        public void InjectInto(IInterfaceRegistrationContext<IComponent, IModule, TModule> context)
        {
            this._injectInto(context);
        }
    }

    public class InterfaceInjector<IComponentBase, IModuleBase> :
        IInterfaceInjector<IComponentBase, IModuleBase>
    {
        private Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> _injectInto;

        public InterfaceInjector(Action<IInterfaceRegistrationContext<IComponentBase, IModuleBase>> injectInto)
        {
            this._injectInto = injectInto;
        }

        public void InjectInto(IInterfaceRegistrationContext<IComponentBase, IModuleBase> context)
        {
            this._injectInto(context);
        }
    }
}
