using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Injection
{
	public class ConstructionContext<TModule, TIComponent>
		   where TModule : class, IInjectionModule
	{
		private IInstantiationStrategy<TIComponent> instantiationStrategy;
		private string componentName;

		protected TModule Module { get; private set; }

		public ConstructionContext(TModule module, IInstantiationStrategy<TIComponent> instantiationStrategy, string componentName =null)
		{
			this.Module = module;
			this.componentName = componentName;
			this.instantiationStrategy = instantiationStrategy;
        }

		public SourceOf<TModule, TIComponent, TComponent> Construct<TComponent>()
			where TComponent : TIComponent, new()
		{
			return this.Construct<TComponent>(m => new TComponent());
		}

		public SourceOf<TModule, TIComponent, TComponent> Construct<TComponent>(Func<TModule, TComponent> constructInstance)
			where TComponent : TIComponent
		{
			var injectionRegister = new InjectionRegister<TModule, TIComponent, TComponent>();

			injectionRegister.InstantiationStrategy(instantiationStrategy);

            var source = new SourceOf<TModule, TIComponent, TComponent>(injectionRegister);

			source.SetContext(Module);
			source.Construct(constructInstance);

			this.Module.RegisterInjectionRegister(injectionRegister.Register, this.componentName);

			return source;
        }
	}
}
