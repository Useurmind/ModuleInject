using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IRegisterInjection<TContext, TIComponent, TComponent>
	{
		void SetContext(TContext context);

		void Construct(Func<TContext, TComponent> constructInstance);

		void Inject(Action<TContext, TComponent> injectInInstance);

		void Change(Func<TContext, TIComponent, TIComponent> changeInstance);
	}
}
