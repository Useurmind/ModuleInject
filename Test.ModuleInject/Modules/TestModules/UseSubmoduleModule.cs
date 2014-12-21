using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IUseSubmoduleModule : IModule
    {
        IMainComponent1 MainComponent { get; }
    }

    public class UseSubmoduleModule : InjectionModule<IUseSubmoduleModule, UseSubmoduleModule>, IUseSubmoduleModule
    {
        public IMainComponent1 MainComponent { get; private set; }

        [PrivateComponent]
        public ISubModule SubModule { get; set; }

        public UseSubmoduleModule()
        {
            this.RegisterPrivateComponent(x => x.SubModule).Construct<Submodule>();
        }

        public void RegisterMainComponent_Injecting_SubmoduleProperty()
        {
            this.RegisterPublicComponent(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject(x => x.SubModule.Component1)
                .IntoProperty(x => x.SubComponent1);
        }

        public void RegisterMainComponent_Injecting_SubmoduleFactory()
        {
            this.RegisterPublicComponent(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject(x => x.SubModule.CreateComponent1())
                .IntoProperty(x => x.SubComponent1);
        }
    }
}
