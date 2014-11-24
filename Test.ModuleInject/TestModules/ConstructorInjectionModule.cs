using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IConstructorInjectionModule : IInjectionModule
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
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.MainComponent3);
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2);
        }

        public void RegisterWithDefaultConstructor()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1);
        }

        public void RegisterWithArgumentsInConstructor()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallConstructor(module => new MainComponent1(module.MainComponent2));
        }

        public void RegisterWithArgumentsInConstructorTwice()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallConstructor(module => new MainComponent1(module.MainComponent2))
                .CallConstructor(module => new MainComponent1(module.MainComponent2));
        }

        public void RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallConstructor(module => new MainComponent1(module.MainComponent3));
        }
    }
}
