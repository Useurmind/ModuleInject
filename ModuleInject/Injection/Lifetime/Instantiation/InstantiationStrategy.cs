using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{

    public abstract class InstantiationStrategy<T> : IInstantiationStrategy<T>, IInstantiationStrategy
    {
        protected InstantiationStrategy()
        {
        }

        public abstract T GetInstance(Func<T> createInstance);

        object IInstantiationStrategy.GetInstance(Func<object> createInstance)
        {
            return this.GetInstance(() => (T)createInstance());
        }
    }
}
