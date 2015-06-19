using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
    public class PureInterfaceInjector : InterfaceInjector<ISomeModuleProperties, IMainComponent1Sub>
    {
        public PureInterfaceInjector()
            : base(context =>
            {
                context.Inject((m, c) => c.MainComponent2 = m.MainComponent2);
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

    public class PureInterfaceInjectionModule : InjectionModule<PureInterfaceInjectionModule>, IPureInterfaceInjectionModule, ISomeModuleProperties
    {
		private ISourceOf<IMainComponent1> mainComponent1;
		private ISourceOf<IMainComponent1> mainComponent1Factory;
		private ISourceOf<IMainComponent2> mainComponent2;

		public IMainComponent1 MainComponent1 { get { return mainComponent1.Get(); } }

        public IMainComponent1 CreateMainComponent1()
        {
			return mainComponent1Factory.Get();
        }
		
        public IMainComponent2 MainComponent2 { get { return mainComponent2.Get(); } }

		public PureInterfaceInjectionModule()
        {
			mainComponent2 = CreateSingleInstance<IMainComponent2>().Construct<MainComponent2>();

            this.RegisterComponentWithInjector();
        }

        public void RegisterComponentWithInjector()
        {
			mainComponent1 = CreateSingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.AddInjector(new PureInterfaceInjector());
        }

        public void RegisterInstanceWithInjector()
        {
			mainComponent1 = CreateSingleInstance<IMainComponent1>()
				.Construct(m => new MainComponent1())
				.AddInjector(new PureInterfaceInjector());
        }

        public void RegisterFactoryWithInjector()
        {
			mainComponent1Factory = CreateFactory<IMainComponent1>()
				.Construct<MainComponent1>()
				.AddInjector(new PureInterfaceInjector());
        }
    }
}
