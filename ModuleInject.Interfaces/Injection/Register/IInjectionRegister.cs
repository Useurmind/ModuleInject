using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Interface for the core class of a module.
    /// An injection register saves all injection steps and information regarding a registration process of a component.
    /// It is so to say a single component IoC container.
    /// Not strongly typed. Direct usage discouraged, except for usage in dynamic <see cref="IRegistrationHook" /> implementations.
    /// </summary>
    public interface IInjectionRegister : IDisposable
    {
        /// <summary>
        /// The name of the component.
        /// </summary>
        string ComponentName { get; }

        /// <summary>
        /// The interface under which the component is registered.
        /// </summary>
        Type ComponentInterface { get; }

        /// <summary>
        /// The actual type of the component instances.
        /// </summary>
        Type ComponentType { get; set; }

        /// <summary>
        /// The type of the context/module.
        /// </summary>
        Type ContextType { get; }

        /// <summary>
        /// Get a list of all meta data associated with the component.
        /// </summary>
        IEnumerable<object> MetaData { get; }

        /// <summary>
        /// Get the instantiation strategy for the component.
        /// </summary>
        /// <returns>The instantiation strategy.</returns>
        IInstantiationStrategy GetInstantiationStrategy();

        /// <summary>
        /// Get the dipose strategy for the component.
        /// </summary>
        /// <returns>The dispose strategy.</returns>
        IDisposeStrategy GetDisposeStrategy();

        /// <summary>
        /// Set the context/module.
        /// </summary>
        /// <param name="context">The context/module.</param>
        void SetContext(object context);

        /// <summary>
        /// Set the instantation strategy.
        /// </summary>
        /// <param name="instantiationStrategy">The instantation strategy.</param>
        void InstantiationStrategy(IInstantiationStrategy instantiationStrategy);

        /// <summary>
        /// Set the dispose strategy.
        /// </summary>
        /// <param name="disposeStrategy">The dispose strategy.</param>
        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        /// <summary>
        /// Set the construct function.
        /// </summary>
        /// <param name="constructInstance"></param>
        void Construct(Func<object, object> constructInstance);

        void Inject(Action<object, object> injectInInstance);

        void Change(Func<object, object, object> changeInstance);

        void Change(Func<object, object> changeInstance);

        /// <summary>
        /// Add meta data to the component registration.
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        void AddMeta(object metaData);

        /// <summary>
        /// Add a action that is performed when an instance of the component is resolved.
        /// </summary>
        /// <param name="resolveHandler">The handler that is performed.</param>
        void OnResolve(Action<ObjectResolvedContext> resolveHandler);

        /// <summary>
        /// Get an instance of the component.
        /// </summary>
        /// <returns>The instance.</returns>
        object GetInstance();
    }

    public interface IInjectionRegister<TContext, TIComponent, TComponent> : IWrapInjectionRegister
        where TComponent : TIComponent
    {
        void SetContext(TContext context);

        void InstantiationStrategy(IInstantiationStrategy<TIComponent> instantiationStrategy);

        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        void Construct(Func<TContext, TComponent> constructInstance);

        void Inject(Action<TContext, TComponent> injectInInstance);

        void Change(Func<TContext, TIComponent, TIComponent> changeInstance);

        void Change(Func<TIComponent, TIComponent> changeInstance);

        void AddMeta<T>(T metaData);

        TIComponent GetInstance();
    }
}
