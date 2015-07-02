using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Context to define how the component should be constructed.
    /// </summary>
    /// <typeparam name="TContext">The type of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component to register.</typeparam>
    public interface IConstructionContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        /// <summary>
        /// Construct the component using the default constructor of the given type.
        /// </summary>
        /// <typeparam name="TComponent">The type of which the default constructor should be used.</typeparam>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IModificationContext<TContext, TIComponent, TComponent> Construct<TComponent>()
            where TComponent : TIComponent, new();

        /// <summary>
        /// Construct the component using the given func.
        /// </summary>
        /// <typeparam name="TComponent">The type that will be returned by the func and be the type of the component.</typeparam>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IModificationContext<TContext, TIComponent, TComponent> Construct<TComponent>(Func<TContext, TComponent> constructInstance)
            where TComponent : TIComponent;
    }
}
