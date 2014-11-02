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
        IMainComponent1 MainComponent1 { get; }
    }

    public class ConstructorInjectionModule : InjectionModule<IConstructorInjectionModule, ConstructorInjectionModule>, IConstructorInjectionModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set; }

        public ConstructorInjectionModule()
        {
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
    }
}
