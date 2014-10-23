using System.Linq;

namespace ModuleInject.Container.Interface
{
    using System;

    public interface IResolvedValue
    {
        object Resolve();
        Type Type { get; }
    }
}
