using System.Linq;

namespace ModuleInject.Utility
{
    using System;

    using ModuleInject.Interfaces;

    /// <summary>
    /// The disposable ext base.
    /// </summary>
    public class DisposableExtBase : IDisposableExt
    {
        /// <summary>
        /// The disposing.
        /// </summary>
        public event EventHandler<DisposingEventArgs> Disposing;

        /// <summary>
        /// The disposed.
        /// </summary>
        public event EventHandler<DisposedEventArgs> Disposed;

        /// <summary>
        /// Gets a value indicating whether is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Dispose(true) and GC.SuppressFinalize(this) are called.")]
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                this.FireDisposing();

                this.IsDisposed = true;
                this.Dispose(true);

                this.FireDisposed();
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// The check dispose.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        protected void CheckDispose()
        {
            if (this.IsDisposed)
            {
                throw new InvalidOperationException("Instance already disposed, operation not allowed.");
            }
        }

        /// <summary>
        /// The fire disposing.
        /// </summary>
        private void FireDisposing()
        {
            if (this.Disposing != null)
            {
                this.Disposing(this, new DisposingEventArgs(this));
            }
        }

        /// <summary>
        /// The fire disposed.
        /// </summary>
        private void FireDisposed()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, new DisposedEventArgs(this));
            }
        }
    }
}