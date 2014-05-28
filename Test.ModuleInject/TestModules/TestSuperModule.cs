using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface ITestSuperModule : IInjectionModule
    {
        IMainModule MainModule { get; }
        ISubModule SubModule { get; }
    }

    public class TestSuperModule : InjectionModule<ITestSuperModule, TestSuperModule>, ITestSuperModule
    {
        public IMainModule MainModule { get; set; }
        public ISubModule SubModule { get; set; }

        public TestSuperModule()
        {
            RegisterComponent<ISubModule, Submodule>(x => x.SubModule);
            RegisterComponent<IMainModule, MainModule>(x => x.MainModule)
                .Inject(x => x.SubModule).IntoProperty(x => x.SubModule);
        }
    }
}
