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

	public class SourceOf<TContext, TIComponent, TComponent> : SourceOf<TIComponent>, IWrapInjectionRegister
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

		public SourceOf<TContext, TIComponent, TComponent> AddMeta<T>(T metaData)
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
		public static SourceOf<TContext, TIComponent, TComponent> AddInjector<TContext, TIComponent, TComponent, TIContext, TIComponent2>(
			this SourceOf<TContext, TIComponent, TComponent> source,
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
