using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IInjectorModule : IModule
    {
        IMainComponent1 Component1 { get; }

        IMainComponent2 Component2 { get; }
        IMainComponent2 Component22 { get; }
        IMainComponent2 Component23 { get; }
    }
    public class MainComponent2Wrapper : IMainComponent2
    {
        public IMainComponent2 WrappedComponent { get; set; }

        public IMainComponent2SubInterface Component2Sub { get; set; }

        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        public ISubComponent2 SubComponent2 { get; set; }
    }

    public class InjectorModule : InjectionModule<InjectorModule>, IInjectorModule
    {
		private ISourceOf<IMainComponent1> component1;
		private ISourceOf<IMainComponent2> component2;
		private ISourceOf<IMainComponent2> component22;
		private ISourceOf<IMainComponent2> component23;

		public IMainComponent1 Component1 { get { return component1.Get(); } }

        public IMainComponent2 Component2 { get { return component2.Get(); } }
        public IMainComponent2 Component22 { get { return component22.Get(); } }
        public IMainComponent2 Component23 { get { return component23.Get(); } }
        
        public IMainComponent2 WrappedComponent
        {
            get
            {
                return GetSingleInstance<IMainComponent2>(cc =>
                {
                    cc.Construct(m => m.Component2)
                        .AddInjector(new ExactInterfaceInjector<IInjectorModule, IMainComponent2>(register =>
                        {
                            register.Change(c => new MainComponent2Wrapper()
                            {
                                WrappedComponent = c,
                                IntProperty = 2
                            });
                        }));
                });
            }
        }

        public InjectorModule()
        {
			component2 = CreateSingleInstance<IMainComponent2>().Construct<MainComponent2>();
			component22 = CreateSingleInstance<IMainComponent2>().Construct<MainComponent2>();
			component23 = CreateSingleInstance<IMainComponent2>().Construct<MainComponent2>();
        }

        public void RegisterClassInjectorWithPropertyValueAndInitialize1()
        {
			component1 = CreateSingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.AddInjector(new InterfaceInjector<InjectorModule, MainComponent1>(context =>
				{
					context.Inject((m, c) => c.Initialize(m.Component2));
				}))
				.AddInjector(new InterfaceInjector<InjectorModule, MainComponent1>(context =>
				{
					context.Inject((m, c) => c.MainComponent22 = m.Component22);
				}))
				.AddInjector(new InterfaceInjector<InjectorModule, MainComponent1>(context =>
				{
					context.Inject((m, c) => c.MainComponent23 = new MainComponent2());
				}));
        }

        public void RegisterInterfaceInjectorWithPropertyValueAndInitialize1()
        {
			component1 = CreateSingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.AddInjector(new ExactInterfaceInjector<IInjectorModule, IMainComponent1>(context =>
				{
					context.Inject((m, c) => c.Initialize(m.Component2));
				}))
				.AddInjector(new InterfaceInjector<IInjectorModule, IMainComponent1>(context =>
				{
					context.Inject((m, c) => c.MainComponent22 = m.Component22);
				}))
				.AddInjector(new InterfaceInjector<IInjectorModule, IMainComponent1>(context =>
				{
					context.Inject((m, c) => c.MainComponent23 = new MainComponent2());
				}));
        }
    }
}
