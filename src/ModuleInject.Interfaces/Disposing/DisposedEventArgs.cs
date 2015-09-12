using System.Linq;

namespace ModuleInject.Interfaces.Disposing
{
    using System;

    public class DisposedEventArgs : EventArgs
    {
        public DisposedEventArgs(IDisposableExt disposedInstance)
        {
            this.DisposedInstance = disposedInstance;
        }

        public IDisposableExt DisposedInstance { get; private set; }
    }
}