using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IDisposeStrategyContext<TContext, TIComponent> : IConstructionContext<TContext, TIComponent>
    {
        IConstructionContext<TContext, TIComponent> DisposeWith(IDisposeStrategy disposeStrategy);
    }
}
