using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public interface ISourceOf<TIComponent>
	{
		/// <summary>
		/// Create an instance of the component.
		/// </summary>
		/// <returns>The created instance.</returns>
		TIComponent Get();
	}

    public interface IInstantiationStrategyContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        IDisposeStrategyContext<TContext, TIComponent> InstantiateWith(IInstantiationStrategy<TIComponent> instantiationStrategy);
    }

    public interface IDisposeStrategyContext<TContext, TIComponent> : IConstructionContext<TContext, TIComponent>
    {
        IConstructionContext<TContext, TIComponent> DisposeWith(IDisposeStrategy disposeStrategy);
    }

    public interface IConstructionContext<TContext, TIComponent> : IWrapInjectionRegister
    {
        ISourceOf<TContext, TIComponent, TComponent> Construct<TComponent>()
            where TComponent : TIComponent, new();

        ISourceOf<TContext, TIComponent, TComponent> Construct<TComponent>(Func<TContext, TComponent> constructInstance)
            where TComponent : TIComponent;
    }

    public interface ISourceOf<TContext, TIComponent, TComponent> : IWrapInjectionRegister, ISourceOf<TIComponent>
    {
        ISourceOf<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance);

        ISourceOf<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance);

        ISourceOf<TContext, TIComponent, TComponent> AddMeta<T>(T metaData);
    }
}
