using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;
using ModuleInject.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
	public interface IFactoryModule : IModule
	{
		IMainComponent1 InstanceComponent { get; }

		IMainComponent1 ComponentWithPrivateComponents { get; }

		IMainComponent1 CreateComponent1();
		IMainComponent2 CreateComponent2();
	}

	public class FactoryModule : InjectionModule<FactoryModule>, IFactoryModule
	{
		public IMainComponent1 InstanceComponent
		{
			get
			{
				return GetSingleInstance<IMainComponent1>(cc =>
				{
					cc.Construct(m => new MainComponent1())
					.Inject((m, c) => c.MainComponent22 = m.CreateComponent2())
					.Inject((m, c) => c.MainComponent23 = m.CreateComponent2())
					.Inject((m, c) => c.Initialize(m.CreateComponent2()));
				});
			}
		}
		public IMainComponent1 ComponentWithPrivateComponents
		{
			get
			{
				return GetSingleInstance<IMainComponent1>(cc =>
				{
					cc.Construct<MainComponent1>()
					.Inject((m, c) =>
					{
						c.MainComponent22 = m.CreatePrivateComponent2();
						c.Initialize(m.CreatePrivateComponent2());
					});
				});
			}
		}

		private IMainComponent1 Component1
		{
			get
			{
				return GetSingleInstance(m =>
				{
					var c = new MainComponent1()
					{
						MainComponent22 = m.CreateComponent2()
					};
					c.MainComponent23 = m.CreateComponent2();
					c.Initialize(m.CreateComponent2());
					return c;
				});
			}
		}

		private IMainComponent2 Component2 { get { return GetSingleInstance<MainComponent2>(); } }

		public IMainComponent1 CreateComponent1()
		{
			return this.GetFactory<IMainComponent1>(cc =>
			{
				cc.Construct<MainComponent1>()
				.Inject((m, c) => c.MainComponent22 = m.CreateComponent2())
				.Inject((m, c) => c.MainComponent23 = m.Component2)
				.Inject((m, c) => c.Initialize(m.CreateComponent2()));
			});
		}

		public IMainComponent2 CreateComponent2()
		{
			return GetFactory<MainComponent2>();
		}

		public IMainComponent1 CreatePrivateComponent1()
		{
			return this.GetFactory(m =>
			{
				var c = new MainComponent1()
				{
					MainComponent23 = m.CreatePrivateComponent2()
				};
				c.Initialize(m.CreateComponent2());
				c.MainComponent22 = m.Component2;
				return c;
			});
		}

		[PrivateFactory]
		public IMainComponent2 CreatePrivateComponent2()
		{
			return GetFactory<MainComponent2>();
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
