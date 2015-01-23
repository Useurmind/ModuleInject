using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IBaseModule : IModule
    {
        IMainComponent1 MainComponent1 { get; }
    }

    public class BaseModule<IDerivedModule, TDerivedModule> : InjectionModule<IDerivedModule, TDerivedModule>, IBaseModule
        where TDerivedModule : BaseModule<IDerivedModule, TDerivedModule>, IDerivedModule
        where IDerivedModule : IBaseModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        [PrivateComponent]
        public IMainComponent1 MainComponent1Private { get; private set; }

        public BaseModule()
        {
            this.RegisterPublicComponent(x => x.MainComponent1).Construct<MainComponent1>();
            this.RegisterPrivateComponent(x => x.MainComponent1Private).Construct<MainComponent1>();
        }
    }

    public interface IDerivedModule : IBaseModule
    {
        IMainComponent2 MainComponent2 { get; }
    }

    public class DerivedModule : BaseModule<IDerivedModule, DerivedModule>, IDerivedModule
    {
        public IMainComponent2 MainComponent2 { get; private set; }

        [PrivateComponent]
        public IMainComponent2 MainComponent2Private { get; private set; }

        public DerivedModule()
        {
            this.RegisterPublicComponent(x => x.MainComponent2).Construct<MainComponent2>();
            this.RegisterPrivateComponent(x => x.MainComponent2Private).Construct<MainComponent2>();
        }
    }
}
