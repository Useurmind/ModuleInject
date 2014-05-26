using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    public class ValueInjectionContext<IComponent, TComponent, IModule, TValue>
        where TComponent : IComponent
    {
        public ComponentRegistrationContext<IComponent, TComponent, IModule> ComponentContext { get; set; }
        public TValue Value { get; private set; }

        public ValueInjectionContext(ComponentRegistrationContext<IComponent, TComponent, IModule> componentContext, TValue value)
        {
            ComponentContext = componentContext;
            Value = value;
        }
    }
}
