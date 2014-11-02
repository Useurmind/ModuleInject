using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface ITestSuperModule : IInjectionModule
    {
        IPropertyModule MainModule { get; }
        ISubModule SubModule { get; }
    }

    public class TestSuperModule : InjectionModule<ITestSuperModule, TestSuperModule>, ITestSuperModule
    {
        public IPropertyModule MainModule { get; set; }
        public ISubModule SubModule { get; set; }

        public TestSuperModule()
        {
            RegisterPublicComponent<ISubModule, Submodule>(x => x.SubModule);
            RegisterPublicComponent<IPropertyModule, PropertyModule>(x => x.MainModule)
                .Inject(x => x.SubModule).IntoProperty(x => x.SubModule);
        }
    }
}
