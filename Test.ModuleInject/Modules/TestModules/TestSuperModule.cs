using System.Linq;

using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;
using ModuleInject.Injection;

namespace Test.ModuleInject.Modules.TestModules
{
	public interface ITestSuperModule : IModule
	{
		PropertyModule MainModule { get; }
		ISubModule SubModule { get; }
	}

	public class TestSuperModule : InjectionModule<TestSuperModule>, ITestSuperModule
	{
		public PropertyModule MainModule
		{
			get
			{
				return GetSingleInstance<PropertyModule>(cc =>
				{
					cc.Construct<PropertyModule>()
					.Inject((m, c) => c.SubModule = m.SubModule);
				});
			}
		}
		public ISubModule SubModule
		{
			get
			{
				return GetSingleInstance<ISubModule>(cc =>
				{
					cc.Construct<Submodule>();
				});
			}
		}
	}
}
