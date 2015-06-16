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
        IExactInterfaceModificationContext<TIContext, TIComponent> Inject(Action<TIContext, TIComponent> injectInInstance);

        IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIContext, TIComponent, TIComponent> changeInstance);

        IExactInterfaceModificationContext<TIContext, TIComponent> Change(Func<TIComponent, TIComponent> changeInstance);

        IExactInterfaceModificationContext<TIContext, TIComponent> AddMeta<T>(T metaData);
    }
}
