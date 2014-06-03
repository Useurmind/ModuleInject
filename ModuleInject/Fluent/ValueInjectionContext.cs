using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Fluent
{
    internal class ValueInjectionContext
    {
        public ComponentRegistrationContext ComponentContext { get; set; }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }

        public ValueInjectionContext(ComponentRegistrationContext componentContext, object value, Type valueType)
        {
            ComponentContext = componentContext;
            Value = value;
        }
    }
}
