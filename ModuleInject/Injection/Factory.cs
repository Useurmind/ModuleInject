using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public sealed class Factory<TContext, TIComponent, TComponent> : IFactory<TIComponent>, IRegisterInjection<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
		where TContext : class
	{
		private TContext context;
		private Func<TContext, TComponent> constructInstance;
		private IList<Action<TContext, TComponent>> injectInInstanceList;
		private IList<Func<TContext, TIComponent, TIComponent>> changeInstanceList;

		public Factory(TContext context=null)
		{
			this.context = context;
			this.injectInInstanceList = new List<Action<TContext, TComponent>>();
			this.changeInstanceList = new List<Func<TContext, TIComponent, TIComponent>>();
		}

		public void SetContext(TContext context)
		{
			this.context = context;
		}

		public void Construct(Func<TContext, TComponent> constructInstance)
		{
			this.constructInstance = constructInstance;
		}

		public void Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.injectInInstanceList.Add(injectInInstance);
		}

		public void Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.changeInstanceList.Add(changeInstance);
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
