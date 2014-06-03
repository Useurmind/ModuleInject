using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class ValueInjectionContext<IComponent, TComponent, IModule, TModule, TValue>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : IInjectionModule
    {
        public ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> ComponentContext { get; set; }
        public TValue Value { get; private set; }

        public ValueInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule, TModule> componentContext, TValue value)
        {
            ComponentContext = componentContext;
            Value = value;
        }
    }
}
