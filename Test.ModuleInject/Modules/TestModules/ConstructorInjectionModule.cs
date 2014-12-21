using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IConstructorInjectionModule : IModule
    {
        // don't change the order, important for test
        IMainComponent1 MainComponent1 { get; }
        IMainComponent2 MainComponent3 { get; }
    }

    public class ConstructorInjectionModule : InjectionModule<IConstructorInjectionModule, ConstructorInjectionModule>, IConstructorInjectionModule
    {
        // don't change the order, important for test
        public IMainComponent1 MainComponent1 { get; private set; }
        public IMainComponent2 MainComponent3 { get; private set; }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set; }

        public ConstructorInjectionModule()
        {
            this.RegisterPublicComponent(x => x.MainComponent3).Construct<MainComponent2>();
            this.RegisterPrivateComponent(x => x.MainComponent2).Construct<MainComponent2>();
        }

        public void RegisterWithDefaultConstructor()
        {
            this.RegisterPublicComponent(x => x.MainComponent1).Construct<MainComponent1>();
        }

        public void RegisterWithArgumentsInConstructor()
        {
            this.RegisterPublicComponent(x => x.MainComponent1)
                .Construct(module => new MainComponent1(module.MainComponent2));
        }

        public void RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis()
        {
            this.RegisterPublicComponent(x => x.MainComponent1)
                .Construct(module => new MainComponent1(module.MainComponent3));
        }
    }
}
