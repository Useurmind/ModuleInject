using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// This is the second context class that can be used to state a dispose strategy.
    /// </summary>
    /// <typeparam name="TContext">The type of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component to register.</typeparam>
    public interface IDisposeStrategyContext<TContext, TIComponent> : IConstructionContext<TContext, TIComponent>
    {
        /// <summary>
        /// State the dispose strategy.
        /// </summary>
        /// <param name="disposeStrategy">The dispose strategy.</param>
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IConstructionContext<TContext, TIComponent> DisposeWith(IDisposeStrategy disposeStrategy);
    }
}
