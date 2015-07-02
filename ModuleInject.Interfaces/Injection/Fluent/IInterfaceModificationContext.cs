using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// This interface is used for injectors that match any interface implemented by context and component.
    /// Allowing any interfaces of the component disallows for changing instances because the returned type is not necessarily correct.
    /// The interface the component is registered with does not necessarily match <see cref="TIComponent"/>.
    /// Therefore, you could easily return components with wrong types.
    /// </summary>
    /// <typeparam name="TIContext">One of the interfaces of the context.</typeparam>
    /// <typeparam name="TIComponent">One of the interfaces of the component</typeparam>
	public interface IInterfaceModificationContext<TIContext, TIComponent> : IWrapInjectionRegister
    {
        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IInterfaceModificationContext<TIContext, TIComponent> Inject(Action<TIContext, TIComponent> injectInInstance);

        /// <returns>A context for performing the next steps in fluent registration.</returns>
        IInterfaceModificationContext<TIContext, TIComponent> AddMeta<T>(T metaData);
	}
}
