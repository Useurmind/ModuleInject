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
}
