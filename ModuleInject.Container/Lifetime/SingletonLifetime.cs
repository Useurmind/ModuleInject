using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Container.Lifetime
{
    using ModuleInject.Common.Disposing;
    using ModuleInject.Container.Interface;

    public class SingletonLifetime : DisposableExtBase, ILifetime
    {
        private bool isResolved;
        private object instance;

        public SingletonLifetime()
        {
            isResolved = false;
            instance = null;
        }

        public virtual bool OnObjectResolving()
        {
            return !isResolved;
        }

        public virtual void OnObjectResolved(ObjectResolvedContext context)
        {
            this.instance = context.Instance;
            this.isResolved = true;
        }

        public virtual object OnObjectNotResolved()
        {
            return this.instance;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
