using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class InterfaceValueInjectionContext<IComponent, IModule, TModule, TValue> : IInterfaceValueInjectionContext<IComponent, IModule, TModule, TValue>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal ValueInjectionContext Context { get; private set; }

        public InterfaceRegistrationContext<IComponent, IModule, TModule> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)Context.Value; } }

        internal InterfaceValueInjectionContext(InterfaceRegistrationContext<IComponent, IModule, TModule> componentContext, ValueInjectionContext context)
        {
            ComponentContext = componentContext;
            Context = context;
        }
    }

    public class InterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue> : IInterfaceValueInjectionContext<IComponentBase, IModuleBase, TValue>
    {
        internal ValueInjectionContext Context { get; private set; }

        public InterfaceRegistrationContext<IComponentBase, IModuleBase> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)Context.Value; } }

        internal InterfaceValueInjectionContext(InterfaceRegistrationContext<IComponentBase, IModuleBase> componentContext, ValueInjectionContext context)
        {
            ComponentContext = componentContext;
            Context = context;
        }
    }
}
