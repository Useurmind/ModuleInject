using ModuleInject.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	[TestFixture]
	public class SingleInstanceInstantiationStrategyTest
	{
		private interface ITestComponent
		{
		}

		private class TestComponent : ITestComponent
		{
		}

		[TestCase]
		public void Instance_ReturnsAlwaysSameInstance()
		{
			var singelton = new SingleInstanceInstantiationStrategy<ITestComponent>();

			var instance1 = singelton.GetInstance(() => new TestComponent());
			var instance2 = singelton.GetInstance(() => new TestComponent());

			Assert.IsNotNull(instance1);
			Assert.AreSame(instance1, instance2);
		}
	}
}
