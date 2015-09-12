using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    /// <summary>
    /// This injector is for cases where the module and component only need to implement the given interfaces.
    /// They component does not need to be registered with the exact interface given.
    /// Therefore, it is not possible to change the instance.
    /// </summary>
    /// <typeparam name="TIContext">The interface of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component.</typeparam>
    public class InterfaceInjector<TIContext, TIComponent> : IInterfaceInjector<TIContext, TIComponent>
	{
		private Action<IInterfaceModificationContext<TIContext, TIComponent>> _injectInto;

		public InterfaceInjector(Action<IInterfaceModificationContext<TIContext, TIComponent>> injectInto)
		{
			this._injectInto = injectInto;
		}

        /// <inheritdoc />
        public void InjectInto(IInterfaceModificationContext<TIContext, TIComponent> context)
		{
			this._injectInto(context);
		}
	}

    /// <summary>
    /// This injector is for cases where the component is registered under the exact interface stated here.
    /// Therefore, it is possible to change the instance.
    /// </summary>
    /// <typeparam name="TIContext">The interface of the module.</typeparam>
    /// <typeparam name="TIComponent">The interface of the component.</typeparam>
    public class ExactInterfaceInjector<TIContext, TIComponent> : IExactInterfaceInjector<TIContext, TIComponent>
    {
        private Action<IExactInterfaceModificationContext<TIContext, TIComponent>> _injectInto;

        public ExactInterfaceInjector(Action<IExactInterfaceModificationContext<TIContext, TIComponent>> injectInto)
        {
            this._injectInto = injectInto;
        }

        /// <inheritdoc />
        public void InjectInto(IExactInterfaceModificationContext<TIContext, TIComponent> context)
        {
            this._injectInto(context);
        }

    }
}
