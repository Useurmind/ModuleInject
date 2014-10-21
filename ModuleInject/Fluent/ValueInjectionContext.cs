using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Linq;

    internal class ValueInjectionContext
    {
        public ComponentRegistrationContext ComponentContext { get; set; }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }

        public ValueInjectionContext(ComponentRegistrationContext componentContext, object value, Type valueType)
        {
            ComponentContext = componentContext;
            Value = value;
            ValueType = valueType;
        }

        public ComponentRegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            ComponentRegistrationContext component = this.ComponentContext;
            ComponentRegistrationTypes types = component.Types;

            string targetName = Property.Get(dependencyTargetExpression);

            component.Container.RegisterType(types.IComponent, types.TComponent, component.ComponentName,
                new InjectionProperty(targetName, Value)
            );

            return component;
        }
    }

}
