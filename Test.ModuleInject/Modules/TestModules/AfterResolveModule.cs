using System.Collections.Generic;
using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;
using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
	public interface IFunctionCallModule : IModule
	{
		IMainComponent1 MainComponent1 { get; }
		IMainComponent1 MainInstance1 { get; }

		IList<IMainComponent1> MainComponent1List { get; }
	}

	public class AfterResolveModule : InjectionModule<AfterResolveModule>, IFunctionCallModule
	{
		private ISourceOf<IMainComponent1> mainComponent1;
		private ISourceOf<IMainComponent1> mainInstance1;
		private ISourceOf<IList<IMainComponent1>> mainComponent1List;

		private ISourceOf<IMainComponent2> mainComponent2;

		private ISourceOf<IMainComponent1> createMainComponent1;

		public IMainComponent1 MainComponent1 { get { return mainComponent1.Get(); } }
		public IMainComponent1 MainInstance1 { get { return mainInstance1.Get(); } }
		public IList<IMainComponent1> MainComponent1List { get { return mainComponent1List.Get(); } }

		internal IMainComponent2 MainComponent2 { get { return mainComponent2.Get(); } }

		internal IMainComponent1 CreateMainComponent1()
		{
			return createMainComponent1.Get();
		}

		//[PrivateFactory]
		//internal IMainComponent2 CreateMainComponent2()
		//{
		//    return base.CreateInstance(x => x.CreateMainComponent2());
		//}

		public void RegisterComponentsByInitializeWithOtherComponent()
		{
			mainComponent1 = SingleInstance<IMainComponent1>()
				.Construct<MainComponent1>()
				.Inject((module, comp) => comp.Initialize(module.MainComponent2));

			createMainComponent1 = Factory<IMainComponent1>().Construct<MainComponent1>();

			mainComponent2 = SingleInstance<IMainComponent2>().Construct<MainComponent2>();

			mainInstance1 = SingleInstance<IMainComponent1>()
				.Construct(m => new MainComponent1())
				.Inject((module, comp) => comp.Initialize(module.MainComponent2));

			mainComponent1List = SingleInstance<IList<IMainComponent1>>()
				.Construct<List<IMainComponent1>>()
				.Inject((module, comp) => comp.Add(module.MainComponent1))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()));
		}
	}
}
