using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInjectionRegister
	{
	}

	public interface IInjectionRegister<TContext, TIComponent, TComponent> : IInjectionRegister
	{
		IInjectionRegister<TContext, TIComponent, TComponent> SetContext(TContext context);

		IInjectionRegister<TContext, TIComponent, TComponent> Construct(Func<TContext, TComponent> constructInstance);

		IInjectionRegister<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance);

		IInjectionRegister<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance);
	}
}
