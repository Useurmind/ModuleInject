using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// This is the very first context class used to state how instances are created (e.g. factory, single instance).
    /// </summary>
    /// <typeparam name="TContext">The type of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component to register.</typeparam>
    public interface IInstantiationStrategyContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        /// <summary>
        /// State the instantiation strategy.
        /// </summary>
        /// <param name="instantiationStrategy">The instantiation strategy.</param>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IDisposeStrategyContext<TContext, TIComponent> InstantiateWith(IInstantiationStrategy<TIComponent> instantiationStrategy);
    }
}
