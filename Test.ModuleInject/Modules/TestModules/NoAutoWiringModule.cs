using System.Linq;

using Microsoft.Practices.Unity;

using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IAutoWiringClass
    {
        MainComponent2 PropertyComponent { get; }
        MainComponent2 ConstructorComponent { get; }
    }

    public class AutoWiringClass : IAutoWiringClass
    {
        [Dependency]
        public MainComponent2 PropertyComponent { get; set; }
        public MainComponent2 ConstructorComponent { get; set; }

        public AutoWiringClass()
        {

        }

        public AutoWiringClass(MainComponent2 component)
        {
            this.ConstructorComponent = component;
        }
    }

    public interface INoAutoWiringModule : IModule
    {

    }

    public class NoAutoWiringModule : InjectionModule<NoAutoWiringModule>, INoAutoWiringModule
    {
        public IAutoWiringClass PrivateAutoWiringComponent { get { return GetSingleInstance(m => new AutoWiringClass()); } }
        
        public IMainComponent2 PrivateComponent { get { return GetSingleInstance(m => new MainComponent2()); } }
    }
}
