using System.Linq;
using ModuleInject.Injection;
using ModuleInject.Modules;

namespace Test.ModuleInject.Modules.TestModules
{
    internal class Submodule : InjectionModule<Submodule>, ISubModule
    {
        public ISubComponent1 Component1 { get { return GetSingleInstance<SubComponent1>(); } }
        public ISubComponent2 Component2 { get { return GetSingleInstance<SubComponent2>(); } }

        public ISubComponent1 CreateComponent1()
        {
            return this.GetFactory<SubComponent1>();
        }
    }
}
