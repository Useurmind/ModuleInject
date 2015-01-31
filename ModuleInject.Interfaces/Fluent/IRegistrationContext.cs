using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    /// <summary>
    /// Interface for a registration context which is created when a component is registered.
    /// </summary>
    public interface IRegistrationContext
    {
        /// <summary>
        /// Gets the types important for the registration represented by this context.
        /// </summary>
        IRegistrationTypes RegistrationTypes { get; }

        /// <summary>
        /// Gets the name part of the key under which the component is registered.
        /// </summary>
        string RegistrationName { get; }

        /// <summary>
        /// Gets if the constructor function top be used to create the component was already registered.
        /// </summary>
        bool WasConstructorCalled { get; }

        /// <summary>
        /// Gets the module by which the registration context was created.
        /// </summary>
        IModule Module { get; }

        /// <summary>
        /// For resolving the registration construct nothing, use the given instance.
        /// </summary>
        /// <param name="instance">The instance to use.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext Construct(object instance);

        /// <summary>
        /// For resolving the registration construct an instance of the given type.
        /// </summary>
        /// <param name="componentType">The type that should be constructed.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext Construct(Type componentType);

        /// <summary>
        /// For resolving the registration construct an instance by executing the given expression.
        /// The expression should have the type Expression<Func<TModule, TComponent>, e.g.
        /// (MyModule mod) => new MyComponent()
        /// </summary>
        /// <param name="constructorCallExpression">The expression to execute.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext Construct(LambdaExpression constructorCallExpression);

        /// <summary>
        /// Starts the registration of a property injection. The given parameters descibe the value
        /// to be injected.
        /// </summary>
        /// <param name="value">The actual value.</param>
        /// <param name="valueType">The type as which the value should be treated.</param>
        /// <returns>A context that can be used to define where the value should be injected.</returns>
        IValueInjectionContext Inject(object value, Type valueType);

        /// <summary>
        /// Starts the registration of a property injection. The given expression describes the source
        /// for the value that should be injected into the property.
        /// It should have the type Expression<Func<TModule, TDependency>>, e.g.
        /// (MyModule mod) => mod.OtherComponent
        /// </summary>
        /// <param name="dependencySourceExpression">The expression that describes the value to inject.</param>
        /// <returns>A context that can be used to define where the value should be injected.</returns>
        IDependencyInjectionContext InjectSource(LambdaExpression dependencySourceExpression);

        /// <summary>
        /// When resolving the registration execute the given expression to inject something.
        /// The expression should have the type Expression<Action<TComponent, TModule>>, e.g.
        /// (MyComponent comp, MyModule mod) => comp.Method1(mod.OtherComponent)
        /// </summary>
        /// <param name="methodCallExpression">An expression that calls a method on the component.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext Inject(LambdaExpression methodCallExpression);

        /// <summary>
        /// Declares that this registration should be used to also resolve another property of the same module.
        /// The expression should have the type Expression<Func<TModule, IComponent2>>, e.g.
        /// (MyModule mod) => mod.Component2
        /// </summary>
        /// <param name="moduleProperty">The module property that should also be resolved using this registration.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext AlsoRegisterFor(Expression moduleProperty);

        /// <summary>
        /// Adds an action that will be executed when this registration is resolved.
        /// </summary>
        /// <typeparam name="T">The type of the resolved component.</typeparam>
        /// <param name="customAction">The action that will be executed for the component.</param>
        /// <returns>The registration context itself.</returns>
        IRegistrationContext AddCustomAction<T>(Action<T> customAction);
    }

    public interface IOuterRegistrationContext
    {
        IRegistrationContext Context { get; }
    }
}
