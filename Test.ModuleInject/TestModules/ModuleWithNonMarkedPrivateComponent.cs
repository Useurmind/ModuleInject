using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject;

    interface IModuleWithNonMarkedPrivateComponent : IInjectionModule
    {
         
    }

    class ModuleWithNonMarkedPrivateComponent : InjectionModule<IModuleWithNonMarkedPrivateComponent, ModuleWithNonMarkedPrivateComponent>, IModuleWithNonMarkedPrivateComponent
    {
        // unmarked private component
        public int UnmarkedPrivateComponent { get; set; }

    }
}
