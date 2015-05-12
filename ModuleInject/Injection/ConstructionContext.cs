using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public abstract class ConstructionContext<TModule, TIComponent>
		   where TModule : class, IInjectionModule
	{
		protected TModule Module { get; private set; }

		public ConstructionContext(TModule module)
		{
			this.Module = module;
		}

		protected void OnConstruct<TComponent>(IInjectionRegister<TModule, TIComponent, TComponent> createdInjectionRegister)
		{
			this.Module.RegisterInjectionRegister(createdInjectionRegister);
        }
	}

	public class FactoryConstructionContext<TModule, TIComponent> : ConstructionContext<TModule, TIComponent>
		where TModule : class, IInjectionModule
	{
		public FactoryConstructionContext(TModule module) : base(module)
		{
		}

		public Factory<TModule, TIComponent, TComponent> Construct<TComponent>()
			where TComponent : TIComponent, new()
		{
			return this.Construct(m => new TComponent());
		}

		public Factory<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
			where TComponent : TIComponent
		{
			var factory = new Factory<TModule, TIComponent, TComponent>(Module);
			factory.Construct(constructInstance);
			return factory;
		}
	}

	public class SingletonConstructionContext<TModule, TIComponent> : ConstructionContext<TModule, TIComponent>
		where TModule : class, IInjectionModule
	{
		public SingletonConstructionContext(TModule module) : base(module)
		{
		}

		public Singleton<TModule, TIComponent, TComponent> Construct<TComponent>()
			where TComponent : TIComponent, new()
		{
			return this.Construct(m => new TComponent());
		}

		public Singleton<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
			where TComponent : TIComponent
		{
			var singleton = new Singleton<TModule, TIComponent, TComponent>(Module);
			singleton.Construct(constructInstance);
			return singleton;
		}
	}
}
