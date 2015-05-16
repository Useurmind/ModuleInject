using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class InjectionRegister : IInjectionRegister
	{
		private object context;
		private Func<object, object> constructInstance;
		private IList<Action<object, object>> injectInInstanceList;
		private IList<Func<object, object, object>> changeInstanceList;
		private ISet<object> metaData;

		public InjectionRegister(Type contextType, Type componentInterface, Type componentType)
		{
			this.ContextType = contextType;
			this.ComponentInterface = componentInterface;
			this.ComponentType = componentType;

			this.injectInInstanceList = new List<Action<object, object>>();
			this.changeInstanceList = new List<Func<object, object, object>>();
		}

		public Type ComponentInterface { get; private set; }
		public Type ComponentType { get; private set; }
		public Type ContextType { get; private set; }

		public IEnumerable<object> MetaData { get { return this.metaData; } }

		public void SetContext(object context)
		{
			this.context = context;
		}

		public void Construct(Func<object, object> constructInstance)
		{
			this.constructInstance = constructInstance;
		}

		public void Inject(Action<object, object> injectInInstance)
		{
			this.injectInInstanceList.Add(injectInInstance);
		}

		public void Change(Func<object, object, object> changeInstance)
		{
			this.changeInstanceList.Add(changeInstance);
		}

		public void Change(Func<object, object> changeInstance)
		{
			this.Change((ctx, comp) => changeInstance(comp));
		}

		public void AddMeta(object metaData)
		{
			this.metaData.Add(metaData);
		}

		public object CreateInstance()
		{
			object instance = constructInstance(this.context);
			foreach (var injectInInstance in this.injectInInstanceList)
			{
				injectInInstance(this.context, instance);
			}

			object usedInstance = instance;

			foreach (var changeInstance in changeInstanceList)
			{
				usedInstance = changeInstance(this.context, usedInstance);
			}

			return usedInstance;
		}
	}

	public class InterfaceInjectionRegister<TIContext, TIComponent> : IInterfaceInjectionRegister<TIContext, TIComponent>
	{
		public InterfaceInjectionRegister(IInjectionRegister injectionRegister)
		{
			this.Register = injectionRegister;
		}

		public IInjectionRegister Register { get; private set; }

		public void Change(Func<TIComponent, TIComponent> changeInstance)
		{
			this.Register.Change(comp => changeInstance((TIComponent)comp));
		}

		public void Change(Func<TIContext, TIComponent, TIComponent> changeInstance)
		{
			this.Register.Change((ctx, comp) => changeInstance((TIContext)ctx, (TIComponent)comp));
		}

		public void Inject(Action<TIContext, TIComponent> injectInInstance)
		{
			this.Register.Inject((ctx, comp) => injectInInstance((TIContext)ctx, (TIComponent)comp));
		}

		public void AddMeta<T>(T metaData)
		{
			this.Register.AddMeta(metaData);
		}		
	}

	public class InjectionRegister<TContext, TIComponent, TComponent> : IInjectionRegister<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
		where TContext : class
	{
		public IInjectionRegister Register { get; private set; }

		public InjectionRegister()
		{
			this.Register = new InjectionRegister(typeof(TContext), typeof(TIComponent), typeof(TComponent));
		}

		public InjectionRegister(IInjectionRegister injectionRegister)
		{
			CheckTypes(injectionRegister);
			this.Register = injectionRegister;
		}

		private void CheckTypes(IInjectionRegister injectionRegister)
		{
			if (injectionRegister.ContextType != typeof(TContext) ||
				injectionRegister.ComponentInterface != typeof(TIComponent) ||
				injectionRegister.ComponentType != typeof(TComponent))
			{
				var message = string.Format("The types in the inner injection register do not match the generic type arguments of the outer one: inner (({0}, {1}, {2})), generic ({3}, {4}, {5}).",
					injectionRegister.ContextType.Name, injectionRegister.ComponentInterface.Name, injectionRegister.ComponentType.Name,
					typeof(TContext).Name, typeof(TIComponent).Name, typeof(TComponent).Name);
				throw new ArgumentOutOfRangeException(message);
			}
		}

		public void SetContext(TContext context)
		{
			this.Register.SetContext(context);
		}

		public void Construct(Func<TContext, TComponent> constructInstance)
		{
			this.Register.Construct(ctx => constructInstance((TContext)ctx));
		}

		public void Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.Register.Inject((ctx, comp) => injectInInstance((TContext)ctx, (TComponent)comp));
		}

		public void Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.Register.Change((ctx, comp) => changeInstance((TContext)ctx, (TIComponent)comp));
		}

		public void Change(Func<TIComponent, TIComponent> changeInstance)
		{
			this.Change((ctx, comp) => changeInstance(comp));
		}

		public void AddMeta<T>(T metaData)
		{
			this.Register.AddMeta(metaData);
		}

		public TIComponent CreateInstance()
		{
			return (TIComponent)this.Register.CreateInstance();
		}
	}
}
