using System;
using System.Linq;
using System.Linq.Expressions;

using ModuleInject.Common.Linq;
using ModuleInject.Container.Resolving;
using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Modules.Fluent
{
    internal class ValueInjectionContext : IValueInjectionContext
    {
        private RegistrationContext registrationContext;

        public IRegistrationContext RegistrationContext { get { return registrationContext; } }
        public object Value { get; private set; }
        public Type ValueType { get; private set; }

        public ValueInjectionContext(RegistrationContext registrationContext, object value, Type valueType)
        {
            this.registrationContext = registrationContext;
            this.Value = value;
            this.ValueType = valueType;
        }

        public IRegistrationContext IntoProperty(Expression dependencyTargetExpression)
        {
            var component = this.registrationContext;
            IRegistrationTypes types = component.RegistrationTypes;

            string targetName = Property.Get(dependencyTargetExpression);

            component.Container.InjectProperty(component.RegistrationName,types.IComponent,  
                targetName, new ConstantValue(this.Value, this.ValueType));

            return component;
        }
    }

}
