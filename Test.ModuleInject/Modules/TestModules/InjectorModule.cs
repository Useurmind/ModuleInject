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

        public InjectorModule()
        {
			component2 = SingleInstance<IMainComponent2>().Construct<MainComponent2>();
			component22 = SingleInstance<IMainComponent2>().Construct<MainComponent2>();
			component23 = SingleInstance<IMainComponent2>().Construct<MainComponent2>();
        }

        public void RegisterClassInjectorWithPropertyValueAndInitialize1()
        {
			component1 = SingleInstance<IMainComponent1>()
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
			component1 = SingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.AddInjector(new InterfaceInjector<IInjectorModule, IMainComponent1>(context =>
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
