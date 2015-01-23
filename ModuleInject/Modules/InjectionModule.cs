using System;
using System.Linq;

using ModuleInject.Container.Interface;
using ModuleInject.Interfaces;

namespace ModuleInject.Modules
{
    public abstract class InjectionModule : Modularity.Module, IDisposable
    {
        internal abstract Type ModuleInterface { get; }
        internal abstract Type ModuleType { get; }

        internal abstract IDependencyContainer Container { get; }

        internal abstract void OnComponentResolved(ObjectResolvedContext context);
    }
}
