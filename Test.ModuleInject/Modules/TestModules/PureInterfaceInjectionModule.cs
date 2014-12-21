using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
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

    public interface IPureInterfaceInjectionModule : IModule
    {
        IMainComponent1 MainComponent1 { get; }

        IMainComponent1 CreateMainComponent1();
    }

    public class PureInterfaceInjectionModule : InjectionModule<IPureInterfaceInjectionModule, PureInterfaceInjectionModule>, IPureInterfaceInjectionModule, ISomeModuleProperties
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        public IMainComponent1 CreateMainComponent1()
        {
            return this.CreateInstance(x => x.CreateMainComponent1());
        }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set;}

        public PureInterfaceInjectionModule()
        {
            this.RegisterPrivateComponent(x => x.MainComponent2).Construct<MainComponent2>();

            this.RegisterComponentWithInjector();
        }

        public void RegisterComponentWithInjector()
        {
            this.RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .AddInjector(new PureInterfaceInjector());
        }

        public void RegisterInstanceWithInjector()
        {
            this.RegisterPublicComponent(x => x.MainComponent1)
                .Construct(new MainComponent1())
                .AddInjector(new PureInterfaceInjector());
        }

        public void RegisterFactoryWithInjector()
        {
            this.RegisterPublicComponentFactory(x => x.CreateMainComponent1())
                .Construct<MainComponent1>()
                .AddInjector(new PureInterfaceInjector());
        }
    }
}
