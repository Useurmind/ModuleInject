using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// A fluent context to perform further steps after construction has been defined.
    /// </summary>
    /// <typeparam name="TContext">The type of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component to register.</typeparam>
    /// <typeparam name="TComponent">The type of the component to register.</typeparam>
    public interface IModificationContext<TContext, TIComponent, TComponent> : IWrapInjectionRegister, ISourceOf<TIComponent>
            where TComponent : TIComponent
    {
        /// <summary>
        /// Change the actual instance that is returned (e.g. for decoration).
        /// </summary>
        /// <param name="changeInstance">The func that will perform the instance switch.</param>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IModificationContext<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance);

        /// <summary>
        /// Perform injection into a component.
        /// </summary>
        /// <param name="injectInInstance">An action that will perform the injection.</param>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IModificationContext<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance);

        /// <summary>
        /// Add a meta data tag to the component registration.
        /// This meta data can for example be used to differentiate instances in a registration hook.
        /// Can be retrieved from <see cref="IInjectionRegister "/> instances.
        /// </summary>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IModificationContext<TContext, TIComponent, TComponent> AddMeta<T>(T metaData);
    }
}
