using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    interface IDelegateInjectionComponent
    {
        IMainComponent1 CreateMainComponent1();
    }

    class DelegateInjectionComponent : IDelegateInjectionComponent
    {
        Func<IMainComponent1> createMainComponent;

        public DelegateInjectionComponent(Func<IMainComponent1> createMainComponent)
        {
            this.createMainComponent = createMainComponent;
        }

        public IMainComponent1 CreateMainComponent1()
        {
            return createMainComponent();
        }
    }

    class DelegateInjectionModule : InjectionModule<IEmptyModule, DelegateInjectionModule>, IEmptyModule
    {
        [PrivateComponent]
        public IMainComponent1 Component { get; private set; }

        [PrivateComponent]
        public IDelegateInjectionComponent DelegateInjectionComponent { get; private set; }

        public DelegateInjectionModule()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component);
        }

        public void RegisterWithFuncReturningConstantValue()
        {
            RegisterPrivateComponent<IDelegateInjectionComponent, DelegateInjectionComponent>(x => x.DelegateInjectionComponent)
                .CallConstructor(mod => new DelegateInjectionComponent(() => new MainComponent1()));
        }
    }
}
