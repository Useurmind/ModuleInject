using System.Collections.Generic;
using System.Linq;

using ModuleInject.Interfaces;
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
		public IMainComponent1 MainComponent1
		{
			get
			{
				return GetSingleInstanceWithConstruct<MainComponent1>(m =>
				{
					var c = new MainComponent1();
					c.Initialize(m.MainComponent2);
					return c;
				});
			}
		}
		public IMainComponent1 MainInstance1
		{
			get
			{
				return GetSingleInstanceWithInject<IMainComponent1, MainComponent1>((module, comp) => 
                    comp.Initialize(module.MainComponent2));
			}
		}
		public IList<IMainComponent1> MainComponent1List
		{
			get
			{
				return GetSingleInstanceWithConstruct(m => new List<IMainComponent1>() {
					m.MainComponent1,
					m.CreateMainComponent1(),
					m.CreateMainComponent1()
				});
			}
		}

		internal IMainComponent2 MainComponent2
		{
			get
			{
				return GetSingleInstance<MainComponent2>();
			}
		}

		internal IMainComponent1 CreateMainComponent1()
		{
			return GetFactory<MainComponent1>();
		}

		//[PrivateFactory]
		//internal IMainComponent2 CreateMainComponent2()
		//{
		//    return base.CreateInstance(x => x.CreateMainComponent2());
		//}

		public void RegisterComponentsByInitializeWithOtherComponent()
		{
			SingleInstance(m => m.MainComponent1List)
				.Construct<List<IMainComponent1>>()
				.Inject((module, comp) => comp.Add(module.MainComponent1))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()))
				.Inject((module, comp) => comp.Add(module.CreateMainComponent1()));
		}
	}
}
