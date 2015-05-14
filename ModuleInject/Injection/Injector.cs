using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInjector<TContext, TComponent>
	{
		void Execute(IInject<TContext, TComponent> injectionRegister);
	}

	//public class Injector<TContext, TComponent> : IInjector<TContext, TComponent>
	//{
	//}
}
