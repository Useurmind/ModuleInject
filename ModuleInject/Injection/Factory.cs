using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class Factory<TContext, TIComponent, TComponent> : IFactory<TIComponent>
		where TComponent : TIComponent
	{
		private TContext context;
		private Func<TContext, TComponent> constructInstance;
		private IList<Action<TContext, TComponent>> injectInInstanceList;
		private IList<Func<TContext, TIComponent, TIComponent>> changeInstanceList;

		public Factory(TContext context)
		{
			this.context = context;
			this.injectInInstanceList = new List<Action<TContext, TComponent>>();
			this.changeInstanceList = new List<Func<TContext, TIComponent, TIComponent>>();
		}

		public Factory<TContext, TIComponent, TComponent> Create(Func<TContext, TComponent> constructInstance)
		{
			this.constructInstance = constructInstance;
			return this;
		}

		public Factory<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.injectInInstanceList.Add(injectInInstance);
			return this;
		}

		public Factory<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.changeInstanceList.Add(changeInstance);
			return this;
		}

		public TIComponent Next()
		{
			TComponent instance = constructInstance(this.context);
			foreach (var injectInInstance in this.injectInInstanceList)
			{
				injectInInstance(this.context, instance);
			}

			TIComponent usedInstance = instance;

			foreach (var changeInstance in changeInstanceList)
			{
				usedInstance = changeInstance(this.context, usedInstance);
			}

			return usedInstance;
		}
	}
}
