using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection
{
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
