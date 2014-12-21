using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class InterfaceValueInjectionContext<IComponent, IModule, TModule, TValue> : IInterfaceValueInjectionContext<IComponent, IModule, TModule, TValue>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        internal ValueInjectionContext Context { get; private set; }

        internal InterfaceRegistrationContext<IComponent, IModule, TModule> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal InterfaceValueInjectionContext(InterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, ValueInjectionContext context)
        {
            this.ComponentContext = componentContext;
            this.Context = context;
        }
    }

    internal class InterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue> : IInterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue>
    {
        internal ValueInjectionContext Context { get; private set; }

        internal InterfaceRegistrationContext<IComponentBase, IModuleBase> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)this.Context.Value; } }

        internal InterfaceValueInjectionContext(InterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext, ValueInjectionContext context)
        {
            this.ComponentContext = componentContext;
            this.Context = context;
        }
    }
}
