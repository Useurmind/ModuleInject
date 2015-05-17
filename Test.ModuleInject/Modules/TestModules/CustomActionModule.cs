using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;
using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
	internal class CustomActionModule : InjectionModule<CustomActionModule>, IEmptyModule
	{
		public ISourceOf<IMainComponent1> mainComponent1;

		public IMainComponent1 MainComponent1 { get { return mainComponent1.GetInstance(); } }

		public IMainComponent2 MainComponent2 { get; private set; }

		public CustomActionModule()
		{
			this.MainComponent2 = new MainComponent2();
		}

		public void RegisterWithClosure_ConstructFromType()
		{
			mainComponent1 = SingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.Inject((m, c) => c.MainComponent2 = this.MainComponent2);
		}

		public void RegisterInInterfaceInjector_ConstructFromInstance()
		{
			mainComponent1 = SingleInstance<IMainComponent1>()
				 .Construct<MainComponent1>()
				 .AddInjector(new InterfaceInjector<IEmptyModule, IMainComponent1>(
					(context) =>
					{
						context.Inject((m, c) => c.MainComponent2 = this.MainComponent2);
					}));
		}
	}
}
