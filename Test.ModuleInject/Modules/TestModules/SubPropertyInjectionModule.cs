﻿using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface ISubPropertyInjectionModule : IModule
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
            this.RegisterPublicComponent(x => x.SubComponent2).Construct<SubComponent2>();
            this.RegisterPublicComponent(x => x.MainComponent2).Construct<MainComponent2>()
                .Inject(x => x.SubComponent2).IntoProperty(x => x.SubComponent2);
            this.RegisterPublicComponent(x => x.SubComponent1)
                .Construct<SubComponent1>()
                .Inject(x => x.MainComponent2.SubComponent2).IntoProperty(x => x.SubComponent2);
        }
    }
}