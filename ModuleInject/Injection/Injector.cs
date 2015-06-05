using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInterfaceInjector<TIContext, TIComponent>
	{
		void InjectInto(IInterfaceInjectionRegister<TIContext, TIComponent> injectionRegister);
    }

    public interface IExactInterfaceInjector<TIContext, TIComponent>
    {
        void InjectInto(IExactInterfaceInjectionRegister<TIContext, TIComponent> injectionRegister);
    }

    public class InterfaceInjector<TIContext, TIComponent> : IInterfaceInjector<TIContext, TIComponent>
	{
		private Action<IInterfaceInjectionRegister<TIContext, TIComponent>> _injectInto;

		public InterfaceInjector(Action<IInterfaceInjectionRegister<TIContext, TIComponent>> injectInto)
		{
			this._injectInto = injectInto;
		}

		public void InjectInto(IInterfaceInjectionRegister<TIContext, TIComponent> context)
		{
			this._injectInto(context);
		}
	}

    public class ExactInterfaceInjector<TIContext, TIComponent> : IExactInterfaceInjector<TIContext, TIComponent>
    {
        private Action<IExactInterfaceInjectionRegister<TIContext, TIComponent>> _injectInto;

        public ExactInterfaceInjector(Action<IExactInterfaceInjectionRegister<TIContext, TIComponent>> injectInto)
        {
            this._injectInto = injectInto;
        }

        public void InjectInto(IExactInterfaceInjectionRegister<TIContext, TIComponent> context)
        {
            this._injectInto(context);
        }

    }
}
