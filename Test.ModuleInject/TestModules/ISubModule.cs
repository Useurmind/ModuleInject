using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface ISubModule : IInjectionModule
    {
        ISubComponent1 Component1 { get; }
        ISubComponent2 Component2 { get; }

        ISubComponent1 CreateComponent1();
    }
}
