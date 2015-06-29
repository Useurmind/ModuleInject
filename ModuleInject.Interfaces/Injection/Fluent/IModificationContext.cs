using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    public interface IModificationContext<TContext, TIComponent, TComponent> : IWrapInjectionRegister, ISourceOf<TIComponent>
            where TComponent : TIComponent
    {
        IModificationContext<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance);

        IModificationContext<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance);

        IModificationContext<TContext, TIComponent, TComponent> AddMeta<T>(T metaData);
    }
}
