using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInjectionRegister
	{
	}

	public interface IInject<TContext, TComponent>
	{
		void Inject(Action<TContext, TComponent> injectInInstance);
	}

	public interface IChange<TContext, TIComponent>
	{
		void Change(Func<TContext, TIComponent, TIComponent> changeInstance);
	}

	public interface IInjectionRegister<TContext, TIComponent, TComponent> : IInjectionRegister
	{
		void SetContext(TContext context);

		void Construct(Func<TContext, TComponent> constructInstance);

		void Inject(Action<TContext, TComponent> injectInInstance);

		void Change(Func<TContext, TIComponent, TIComponent> changeInstance);

		TIComponent CreateInstance();
	}
}
