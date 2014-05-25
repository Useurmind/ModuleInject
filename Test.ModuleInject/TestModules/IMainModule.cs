using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal interface IMainModule : IInjectionModule
    {
        IMainComponent1 Component1 { get; set; }
        IMainComponent1 SecondComponent1 { get; set; }
        IMainComponent2 Component2 { get; set; }

        ISubModule SubModule { get; set; }
    }
}
