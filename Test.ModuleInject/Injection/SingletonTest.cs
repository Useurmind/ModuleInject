using ModuleInject.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	[TestFixture]
	public class SingletonTest
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
			var singelton = new Singleton<object, ITestComponent, TestComponent>(null);
			singelton.Construct(ctx => new TestComponent());

			var instance1 = singelton.Instance;
			var instance2 = singelton.Instance;

			Assert.IsNotNull(instance1);
			Assert.AreSame(instance1, instance2);
		}
	}
}
