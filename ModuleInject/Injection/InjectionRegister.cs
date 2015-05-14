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

	public class InjectionRegister<TContext, TIComponent, TComponent> : InjectionRegister, IInjectionRegister<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
		where TContext : class
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

	//public class WrappingInjectionRegister<TIModule, TModule, TIComponent, TIComponent2, TComponent> : IInject<TIModule, TIComponent2>
	//	where TModule : TIModule
	//	where TComponent : TIComponent2, TIComponent
	//{
	//	public WrappingInjectionRegister(IInjectionRegister<TModule, TIComponent, TComponent> injectionRegister)
	//	{

	//	}

	//}
}
