using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public abstract class SourceOf<T> : ISourceOf<T>
	{
		private readonly IInstantiationStrategy<T> instantiationStrategy;

		public SourceOf(IInstantiationStrategy<T> instantiationStrategy)
		{
			if (instantiationStrategy == null)
			{
				throw new ArgumentNullException("The inner ISourceOf for a SourceOf instance must not be null.");
			}
			this.instantiationStrategy = instantiationStrategy;
        }

		public T GetInstance()
		{
			return instantiationStrategy.GetInstance(CreateInstance);
		}

		protected abstract T CreateInstance();
	}

	public class SourceOf<TContext, TIComponent, TComponent> : SourceOf<TIComponent>
		where TComponent : TIComponent
	{
		private readonly IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister;

		public SourceOf(IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister, IInstantiationStrategy<TIComponent> instantiationStrategy) : base(instantiationStrategy)
		{
			this.injectionRegister = injectionRegister;
		}

		public SourceOf<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.injectionRegister.Change(changeInstance);
			return this;
		}

		public SourceOf<TContext, TIComponent, TComponent> Construct(Func<TContext, TComponent> constructInstance)
		{
			this.injectionRegister.Construct(constructInstance);
			return this;
		}

		public SourceOf<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.injectionRegister.Inject(injectInInstance);
			return this;
		}

		public SourceOf<TContext, TIComponent, TComponent> SetContext(TContext context)
		{
			this.injectionRegister.SetContext(context);
			
			return this;
		}

		protected override TIComponent CreateInstance()
		{
			return injectionRegister.CreateInstance();
		}
	}
}
