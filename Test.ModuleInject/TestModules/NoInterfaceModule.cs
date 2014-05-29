using ModuleInject;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public class NoInterfaceModule : InjectionModule<NoInterfaceModule, NoInterfaceModule>, IInjectionModule
    {
    }
}
