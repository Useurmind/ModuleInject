using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Injection
{
    /// <summary>
    /// Strategy that is responsible for creating new instances of the component.
    /// </summary>
	public interface IInstantiationStrategy
	{
        /// <summary>
        /// Called when a instance of the component should be retrieved.
        /// </summary>
        /// <param name="createInstance">A function that will create a fully injected component instance.</param>
        /// <returns>The "new" instance that will be returned.</returns>
		object GetInstance(Func<object> createInstance);
	}

    /// <summary>
    /// Strategy that is responsible for creating new instances of the component.
    /// </summary>
    /// <typeparam name="T">Type of the component instances that will be created.</typeparam>
    public interface IInstantiationStrategy<T> : IInstantiationStrategy
	{
        /// <summary>
        /// Called when a instance of the component should be retrieved.
        /// </summary>
        /// <param name="createInstance">A function that will create a fully injected component instance.</param>
        /// <returns>The "new" instance that will be returned.</returns>
        T GetInstance(Func<T> createInstance);
	}
}
