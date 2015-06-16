using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    public abstract class DisposeStrategy : DisposableExtBase, IDisposeStrategy
    {
        private ISet<IDisposable> disposables;

        public DisposeStrategy()
        {
            disposables = new HashSet<IDisposable>();
        }

        protected void AddInstance(object instance)
        {
            var disposable = instance as IDisposable;
            if(disposable != null)
            {
                disposables.Add(disposable);
            }
        }

        public abstract void OnInstance(object instance);
        
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    foreach (var disposable in disposables)
                    {
                        disposable.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }
    }
}
