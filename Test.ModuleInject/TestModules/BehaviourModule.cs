using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Interfaces;

    public interface IBehaviourModule : IInjectionModule
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
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InterceptedWithChangeReturnValueComponent)
                .AddBehaviour(new ChangeReturnValueBehaviour());
        }
    }
}
