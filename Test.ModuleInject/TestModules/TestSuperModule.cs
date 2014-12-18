using ModuleInject;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface ITestSuperModule : IModule
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
            RegisterPublicComponent(x => x.SubModule).Construct<Submodule>();
            RegisterPublicComponent(x => x.MainModule)
                .Construct<PropertyModule>()
                .Inject(x => x.SubModule).IntoProperty(x => x.SubModule);
        }
    }
}
