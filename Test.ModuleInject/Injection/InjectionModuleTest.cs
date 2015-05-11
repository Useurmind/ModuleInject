using ModuleInject.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	[TestFixture]
	public class InjectionModuleTest
	{
		private interface ITestComponent {
		}

		private class TestComponent : ITestComponent {
		}

		private class TestModule : InjectionModule<TestModule>
		{
			private IFactory<ITestComponent> component1;

			public TestModule()
			{
				component1 = Factory<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { })
								.ToFactory();
			}
		}
	}
}
