using ModuleInject;
using ModuleInject.Decoration;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public class PureInterfaceInjector : InterfaceInjector<IMainComponent1Sub, ISomeModuleProperties>
    {
        public PureInterfaceInjector()
            : base(context =>
            {
                context.Inject(x => x.MainComponent2).IntoProperty(x => x.MainComponent2);
            })
        {

        }
    }

    public interface ISomeModuleProperties
    {
        IMainComponent2 MainComponent2 { get; }
    }

    public interface IPureInterfaceInjectionModule : IInjectionModule
    {
        IMainComponent1 MainComponent1 { get; }

        IMainComponent1 CreateMainComponent1();
    }

    public class PureInterfaceInjectionModule : InjectionModule<IPureInterfaceInjectionModule, PureInterfaceInjectionModule>, IPureInterfaceInjectionModule, ISomeModuleProperties
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        public IMainComponent1 CreateMainComponent1()
        {
            return CreateInstance(x => x.CreateMainComponent1());
        }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set;}

        public PureInterfaceInjectionModule()
        {
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2);

            RegisterComponentWithInjector();
        }

        public void RegisterComponentWithInjector()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .AddInjector(new PureInterfaceInjector());
        }

        public void RegisterInstanceWithInjector()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1, new MainComponent1())
                .AddInjector(new PureInterfaceInjector());
        }

        public void RegisterFactoryWithInjector()
        {
            RegisterPublicComponentFactory<IMainComponent1, MainComponent1>(x => x.CreateMainComponent1())
                .AddInjector(new PureInterfaceInjector());
        }
    }
}
