namespace ModuleInject.Common.Disposing
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