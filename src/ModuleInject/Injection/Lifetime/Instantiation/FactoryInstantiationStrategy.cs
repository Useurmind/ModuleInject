using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    /// <summary>
    /// Instantiation strategy that will return a new instance for each instance request.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FactoryInstantiationStrategy<T> : InstantiationStrategy<T>
    {
        public FactoryInstantiationStrategy()
        {
        }

        /// <inheritdoc />
        public override T GetInstance(Func<T> createInstance)
        {
            return createInstance();
        }
    }
}
