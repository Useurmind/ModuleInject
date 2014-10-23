using System.Linq;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using System;

    using global::ModuleInject.Container;

    public interface IInjectionTestSpec : INSpec
    {
        string Name { get; }
        Type RegisteredType { get; }
        DependencyContainer Container { get; }
    }
}