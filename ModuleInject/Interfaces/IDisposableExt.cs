using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    public class DisposingEventArgs : EventArgs
    {
        public DisposingEventArgs(IDisposableExt disposedInstance)
        {
            DisposedInstance = disposedInstance;
        }

        public IDisposableExt DisposedInstance { get; private set; }
    }
    public class DisposedEventArgs : EventArgs
    {
        public DisposedEventArgs(IDisposableExt disposedInstance)
        {
            DisposedInstance = disposedInstance;
        }

        public IDisposableExt DisposedInstance { get; private set; }
    }

    /// <summary>
    /// An extended interface for disposable objects.
    /// </summary>
    public interface IDisposableExt : IDisposable
    {
        /// <summary>
        /// Triggered when the object is about to be disposed.
        /// </summary>
        event EventHandler<DisposingEventArgs> Disposing;

        /// <summary>
        /// Triggered when the object was just disposed.
        /// </summary>
        event EventHandler<DisposedEventArgs> Disposed;

        /// <summary>
        /// Gets a value indicating whether the object is already disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}
