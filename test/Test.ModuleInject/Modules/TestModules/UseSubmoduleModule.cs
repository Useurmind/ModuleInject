using System.Linq;

using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IUseSubmoduleModule : IModule
    {
        IMainComponent1 MainComponent { get; }
    }

    public class UseSubmoduleModule : InjectionModule<UseSubmoduleModule>, IUseSubmoduleModule
    {
        public IMainComponent1 MainComponent { get { return Get<IMainComponent1>(); } }
        
        public ISubModule SubModule { get { return GetSingleInstance<Submodule>(); } }
        
        public void RegisterMainComponent_Injecting_SubmoduleProperty()
        {
            this.SingleInstance(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.SubComponent1 = m.SubModule.Component1);
        }

        public void RegisterMainComponent_Injecting_SubmoduleFactory()
        {
            this.SingleInstance(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.SubComponent1 = m.SubModule.CreateComponent1());
        }
    }
}
