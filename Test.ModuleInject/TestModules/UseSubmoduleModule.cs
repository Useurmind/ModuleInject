using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

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
            RegisterPrivateComponent(x => x.SubModule).Construct<Submodule>();
        }

        public void RegisterMainComponent_Injecting_SubmoduleProperty()
        {
            RegisterPublicComponent(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject(x => x.SubModule.Component1)
                .IntoProperty(x => x.SubComponent1);
        }

        public void RegisterMainComponent_Injecting_SubmoduleFactory()
        {
            RegisterPublicComponent(x => x.MainComponent)
                .Construct<MainComponent1>()
                .Inject(x => x.SubModule.CreateComponent1())
                .IntoProperty(x => x.SubComponent1);
        }
    }
}
