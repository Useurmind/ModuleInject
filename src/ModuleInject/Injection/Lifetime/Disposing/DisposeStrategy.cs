using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces.Injection;

namespace ModuleInject.Injection
{
    /// <summary>
    /// Base class for dispose strategies.
    /// </summary>
    public abstract class DisposeStrategy : DisposableExtBase, IDisposeStrategy
    {
        private HashSet<IDisposable> disposables;

        public DisposeStrategy()
        {
            disposables = new HashSet<IDisposable>();
        }

        /// <summary>
        /// Remember an instance to dispose.
        /// </summary>
        /// <param name="instance">The instance to dispose.</param>
        protected void AddInstance(object instance)
        {
            var disposable = instance as IDisposable;
            if(disposable != null)
            {
                disposables.Add(disposable);
            }
        }

        /// <inheritdoc />
        /// Must be implemented in derived classes.
        public abstract void OnInstance(object instance);
        
        /// <summary>
        /// Will dispose all added instances add via <see cref="AddInstance(object)"/>.
        /// </summary>
        /// <param name="disposing"></param>
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
