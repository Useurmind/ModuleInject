using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;

namespace Test.ModuleInject.Modules.TestModules
{
    public class NoInterfaceModule : InjectionModule<NoInterfaceModule, NoInterfaceModule>, IModule
    {
    }
}
