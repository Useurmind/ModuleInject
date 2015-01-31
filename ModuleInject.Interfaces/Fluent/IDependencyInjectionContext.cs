using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    public interface IDependencyInjectionContext
    {
        /// <summary>
        /// Describes into which property another component should be injected.
        /// The expression should have the type Expression<Func<TComponent, TProperty>>, e.g.
        /// (MyComponent comp) => comp.Property1
        /// </summary>
        /// <param name="dependencyTargetExpression">The expression describing the property into which should be injected.</param>
        /// <returns>The registration context.</returns>
        IRegistrationContext IntoProperty(LambdaExpression dependencyTargetExpression);
    }

    public interface IOuterDependencyInjectionContext
    {
        IDependencyInjectionContext Context { get; }
    }
}
