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

    public interface IDisposeStrategy : IDisposable
    {
        void OnInstance(object instance);
    }

    public interface IDisposeStrategy<T> : IDisposable
    {
        void OnInstance(T instance);
    }

    public interface IInstantiationStrategy<T> : IInstantiationStrategy
	{
		T GetInstance(Func<T> createInstance);
	}
}
