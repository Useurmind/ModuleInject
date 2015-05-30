using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IBaseModule : IModule
    {
        IMainComponent1 MainComponent1 { get; }
    }

    public class BaseModule<TDerivedModule> : InjectionModule<TDerivedModule>, IBaseModule
        where TDerivedModule : BaseModule<TDerivedModule>, IDerivedModule
    {
        public IMainComponent1 MainComponent1 { get { return GetSingleInstance<MainComponent1>(); } }
		
        public IMainComponent1 MainComponent1Private { get { return GetSingleInstance<MainComponent1>(); } }
    }

    public interface IDerivedModule : IBaseModule
    {
        IMainComponent2 MainComponent2 { get; }
    }

    public class DerivedModule : BaseModule<DerivedModule>, IDerivedModule
    {
        public IMainComponent2 MainComponent2 { get { return GetSingleInstance<MainComponent2>(); } }
		
        public IMainComponent2 MainComponent2Private { get { return GetSingleInstance<MainComponent2>(); } }
    }
}
