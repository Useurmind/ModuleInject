using System.Linq;

namespace ModuleInject.Container.Interface
{
    using System;
    
    public interface IContainerRegistration
    {
        Type ActualType { get; }
        Type RegisteredType { get; }
        string Name { get; }
        ILifetime Lifetime { get; }

        object Resolve();
    }
}
