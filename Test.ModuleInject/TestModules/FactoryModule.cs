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
            RegisterPrivateComponent(x => x.Component2).Construct<MainComponent2>();
            
            RegisterPublicComponent(x => x.InstanceComponent)
                .Construct(new MainComponent1())
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => CreateComponent2());

            RegisterPrivateComponent(x => x.Component1)
                .Construct(m => new MainComponent1() {
                    MainComponent22 = m.CreateComponent2()
                })
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => x.CreateComponent2());

            RegisterPublicComponentFactory(x => x.CreateComponent1())
                .Construct<MainComponent1>()
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent23)
                .InitializeWith(x => x.CreateComponent2());
            RegisterPublicComponentFactory(x => x.CreateComponent2()).Construct<MainComponent2>();


            RegisterPrivateComponentFactory(x => x.CreatePrivateComponent1())
                .Construct<MainComponent1>()
                .InitializeWith(x => x.CreateComponent2())
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreatePrivateComponent2()).IntoProperty(x => x.MainComponent23);
            RegisterPrivateComponentFactory(x => x.CreatePrivateComponent2()).Construct<MainComponent2>();

            RegisterPublicComponent(x => x.ComponentWithPrivateComponents)
                .Construct<MainComponent1>()
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
