using System.Linq;

namespace ModuleInject.Container.Interface
{
    using System;

    public interface IInstanceCreation
    {
        object Resolve(Type actualType);
    }
}
