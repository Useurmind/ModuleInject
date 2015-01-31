using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IValueInjectionContext
    {
        /// <summary>
        /// Gets the registration context from which this context originated.
        /// </summary>
        IRegistrationContext RegistrationContext { get; }

        /// <summary>
        /// Gets the value that should be injected.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Get the type of the value that should be injected.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Describes into which property a value should be injected.
        /// The expression should have the type Expression<Func<TComponent, TProperty>>, e.g.
        /// (MyComponent comp) => comp.Property1
        /// </summary>
        /// <param name="dependencyTargetExpression">The expression describing the property into which should be injected.</param>
        /// <returns>The registration context.</returns>
        IRegistrationContext IntoProperty(Expression dependencyTargetExpression);
    }

    public interface IOuterValueInjectionContext
    {
        IValueInjectionContext Context { get; }
    }
}
