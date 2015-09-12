using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    /// <summary>
    /// Base class for instantiation strategies.
    /// </summary>
    /// <typeparam name="T">Type of the instance to create.</typeparam>
    public abstract class InstantiationStrategy<T> : IInstantiationStrategy<T>, IInstantiationStrategy
    {
        protected InstantiationStrategy()
        {
        }

        /// <inheritdoc />
        public abstract T GetInstance(Func<T> createInstance);


        /// <inheritdoc />
        object IInstantiationStrategy.GetInstance(Func<object> createInstance)
        {
            return this.GetInstance(() => (T)createInstance());
        }
    }
}
