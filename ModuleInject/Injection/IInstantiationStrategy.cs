using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public class DelegateInstantiationStrategy : IInstantiationStrategy
	{
		private Func<Func<object>, object> getInstance;

		public DelegateInstantiationStrategy(Func<Func<object>, object> getInstance)
		{
			this.getInstance = getInstance;
		}

		public object GetInstance(Func<object> createInstance)
		{
			return getInstance(createInstance);
		}
	}

	public abstract class InstantiationStrategy<T> : IInstantiationStrategy<T>
	{
		public IInstantiationStrategy Strategy { get; private set; }

		public InstantiationStrategy()
		{
			this.Strategy = new DelegateInstantiationStrategy(createInstance =>
			{
				return GetInstance(() => (T)createInstance());
            });
        }

		public abstract T GetInstance(Func<T> createInstance);
	}

	public class SingleInstanceInstantiationStrategy<T> : InstantiationStrategy<T>
	{
		private T instance;

		public override T GetInstance(Func<T> createInstance)
		{
			if (instance == null)
			{
				instance = createInstance();
			}
			return instance;
		}
	}

	public class FactoryInstantiationStrategy<T> : InstantiationStrategy<T>
	{
		public override T GetInstance(Func<T> createInstance)
		{
			return createInstance();
		}
	}
}
