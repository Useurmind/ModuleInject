using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IConstructionContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        IModificationContext<TContext, TIComponent, TComponent> Construct<TComponent>()
            where TComponent : TIComponent, new();

        IModificationContext<TContext, TIComponent, TComponent> Construct<TComponent>(Func<TContext, TComponent> constructInstance)
            where TComponent : TIComponent;
    }
}
