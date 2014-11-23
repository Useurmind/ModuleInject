using ModuleInject.Container.Interface;
using ModuleInject.Container.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Dependencies
{
    public class LambdaDependencyInjection : IDependencyInjection
    {
        private Action<IDependencyContainer, object> InjectionAction;
        private IDependencyContainer Container;

        public LambdaDependencyInjection(IDependencyContainer container, Action<IDependencyContainer, object> injectionAction)
        {
            this.Container = container;
            this.InjectionAction = injectionAction;
        }

        public void Resolve(object instance)
        {
            this.InjectionAction(this.Container, instance);
        }
    }

    public class LambdaDependencyInjection<TInstance> : LambdaDependencyInjection
    {
        public LambdaDependencyInjection(IDependencyContainer container, Action<IDependencyContainer, TInstance> injectionAction)
            : base(container, new Action<IDependencyContainer, object>((cont, obj) => injectionAction(cont, (TInstance)obj)))
        {

        }
    }
}
