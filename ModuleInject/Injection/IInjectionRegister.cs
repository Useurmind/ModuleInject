using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInjectionRegister
	{
		Type ComponentInterface { get; }
		Type ComponentType { get; }
		Type ContextType { get; }

		void SetContext(object context);

		void Construct(Func<object, object> constructInstance);

		void Inject(Action<object, object> injectInInstance);

		void Change(Func<object, object, object> changeInstance);

		void Change(Func<object, object> changeInstance);

		object CreateInstance();
	}

	public interface IInterfaceInjectionRegister<TIContext, TIComponent>
	{
		void Inject(Action<TIContext, TIComponent> injectInInstance);

		void Change(Func<TIContext, TIComponent, TIComponent> changeInstance);

		void Change(Func<TIComponent, TIComponent> changeInstance);
	}

	public interface IInjectionRegister<TContext, TIComponent, TComponent>
	{
		void SetContext(TContext context);

		void Construct(Func<TContext, TComponent> constructInstance);

		void Inject(Action<TContext, TComponent> injectInInstance);

		void Change(Func<TContext, TIComponent, TIComponent> changeInstance);

		void Change(Func<TIComponent, TIComponent> changeInstance);

		TIComponent CreateInstance();
	}
}
