using System.Linq;

namespace ModuleInject.Interfaces.Disposing
{
    using System;

    public class DisposingEventArgs : EventArgs
    {
        public DisposingEventArgs(IDisposableExt disposedInstance)
        {
            this.DisposedInstance = disposedInstance;
        }

        public IDisposableExt DisposedInstance { get; private set; }
    }
}