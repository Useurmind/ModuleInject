using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Fluent;
    using global::ModuleInject.Interfaces;

    public interface IPropertyInjectionModule : IInjectionModule
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
            RegisterPrivateComponent<MainComponent2, MainComponent2>(x => x.MainComponent2);
        }
        
        public void RegisterPublicComponentAndInjectNewInstanceInProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicMainComponent1)
                .Inject(new MainComponent2())
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicComponentAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicMainComponent1)
                .Inject(x => x.MainComponent2)
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicInstanceAndInjectNewInstanceInProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicMainComponent1, new MainComponent1())
                .Inject(new MainComponent2())
                .IntoProperty(x => x.MainComponent2);
        }

        public void RegisterPublicInstanceAndInjectRegisteredComponentWithoutInterfaceInProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicMainComponent1, new MainComponent1())
                .Inject(x => x.MainComponent2)
                .IntoProperty(x => x.MainComponent2);
        }
    }
}
