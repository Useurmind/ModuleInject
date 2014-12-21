using System.Linq;

using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface ISubModule : IModule
    {
        ISubComponent1 Component1 { get; }
        ISubComponent2 Component2 { get; }

        ISubComponent1 CreateComponent1();
    }
}
