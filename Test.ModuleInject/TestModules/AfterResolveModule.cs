﻿using ModuleInject;
using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IFunctionCallModule : IInjectionModule
    {
        IMainComponent1 MainComponent1 { get; }
        IMainComponent1 MainInstance1 { get; }

        IList<IMainComponent1> MainComponent1List { get; }
    }

    public class AfterResolveModule : InjectionModule<IFunctionCallModule, AfterResolveModule>, IFunctionCallModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }
        public IMainComponent1 MainInstance1 { get; private set; }
        public IList<IMainComponent1> MainComponent1List { get; private set; }

        [PrivateComponent]
        internal IMainComponent2 MainComponent2 { get; private set; }

        [PrivateFactory]
        internal IMainComponent1 CreateMainComponent1()
        {
            return base.CreateInstance(x => x.CreateMainComponent1());
        }

        //[PrivateFactory]
        //internal IMainComponent2 CreateMainComponent2()
        //{
        //    return base.CreateInstance(x => x.CreateMainComponent2());
        //}

        public void RegisterComponentsByInitializeWithOtherComponent()
        {
            RegisterPrivateComponentFactory<IMainComponent1, MainComponent1>(x => x.CreateMainComponent1());

            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainInstance1, new MainComponent1())
                .Inject((comp, module) => comp.Initialize(module.MainComponent2));

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .Inject((comp, module) => comp.Initialize(module.MainComponent2));

            RegisterPublicComponent<IList<IMainComponent1>, List<IMainComponent1>>(x => x.MainComponent1List)
                .Inject((comp, module) => comp.Add(module.MainComponent1))
                .Inject((comp, module) => comp.Add(module.CreateMainComponent1()))
                .Inject((comp, module) => comp.Add(module.CreateMainComponent1()));
        }
    }
}
