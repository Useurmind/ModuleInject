using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal interface IMainModule : IInjectionModule
    {
        IMainComponent1 InstanceRegistrationComponent { get; set; }
        IMainComponent1 InitWithPropertiesComponent { get; set; }
        IMainComponent1 InitWithInitialize1Component { get; set; }
        IMainComponent1 InitWithInitialize1FromSubComponent { get; set; }
        IMainComponent1 InitWithInitialize2Component { get; set; }
        IMainComponent1 InitWithInitialize3Component { get; set; }
        IMainComponent1 InitWithInjectorComponent { get; set; }
        IMainComponent2 Component2 { get; set; }
        IMainComponent2 Component22 { get; set; }

        ISubModule SubModule { get; set; }
    }
}
