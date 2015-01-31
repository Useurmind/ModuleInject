using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class InterfaceValueInjectionContext<IComponent, IModule, TModule, TValue> : IInterfaceValueInjectionContext<IComponent, IModule, TModule, TValue>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        public IValueInjectionContext Context { get; private set; }

        public IInterfaceRegistrationContext<IComponent, IModule, TModule> RegistrationContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal InterfaceValueInjectionContext(IInterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, IValueInjectionContext context)
        {
            this.RegistrationContext = componentContext;
            this.Context = context;
        }
    }

    internal class InterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue> : IInterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue>
    {
        public IValueInjectionContext Context { get; private set; }

        public IInterfaceRegistrationContext<IComponentBase, IModuleBase> RegistrationContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal InterfaceValueInjectionContext(IInterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext, IValueInjectionContext context)
        {
            this.RegistrationContext = componentContext;
            this.Context = context;
        }
    }
}
