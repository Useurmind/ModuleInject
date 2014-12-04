using System.Linq;

namespace ModuleInject.Interfaces
{
    using System;

    using ModuleInject.Interfaces.Disposing;

    public interface IRegistry : IDisposableExt
    {
        bool IsRegistered(Type type);

        object GetComponent(Type type);
    }
}
