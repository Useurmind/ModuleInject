using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public abstract class InjectionRegister
	{
		public InjectionRegister(Type contextType, Type componentInterface, Type componentType)
		{
			this.ContextType = contextType;
			this.ComponentInterface = componentInterface;
			this.ComponentType = componentType;
		}

		public Type ComponentInterface { get; private set; }
		public Type ComponentType { get; private set; }
		public Type ContextType { get; private set; }
	}

	public abstract class InjectionRegister<TSelf, TContext, TIComponent, TComponent> : InjectionRegister, IInjectionRegister<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
		where TContext : class
		where TSelf :InjectionRegister<TSelf, TContext, TIComponent, TComponent>
    {
		private TContext context;
		private Func<TContext, TComponent> constructInstance;
		private IList<Action<TContext, TComponent>> injectInInstanceList;
		private IList<Func<TContext, TIComponent, TIComponent>> changeInstanceList;

		public InjectionRegister(TContext context=null) : base(typeof(TContext), typeof(TIComponent), typeof(TComponent))
		{
			this.context = context;
			this.injectInInstanceList = new List<Action<TContext, TComponent>>();
			this.changeInstanceList = new List<Func<TContext, TIComponent, TIComponent>>();
		}

		public TSelf SetContext(TContext context)
		{
			this.context = context;
			return (TSelf)this;
		}

		public TSelf Construct(Func<TContext, TComponent> constructInstance)
		{
			this.constructInstance = constructInstance;
			return (TSelf)this;
		}

		public TSelf Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.injectInInstanceList.Add(injectInInstance);
			return (TSelf)this;
		}

		public TSelf Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.changeInstanceList.Add(changeInstance);
			return (TSelf)this;
		}

		IInjectionRegister<TContext, TIComponent, TComponent> IInjectionRegister<TContext, TIComponent, TComponent>.SetContext(TContext context)
		{
			return this.SetContext(context);
		}

		IInjectionRegister<TContext, TIComponent, TComponent> IInjectionRegister<TContext, TIComponent, TComponent>.Construct(Func<TContext, TComponent> constructInstance)
		{
			return this.Construct(constructInstance);
		}

		IInjectionRegister<TContext, TIComponent, TComponent> IInjectionRegister<TContext, TIComponent, TComponent>.Inject(Action<TContext, TComponent> injectInInstance)
		{
			return this.Inject(injectInInstance);
		}

		IInjectionRegister<TContext, TIComponent, TComponent> IInjectionRegister<TContext, TIComponent, TComponent>.Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			return this.Change(changeInstance);
		}

		public TIComponent CreateInstance()
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
