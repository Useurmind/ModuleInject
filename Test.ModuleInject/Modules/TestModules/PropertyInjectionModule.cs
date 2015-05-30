using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Injection;
using ModuleInject.Interfaces;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IPropertyInjectionModule : IModule
    {
        IMainComponent1 PublicMainComponent1 { get;  }
    }

    public class PropertyInjectionModule : InjectionModule<PropertyInjectionModule>, IPropertyInjectionModule
    {
        public IMainComponent1 PublicMainComponent1 { get { return Get<IMainComponent1>(); } }
        
        public MainComponent2 MainComponent2 { get { return GetSingleInstance<MainComponent2>(); } }
                
        public void RegisterPublicComponentAndInjectNewInstanceInProperty()
        {
            this.SingleInstance(x => x.PublicMainComponent1)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = new MainComponent2());
        }

        public void RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            this.SingleInstance(x => x.PublicMainComponent1)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = m.MainComponent2);
        }

        public void RegisterPublicInstanceAndInjectNewInstanceInProperty()
        {
            this.SingleInstance(x => x.PublicMainComponent1)
                .Construct(m => new MainComponent1())
                .Inject((m, c) => c.MainComponent2 = new MainComponent2());
        }

        public void RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            this.SingleInstance(x => x.PublicMainComponent1)
                .Construct(m => new MainComponent1())
                .Inject((m, c) => c.MainComponent2 = m.MainComponent2);
        }
    }
}
