using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface ITestSuperModule : IModule
    {
        PropertyModule MainModule { get; }
        ISubModule SubModule { get; }
    }

    public class TestSuperModule : InjectionModule<ITestSuperModule, TestSuperModule>, ITestSuperModule
    {
        public PropertyModule MainModule { get; set; }
        public ISubModule SubModule { get; set; }

        public TestSuperModule()
        {
            this.RegisterPublicComponent(x => x.SubModule).Construct<Submodule>();
            this.RegisterPublicComponent(x => x.MainModule)
                .Construct<PropertyModule>()
                .Inject(x => x.SubModule).IntoProperty(x => x.SubModule);
        }
    }
}
