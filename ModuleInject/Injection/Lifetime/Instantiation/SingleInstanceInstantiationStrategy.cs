using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Utility;

namespace ModuleInject.Injection
{
    public class SingleInstanceInstantiationStrategy : IInstantiationStrategy
    {
        private object instance;

        public object GetInstance(Func<object> createInstance)
        {
            if (instance == null)
            {
                CommonFunctions.CheckNullArgument(nameof(createInstance), createInstance);
                instance = createInstance();
            }
            return instance;
        }
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
}
