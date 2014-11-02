using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface ISubPropertyInjectionModule : IInjectionModule
    {
        ISubComponent2 SubComponent2 { get; set; }
        ISubComponent1 SubComponent1 { get; set; }
        IMainComponent2 MainComponent2 { get; set; }
    }

    public class SubPropertyInjectionModule : InjectionModule<ISubPropertyInjectionModule, SubPropertyInjectionModule>, ISubPropertyInjectionModule
    {
        public ISubComponent2 SubComponent2 { get; set; }
        public ISubComponent1 SubComponent1 { get; set; }
        public IMainComponent2 MainComponent2 { get; set; }

        public void SetupSubComponent2OfMainComponent2IsInjectedIntoSubComponent1()
        {
            RegisterPublicComponent<ISubComponent2, SubComponent2>(x => x.SubComponent2);
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2)
                .Inject(x => x.SubComponent2).IntoProperty(x => x.SubComponent2);
            RegisterPublicComponent<ISubComponent1, SubComponent1>(x => x.SubComponent1)
                .Inject(x => x.MainComponent2.SubComponent2).IntoProperty(x => x.SubComponent2);
        }
    }
}
