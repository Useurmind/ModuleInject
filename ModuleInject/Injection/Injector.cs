using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    public class InterfaceInjector<TIContext, TIComponent> : IInterfaceInjector<TIContext, TIComponent>
	{
		private Action<IInterfaceModificationContext<TIContext, TIComponent>> _injectInto;

		public InterfaceInjector(Action<IInterfaceModificationContext<TIContext, TIComponent>> injectInto)
		{
			this._injectInto = injectInto;
		}

		public void InjectInto(IInterfaceModificationContext<TIContext, TIComponent> context)
		{
			this._injectInto(context);
		}
	}

    public class ExactInterfaceInjector<TIContext, TIComponent> : IExactInterfaceInjector<TIContext, TIComponent>
    {
        private Action<IExactInterfaceModificationContext<TIContext, TIComponent>> _injectInto;

        public ExactInterfaceInjector(Action<IExactInterfaceModificationContext<TIContext, TIComponent>> injectInto)
        {
            this._injectInto = injectInto;
        }

        public void InjectInto(IExactInterfaceModificationContext<TIContext, TIComponent> context)
        {
            this._injectInto(context);
        }

    }
}
