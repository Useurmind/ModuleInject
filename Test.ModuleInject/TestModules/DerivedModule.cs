using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IBaseModule : IInjectionModule
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
            RegisterPublicComponent(x => x.MainComponent1).Construct<MainComponent1>();
            RegisterPrivateComponent(x => x.MainComponent1Private).Construct<MainComponent1>();
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
            RegisterPublicComponent(x => x.MainComponent2).Construct<MainComponent2>();
            RegisterPrivateComponent(x => x.MainComponent2Private).Construct<MainComponent2>();
        }
    }
}
