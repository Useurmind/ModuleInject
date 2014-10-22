using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Lifetime
{
    using ModuleInject.Container.Interface;

    public sealed class SingletonLifetime : ILifetime
    {
        private bool isResolved;
        private object instance;

        public SingletonLifetime()
        {
            isResolved = false;
            instance = null;
        }

        public bool OnObjectResolving()
        {
            return !isResolved;
        }

        public void OnObjectResolved(object instance)
        {
            this.instance = instance;
            this.isResolved = true;
        }

        public object OnObjectNotResolved()
        {
            return this.instance;
        }

        public void Dispose()
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
