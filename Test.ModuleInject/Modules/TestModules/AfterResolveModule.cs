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
		public IMainComponent1 MainComponent1 { get { return Get(m => m.MainComponent1); } }
		public IMainComponent1 MainInstance1 { get { return Get(m => m.MainInstance1); } }
		public IList<IMainComponent1> MainComponent1List { get { return Get(m => m.MainComponent1List); } }

		internal IMainComponent2 MainComponent2 { get { return Get(m => m.MainComponent2); } }

		internal IMainComponent1 CreateMainComponent1()
		{
			return Get(m => m.CreateMainComponent1());
		}

		//[PrivateFactory]
		//internal IMainComponent2 CreateMainComponent2()
		//{
		//    return base.CreateInstance(x => x.CreateMainComponent2());
		//}

		public void RegisterComponentsByInitializeWithOtherComponent()
		{
			SingleInstance(m => m.MainComponent1)
				.Construct<MainComponent1>()
				.Inject((module, comp) => comp.Initialize(module.MainComponent2));

			Factory(m => m.CreateMainComponent1()).Construct<MainComponent1>();

			SingleInstance(m => m.MainComponent2).Construct<MainComponent2>();

			SingleInstance(m => m.MainInstance1)
				.Construct(m => new MainComponent1())
				.Inject((module, comp) => comp.Initialize(module.MainComponent2));

			SingleInstance(m => m.MainComponent1List)
				.Construct<List<IMainComponent1>>()
				.Inject((module, comp) => comp.Add(module.MainComponent1))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()));
		}
	}
}
