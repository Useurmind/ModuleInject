using System.Linq;

using ModuleInject.Modules;

namespace Test.ModuleInject.Modules.TestModules
{
    internal class Submodule : InjectionModule<ISubModule, Submodule>, ISubModule
    {
        public ISubComponent1 Component1 { get; set; }
        public ISubComponent2 Component2 { get; set; }

        public Submodule()
        {
            this.RegisterPublicComponent(x => x.Component1).Construct<SubComponent1>();
            this.RegisterPublicComponent(x => x.Component2).Construct<SubComponent2>();
            this.RegisterPublicComponentFactory(x => x.CreateComponent1()).Construct<SubComponent1>();
        }

        public ISubComponent1 CreateComponent1()
        {
            return this.CreateInstance(x => x.CreateComponent1());
        }
    }
}
