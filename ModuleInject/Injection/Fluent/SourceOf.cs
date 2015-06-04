using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public abstract class SourceOf<T> : ISourceOf<T>
	{
		public abstract T Get();
	}

	public class SourceOf<TContext, TIComponent, TComponent> : SourceOf<TIComponent>, IWrapInjectionRegister, ISourceOf<TContext, TIComponent, TComponent>
		where TComponent : TIComponent
	{
		private readonly IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister;

		public IInjectionRegister Register
		{
			get
			{
				return injectionRegister.Register;
			}
		}

		public SourceOf(IInjectionRegister<TContext, TIComponent, TComponent> injectionRegister)
		{
			this.injectionRegister = injectionRegister;
		}

		public ISourceOf<TContext, TIComponent, TComponent> Change(Func<TContext, TIComponent, TIComponent> changeInstance)
		{
			this.injectionRegister.Change(changeInstance);
			return this;
		}

		public ISourceOf<TContext, TIComponent, TComponent> Inject(Action<TContext, TComponent> injectInInstance)
		{
			this.injectionRegister.Inject(injectInInstance);
			return this;
		}

		public ISourceOf<TContext, TIComponent, TComponent> AddMeta<T>(T metaData)
		{
			this.injectionRegister.AddMeta(metaData);

			return this;
		}

		public override TIComponent Get()
		{
			return injectionRegister.GetInstance();
		}
	}

	public static class SourceOfExtensions
	{
		public static ISourceOf<TContext, TIComponent, TComponent> AddInjector<TContext, TIComponent, TComponent, TIContext, TIComponent2>(
			this ISourceOf<TContext, TIComponent, TComponent> source,
			IInterfaceInjector<TIContext, TIComponent2> injector)
		where TComponent : TIComponent, TIComponent2
			where TContext : TIContext
		{
			var interfaceInjectionRegister = new InterfaceInjectionRegister<TIContext, TIComponent2>(source.Register);
			injector.InjectInto(interfaceInjectionRegister);
			return source;
		}
	}
}
