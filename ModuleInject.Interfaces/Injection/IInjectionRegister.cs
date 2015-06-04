using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public class ObjectResolvedContext
	{
        public string ComponentName { get; set; }
		public Type ContextType { get; set; }
		public Type ComponentInterface { get; set; }
		public Type ComponentType { get; set; }
		public Object Instance { get; set; }
	}

	public interface IWrapInjectionRegister
	{
		IInjectionRegister Register { get; }
	}

	public interface IInjectionRegister : IDisposable
	{
        string ComponentName { get; }
		Type ComponentInterface { get; }
		Type ComponentType { get; set; }
		Type ContextType { get; }

		IEnumerable<object> MetaData { get; }

		IInstantiationStrategy GetInstantiationStrategy();

        IDisposeStrategy GetDisposeStrategy();

        void SetContext(object context);

		void InstantiationStrategy(IInstantiationStrategy instantiationStrategy);

        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        void Construct(Func<object, object> constructInstance);

		void Inject(Action<object, object> injectInInstance);

		void Change(Func<object, object, object> changeInstance);

		void Change(Func<object, object> changeInstance);

		void AddMeta(object metaData);

		void OnResolve(Action<ObjectResolvedContext> resolveHandler);

		object GetInstance();
	}

	public interface IInterfaceInjectionRegister<TIContext, TIComponent> : IWrapInjectionRegister
	{
		void Inject(Action<TIContext, TIComponent> injectInInstance);

		//void Change(Func<TIContext, TIComponent, TIComponent> changeInstance);

		//void Change(Func<TIComponent, TIComponent> changeInstance);

		void AddMeta<T>(T metaData);
	}

	public interface IInjectionRegister<TContext, TIComponent, TComponent> : IWrapInjectionRegister
	{
		void SetContext(TContext context);

		void InstantiationStrategy(IInstantiationStrategy<TIComponent> instantiationStrategy);

        void DisposeStrategy(IDisposeStrategy disposeStrategy);

        void Construct(Func<TContext, TComponent> constructInstance);

		void Inject(Action<TContext, TComponent> injectInInstance);

		void Change(Func<TContext, TIComponent, TIComponent> changeInstance);

		void Change(Func<TIComponent, TIComponent> changeInstance);

		void AddMeta<T>(T metaData);

		TIComponent GetInstance();
	}
}
