using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;

    public interface IFactoryModule : IInjectionModule
    {
        IMainComponent1 InstanceComponent { get; set; }

        IMainComponent1 ComponentWithPrivateComponents { get; set; }

        IMainComponent1 CreateComponent1();
        IMainComponent2 CreateComponent2();
    }

    public class FactoryModule : InjectionModule<IFactoryModule, FactoryModule>, IFactoryModule
    {
        public IMainComponent1 InstanceComponent { get; set; }
        public IMainComponent1 ComponentWithPrivateComponents { get; set; }

        [PrivateComponent]
        private IMainComponent1 Component1 { get; set; }
        [PrivateComponent]
        private IMainComponent2 Component2 { get; set; }

        public FactoryModule()
        {
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.Component2);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InstanceComponent, new MainComponent1())
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => CreateComponent2());

            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component1)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => x.CreateComponent2());

            RegisterPublicComponentFactory<IMainComponent1, MainComponent1>(x => x.CreateComponent1())
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => x.CreateComponent2());
            RegisterPublicComponentFactory<IMainComponent2, MainComponent2>(x => x.CreateComponent2());


            RegisterPrivateComponentFactory<IMainComponent1, MainComponent1>(x => x.CreatePrivateComponent1())
                .InitializeWith(x => x.CreateComponent2())
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreatePrivateComponent2()).IntoProperty(x => x.MainComponent23);
            RegisterPrivateComponentFactory<IMainComponent2, MainComponent2>(x => x.CreatePrivateComponent2());

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.ComponentWithPrivateComponents)
                .Inject(x => x.CreatePrivateComponent2()).IntoProperty(x => x.MainComponent22)
                .InitializeWith(x => x.CreatePrivateComponent2());
                
        }

        public IMainComponent1 CreateComponent1()
        {
            return CreateInstance(x => x.CreateComponent1());
        }

        public IMainComponent2 CreateComponent2()
        {
            return CreateInstance(x => x.CreateComponent2());
        }

        [PrivateFactory]
        public IMainComponent1 CreatePrivateComponent1()
        {
            return CreateInstance(x => x.CreatePrivateComponent1());
        }

        [PrivateFactory]
        public IMainComponent2 CreatePrivateComponent2()
        {
            return CreateInstance(x => x.CreatePrivateComponent2());
        }

        public IMainComponent1 RetrievePrivateComponent1()
        {
            return Component1;
        }

        public IMainComponent2 RetrievePrivateComponent2()
        {
            return Component2;
        }
    }
}
