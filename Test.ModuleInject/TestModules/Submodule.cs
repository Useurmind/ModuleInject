using ModuleInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal class Submodule : InjectionModule<ISubModule, Submodule>, ISubModule
    {
        public ISubComponent1 Component1 { get; set; }
        public ISubComponent2 Component2 { get; set; }

        public Submodule()
        {
            RegisterPublicComponent<ISubComponent1, SubComponent1>(x => x.Component1);
            RegisterPublicComponent<ISubComponent2, SubComponent2>(x => x.Component2);
        }

        public ISubComponent1 CreateComponent1()
        {
            return CreateInstance(x => x.CreateComponent1());
        }
    }
}
