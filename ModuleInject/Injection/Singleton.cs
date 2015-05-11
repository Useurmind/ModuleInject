using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class Singleton<TContext, TIComponent, TComponent> : ISingleton<TIComponent>, IRegisterInjection<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
		where TContext : class
	{
		private Factory<TContext, TIComponent, TComponent> factory;
		private TIComponent instance;

		public Singleton(TContext context=null)
		{
			factory = new Factory<TContext, TIComponent, TComponent>(context);
		}

		public TIComponent Instance
		{
			get
			{
				if (instance == null)
				{
					instance = factory.Next();
				}
				return instance;
			}
		}

		public void SetContext(TContext context)
		{
			this.factory.SetContext(context);
		}

		public void Construct(Func<TContext, TComponent> constructInstance)
		{
			factory.Construct(constructInstance);
		}

		public void Inject(Action<TContext, TComponent> injectInInstance)
		{
			factory.Inject(injectInInstance);
		}

		public void Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			factory.Change(changeInstance);
		}
	}
}
