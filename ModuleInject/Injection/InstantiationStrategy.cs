using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
    public class DelegateInstantiationStrategy : IInstantiationStrategy
    {
        private Func<Func<object>, object> getInstanceFunc;

        public DelegateInstantiationStrategy(Func<Func<object>, object> getInstance)
        {
            this.getInstanceFunc = getInstance;
        }
        
        public object GetInstance(Func<object> createInstance)
        {
            var instance = getInstanceFunc(createInstance);
            return instance;
        }
    }

    public abstract class InstantiationStrategy<T> : IInstantiationStrategy<T>, IInstantiationStrategy
    {
        public InstantiationStrategy()
        {
        }

        public abstract T GetInstance(Func<T> createInstance);

        object IInstantiationStrategy.GetInstance(Func<object> createInstance)
        {
            return this.GetInstance(() => (T)createInstance());
        }
    }

    public class SingleInstanceInstantiationStrategy<T> : InstantiationStrategy<T>
    {
        private T instance;

        public SingleInstanceInstantiationStrategy()
        {
        }

        public override T GetInstance(Func<T> createInstance)
        {
            if (instance == null)
            {
                instance = createInstance();
            }
            return instance;
        }
    }

    public class FactoryInstantiationStrategy<T> : InstantiationStrategy<T>
    {
        public FactoryInstantiationStrategy()
        {
        }

        public override T GetInstance(Func<T> createInstance)
        {
            return createInstance();
        }
    }
}
