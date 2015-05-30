using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    public abstract class DisposeStrategy : IDisposeStrategy
    {
        private bool isDisposed = false;
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
        
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    foreach (var disposable in disposables)
                    {
                        disposable.Dispose();
                    }
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public class RememberAndDisposeStrategy : DisposeStrategy
    {
        public override void OnInstance(object instance)
        {
            this.AddInstance(instance);
        }
    }

    public class FireAndForgetStrategy : IDisposeStrategy
    {
        public void Dispose()
        {
        }

        public void OnInstance(object instance)
        {
        }
    }
}
