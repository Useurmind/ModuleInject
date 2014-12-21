using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IPropertyInjectionModule : IModule
    {
        IMainComponent1 PublicMainComponent1 { get;  }
    }

    public class PropertyInjectionModule : InjectionModule<IPropertyInjectionModule, PropertyInjectionModule>, IPropertyInjectionModule
    {
        public IMainComponent1 PublicMainComponent1 { get; private set; }

        [PrivateComponent]
        public MainComponent2 MainComponent2 { get; private set; }

        public PropertyInjectionModule()
        {
            this.RegisterPrivateComponent(x => x.MainComponent2).Construct<MainComponent2>();
        }
        
        public void RegisterPublicComponentAndInjectNewInstanceInProperty()
        {
            this.RegisterPublicComponent(x => x.PublicMainComponent1)
                .Construct<MainComponent1>()
                .Inject(new MainComponent2())
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            this.RegisterPublicComponent(x => x.PublicMainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.MainComponent2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicInstanceAndInjectNewInstanceInProperty()
        {
            this.RegisterPublicComponent(x => x.PublicMainComponent1)
                .Construct(new MainComponent1())
                .Inject(new MainComponent2())
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            this.RegisterPublicComponent(x => x.PublicMainComponent1)
                .Construct(new MainComponent1())
                .Inject(x => x.MainComponent2)
                .IntoProperty(x => x.MainComponent2);
        }
    }
}
