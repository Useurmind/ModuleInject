﻿using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IBehaviourModule : IModule
    {
        IMainComponent1 InterceptedWithChangeReturnValueComponent { get; }
    }

    public class BehaviourModule : InjectionModule<IBehaviourModule, BehaviourModule>, IBehaviourModule
    {
        public IMainComponent1 InterceptedWithChangeReturnValueComponent { get; set; }

        public BehaviourModule()
        {
        }

        public void RegisterBehaviour()
        {
            RegisterPublicComponent(x => x.InterceptedWithChangeReturnValueComponent)
                .Construct<MainComponent1>()
                .AddBehaviour(new ChangeReturnValueBehaviour());
        }
    }
}
