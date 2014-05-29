using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMainModule : IInjectionModule
    {
        IMainComponent1 InstanceRegistrationComponent { get; }
        IMainComponent1 InitWithPropertiesComponent { get; }
        IMainComponent1 InitWithInitialize1Component { get; }
        IMainComponent1 InitWithInitialize1FromSubComponent { get; }
        IMainComponent1 InitWithInitialize2Component { get; }
        IMainComponent1 InitWithInitialize3Component { get;}
        IMainComponent1 InitWithInjectorComponent { get; }
        IMainComponent2 Component2 { get; }
        IMainComponent2 Component22 { get;  }

        ISubModule SubModule { get; set; }

        IMainComponent2 CreateComponent2();
    }
}
