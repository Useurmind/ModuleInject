using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public interface IInstantiationStrategy
	{
		object GetInstance(Func<object> createInstance);
	}

	public interface IInstantiationStrategy<T>
	{
		IInstantiationStrategy Strategy { get; }

		T GetInstance(Func<T> createInstance);
	}
}
