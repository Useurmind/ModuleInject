﻿using ModuleInject;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IBaseModule : IInjectionModule
    {
        IMainComponent1 MainComponent1 { get; }
    }

    public class BaseModule<IDerivedModule, TDerivedModule> : InjectionModule<IDerivedModule, TDerivedModule>, IBaseModule
        where TDerivedModule : BaseModule<IDerivedModule, TDerivedModule>, IDerivedModule
        where IDerivedModule : IBaseModule
    {
        public IMainComponent1 MainComponent1 { get; set; }

        public BaseModule()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1);
        }
    }

    public interface IDerivedModule : IBaseModule
    {
        IMainComponent2 MainComponent2 { get; }
    }

    public class DerivedModule : BaseModule<IDerivedModule, DerivedModule>, IDerivedModule
    {
        public IMainComponent2 MainComponent2 { get; set; }

        public DerivedModule()
        {
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2);
        }
    }
}
