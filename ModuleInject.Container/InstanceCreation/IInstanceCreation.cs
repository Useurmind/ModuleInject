using System.Linq;

namespace ModuleInject.Container.InstanceCreation
{
    using System;

    public interface IInstanceCreation
    {
        object Resolve(Type actualType);
    }
}
