using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;

namespace ModuleInject.Injection
{
	public abstract class ConstructionContext<TModule, TIComponent>
		where TModule : class
	{
		protected TModule Module { get; private set; }

		public ConstructionContext(TModule module)
		{
			this.Module = module;
		}
	}

	public class FactoryConstructionContext<TModule, TIComponent> : ConstructionContext<TModule, TIComponent>
		where TModule : class
	{
		public FactoryConstructionContext(TModule module) : base(module)
		{
		}

		public FactoryInjectionContext<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
			where TComponent : TIComponent, new()
		{
			var factory = new Factory<TModule, TIComponent, TComponent>(Module);
			factory.Construct(constructInstance);
			return new FactoryInjectionContext<TModule, TIComponent, TComponent>(factory);
		}
	}

	public class SingletonConstructionContext<TModule, TIComponent> : ConstructionContext<TModule, TIComponent>
		where TModule : class
	{
		public SingletonConstructionContext(TModule module) : base(module)
		{
		}

		public SingletonInjectionContext<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
			where TComponent : TIComponent, new()
		{
			var singleton = new Singleton<TModule, TIComponent, TComponent>(Module);
			singleton.Construct(constructInstance);
			return new SingletonInjectionContext<TModule, TIComponent, TComponent>(singleton);
		}
	}

	public class InjectionContext<TSelf, TModule, TIComponent, TComponent>
		where TSelf : InjectionContext<TSelf, TModule, TIComponent, TComponent>
	where TComponent : TIComponent
		where TModule : class
	{
		private IRegisterInjection<TModule, TIComponent, TComponent> injectionRegistrator;

		public InjectionContext(IRegisterInjection<TModule, TIComponent, TComponent> factory)
		{
			this.injectionRegistrator = factory;
		}

		public TSelf Inject(Action<TModule, TComponent> injectInInstance)
		{
			this.injectionRegistrator.Inject(injectInInstance);
			return (TSelf)this;
		}

		public TSelf Change(Func<TModule, TIComponent, TIComponent> changeInstance)
		{
			this.injectionRegistrator.Change(changeInstance);
			return (TSelf)this;
		}
	}

	public class FactoryInjectionContext<TModule, TIComponent, TComponent> : InjectionContext<FactoryInjectionContext<TModule, TIComponent, TComponent>, TModule, TIComponent, TComponent>
	where TComponent : TIComponent
		where TModule : class
	{
		private Factory<TModule, TIComponent, TComponent> factory;

		public FactoryInjectionContext(Factory<TModule, TIComponent, TComponent> factory) : base(factory)
		{
			this.factory = factory;
		}
		public IFactory<TIComponent> ToFactory()
		{
			return this.factory;
		}
	}

	public class SingletonInjectionContext<TModule, TIComponent, TComponent> : InjectionContext<SingletonInjectionContext<TModule, TIComponent, TComponent>, TModule, TIComponent, TComponent>
	where TComponent : TIComponent
		where TModule : class
	{
		private Singleton<TModule, TIComponent, TComponent> singleton;

		public SingletonInjectionContext(Singleton<TModule, TIComponent, TComponent> singleton) : base(singleton)
		{
			this.singleton = singleton;
		}
		public ISingleton<TIComponent> ToSingleton()
		{
			return this.singleton;
		}
	}

	public class InjectionModule<TModule> : Module
		where TModule : InjectionModule<TModule>
	{
		protected FactoryConstructionContext<TModule, TIComponent> Factory<TIComponent>()
		{
			return new FactoryConstructionContext<TModule, TIComponent>((TModule)this);
		}

		protected SingletonConstructionContext<TModule, TIComponent> Singleton<TIComponent>()
		{
			return new SingletonConstructionContext<TModule, TIComponent>((TModule)this);
		}

		protected override void OnRegistryResolved(IRegistry usedRegistry)
		{

		}
	}
}
