using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
	public interface IInstantiationStrategy<T>
	{
		T GetInstance(Func<T> createInstance);
	}

	public class SingleInstanceInstantiationStrategy<T> : IInstantiationStrategy<T>
	{
		private T instance;

		public T GetInstance(Func<T> createInstance)
		{
			if (instance == null)
			{
				instance = createInstance();
			}
			return instance;
		}
	}

	public class FactoryInstantiationStrategy<T> : IInstantiationStrategy<T>
	{
		public T GetInstance(Func<T> createInstance)
		{
			return createInstance();
		}
	}
}
