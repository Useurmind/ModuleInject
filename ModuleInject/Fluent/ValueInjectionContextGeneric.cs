using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;

    public class ValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue> : IValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        internal ValueInjectionContext Context { get; private set; }

        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; private set; }
        public TValue Value { get { return (TValue)Context.Value; } }

        internal ValueInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, ValueInjectionContext context)
        {
            ComponentContext = componentContext;
            Context = context;
        }
    }
}
