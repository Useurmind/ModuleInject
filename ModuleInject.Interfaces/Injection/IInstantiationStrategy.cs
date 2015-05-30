using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
	public interface IInstantiationStrategy : IDisposable
	{
		object GetInstance(Func<object> createInstance);

        void SetDisposeStrategy(IDisposeStrategy disposeStrategy);
	}

    public interface IDisposeStrategy : IDisposable
    {
        void OnInstance(object instance);
    }

	public interface IInstantiationStrategy<T>
	{
		IInstantiationStrategy Strategy { get; }

		T GetInstance(Func<T> createInstance);
	}
}
