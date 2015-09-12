using ModuleInject.Injection;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	
	public class SingleInstanceInstantiationStrategyTest
	{
		private interface ITestComponent
		{
		}

		private class TestComponent : ITestComponent
		{
		}

		[Fact]
		public void Instance_ReturnsAlwaysSameInstance()
		{
			var singelton = new SingleInstanceInstantiationStrategy<ITestComponent>();

			var instance1 = singelton.GetInstance(() => new TestComponent());
			var instance2 = singelton.GetInstance(() => new TestComponent());

			Assert.NotNull(instance1);
			Assert.Same(instance1, instance2);
		}
	}
}
