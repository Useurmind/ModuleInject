using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
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
                .AddBehaviour<ChangeReturnValueBehaviour>();
        }

        public new void ActivateInterception()
        {
            base.ActivateInterception();
        }
    }
}
