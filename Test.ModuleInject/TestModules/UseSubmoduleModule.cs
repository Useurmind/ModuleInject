using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Fluent;
    using global::ModuleInject.Interfaces;

    public interface IUseSubmoduleModule : IInjectionModule
    {
        IMainComponent1 MainComponent { get; set; }
    }

    public class UseSubmoduleModule : InjectionModule<IUseSubmoduleModule, UseSubmoduleModule>, IUseSubmoduleModule
    {
        public IMainComponent1 MainComponent { get; set; }

        [PrivateComponent]
        public ISubModule SubModule { get; set; }

        public UseSubmoduleModule()
        {
            RegisterPrivateComponent<ISubModule, Submodule>(x => x.SubModule);
        }

        public void RegisterMainComponent_Injecting_SubmoduleProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent)
                .Inject(x => x.SubModule.Component1)
                .IntoProperty(x => x.SubComponent1);
        }

        public void RegisterMainComponent_Injecting_SubmoduleFactory()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent)
                .Inject(x => x.SubModule.CreateComponent1())
                .IntoProperty(x => x.SubComponent1);
        }
    }
}
