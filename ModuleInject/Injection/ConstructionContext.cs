using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class ConstructionContext<TModule, TIComponent>
		   where TModule : class, IInjectionModule
	{
		private IInstantiationStrategy<TIComponent> instantiationStrategy;

		protected TModule Module { get; private set; }

		public ConstructionContext(TModule module, IInstantiationStrategy<TIComponent> instantiationStrategy)
		{
			this.Module = module;
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
            var source = new SourceOf<TModule, TIComponent, TComponent>(injectionRegister, instantiationStrategy);

			source.SetContext(Module);
			source.Construct(constructInstance);

			this.Module.RegisterInjectionRegister(injectionRegister.Register);

			return source;
        }
	}
}
