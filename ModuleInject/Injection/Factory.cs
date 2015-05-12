using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public sealed class Factory<TContext, TIComponent, TComponent> : 
		InjectionRegister<Factory<TContext, TIComponent, TComponent>, TContext, TIComponent, TComponent>,
		IFactory<TIComponent>
		where TComponent : TIComponent
		where TContext : class
	{

		public Factory(TContext context = null) :base(context)
		{
		}

		public TIComponent Next()
		{
			return CreateInstance();
		}

		public TIComponent GetInstance()
		{
			return Next();
		}
	}
}
