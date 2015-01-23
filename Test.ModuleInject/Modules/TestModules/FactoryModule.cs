using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IFactoryModule : IModule
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
            this.RegisterPrivateComponent(x => x.Component2).Construct<MainComponent2>();
            
            this.RegisterPublicComponent(x => x.InstanceComponent)
                .Construct(new MainComponent1())
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .Inject((c, m) => c.Initialize(m.CreateComponent2()));

            this.RegisterPrivateComponent(x => x.Component1)
                .Construct(m => new MainComponent1() {
                    MainComponent22 = m.CreateComponent2()
                })
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent23)
                .Inject((c, m) => c.Initialize(m.CreateComponent2()));

            this.RegisterPublicComponentFactory(x => x.CreateComponent1())
                .Construct<MainComponent1>()
                .Inject(x => x.CreateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent23)
                .Inject((c, m) => c.Initialize(m.CreateComponent2()));
            this.RegisterPublicComponentFactory(x => x.CreateComponent2()).Construct<MainComponent2>();


            this.RegisterPrivateComponentFactory(x => x.CreatePrivateComponent1())
                .Construct<MainComponent1>()
                .Inject((c, m) => c.Initialize(m.CreateComponent2()))
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.CreatePrivateComponent2()).IntoProperty(x => x.MainComponent23);
            this.RegisterPrivateComponentFactory(x => x.CreatePrivateComponent2()).Construct<MainComponent2>();

            this.RegisterPublicComponent(x => x.ComponentWithPrivateComponents)
                .Construct<MainComponent1>()
                .Inject(x => x.CreatePrivateComponent2()).IntoProperty(x => x.MainComponent22)
                .Inject((c, m) => c.Initialize(m.CreatePrivateComponent2()));
                
        }

        public IMainComponent1 CreateComponent1()
        {
            return this.CreateInstance(x => x.CreateComponent1());
        }

        public IMainComponent2 CreateComponent2()
        {
            return this.CreateInstance(x => x.CreateComponent2());
        }

        [PrivateFactory]
        public IMainComponent1 CreatePrivateComponent1()
        {
            return this.CreateInstance(x => x.CreatePrivateComponent1());
        }

        [PrivateFactory]
        public IMainComponent2 CreatePrivateComponent2()
        {
            return this.CreateInstance(x => x.CreatePrivateComponent2());
        }

        public IMainComponent1 RetrievePrivateComponent1()
        {
            return this.Component1;
        }

        public IMainComponent2 RetrievePrivateComponent2()
        {
            return this.Component2;
        }
    }
}
