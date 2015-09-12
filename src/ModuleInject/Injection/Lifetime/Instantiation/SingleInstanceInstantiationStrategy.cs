using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Utility;

namespace ModuleInject.Injection
{
    /// <summary>
    /// Instantiation strategy that will always return the same instance.
    /// Type ignorant.
    /// </summary>
    public class SingleInstanceInstantiationStrategy : IInstantiationStrategy
    {
        private object instance;

        /// <inheritdoc />
        public object GetInstance(Func<object> createInstance)
        {
            if (instance == null)
            {
                CommonFunctions.CheckNullArgument("createInstance", createInstance);
                instance = createInstance();
            }
            return instance;
        }
    }

    /// <summary>
    /// Instantiation strategy that will always return the same instance.
    /// </summary>
    /// <typeparam name="T">Type of the instance.</typeparam>
    public class SingleInstanceInstantiationStrategy<T> : InstantiationStrategy<T>
    {
        private T instance;

        /// <inheritdoc />
        public override T GetInstance(Func<T> createInstance)
        {
            if (instance == null)
            {
                instance = createInstance();
            }
            return instance;
        }
    }
}
