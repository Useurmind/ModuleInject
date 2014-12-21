using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
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
            this.RegisterPublicComponent(x => x.InterceptedWithChangeReturnValueComponent)
                .Construct<MainComponent1>()
                .AddBehaviour(new ChangeReturnValueBehaviour());
        }
    }
}
