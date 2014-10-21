using System.Linq;

namespace ModuleInject.Common.Disposing
{
    using System;

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
