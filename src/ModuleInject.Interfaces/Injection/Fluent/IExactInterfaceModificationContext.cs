using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// This interface is used in injectors that match any interface of the context but match exactly the interface with
    /// which the component is registered in the context.
    /// This allows for exchaning the instance with a new instance of the correct interface.
    /// </summary>
    /// <typeparam name="TIContext">One of the interfaces of the context.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component with which it is registered in the context.</typeparam>
    public interface IExactInterfaceModificationContext<TIContext, TIComponent> : IWrapInjectionRegister
    {
        /// <summary>
        /// Perform injection into a component.
        /// </summary>
        /// <param name="injectInInstance">An action that will perform the injection.</param>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IExactInterfaceModificationContext<TIContext, TIComponent> Inject(Action<TIContext, TIComponent> injectInInstance);

        /// <summary>
        /// Change the actual instance that is returned (e.g. for decoration).
        /// </summary>
        /// <param name="changeInstance">The func that will perform the instance switch.</param>
        /// <returns>A context for performing the
        IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIContext, TIComponent, TIComponent> changeInstance);

        /// <summary>
        /// Change the actual instance that is returned (e.g. for decoration).
        /// </summary>
        /// <param name="changeInstance">The func that will perform the instance switch.</param>
        /// <returns>A context for performing the
        IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIComponent, TIComponent> changeInstance);

        /// <summary>
        /// Add a meta data tag to the component registration.
        /// This meta data can for example be used to differentiate instances in a registration hook.
        /// Can be retrieved from <see cref="IInjectionRegister "/> instances.
        /// </summary>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IExactInterfaceModificationContext<TIContext, TIComponent> AddMeta<T>(T metaData);
    }
}
