using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class Singleton<TContext, TIComponent, TComponent> :
		InjectionRegister<Singleton<TContext, TIComponent, TComponent>, TContext, TIComponent, TComponent>,
        ISingleton<TIComponent>
		where TComponent : TIComponent
		where TContext : class
	{
		private TIComponent instance;

		public Singleton(TContext context=null) : base(context)
		{
		}

		public TIComponent Instance
		{
			get
			{
				if (instance == null)
				{
					instance = CreateInstance();
				}
				return instance;
			}
		}

		public TIComponent GetInstance()
		{
			return Instance;
		}
	}
}
