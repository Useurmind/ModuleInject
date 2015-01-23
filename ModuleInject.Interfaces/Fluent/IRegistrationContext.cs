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
        IInjectionModule Module { get; }

        IRegistrationContext Construct(object instance);

        IRegistrationContext Construct(Type componentType);

        IRegistrationContext Construct(LambdaExpression constructorCallExpression);
    }

    public interface IRegistrationContextT
    {
        IRegistrationContext ReflectionContext { get; }
    }
}
