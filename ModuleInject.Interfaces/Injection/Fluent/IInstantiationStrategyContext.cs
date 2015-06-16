using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IInstantiationStrategyContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        IDisposeStrategyContext<TContext, TIComponent> InstantiateWith(IInstantiationStrategy<TIComponent> instantiationStrategy);
    }
}
