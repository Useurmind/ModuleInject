using System.Linq;

using Microsoft.Practices.Unity;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;

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

    public class NoAutoWiringModule : InjectionModule<INoAutoWiringModule, NoAutoWiringModule>, INoAutoWiringModule
    {
        [PrivateComponent]
        public IAutoWiringClass PrivateAutoWiringComponent { get; set; }

        [PrivateComponent]
        public IMainComponent2 PrivateComponent { get; set; }

        public NoAutoWiringModule()
        {
            this.RegisterPrivateComponent(x => x.PrivateAutoWiringComponent).Construct<AutoWiringClass>();
            this.RegisterPrivateComponent(x => x.PrivateComponent).Construct<MainComponent2>();
        }
    }
}
