using ModuleInject.Container.Interface;
using ModuleInject.Container.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.InstanceCreation
{
    public class FactoryInstanceCreation : IInstanceCreation
    {
        private Func<IDependencyContainer, object> createInstance;
        private IDependencyContainer container;

        public FactoryInstanceCreation(IDependencyContainer container, Func<IDependencyContainer, object> createInstance)
        {
            this.container = container;
            this.createInstance = createInstance;
        }

        public object Resolve(Type actualType)
        {
            return this.createInstance(container);
        }
    }
}
