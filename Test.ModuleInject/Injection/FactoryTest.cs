using ModuleInject.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	[TestFixture]
	public class FactoryTest
	{
		private interface ITestComponent
		{
			int IntProperty { get; set; }
			string StringProperty { get; set; }
		}

		private class TestComponent : ITestComponent
		{
			public int IntProperty { get; set; }
			public string StringProperty { get; set; }
		}

		[Test]
		public void Create_Next_ReturnsNewInstanceEachTime()
		{
			object stubContext = null;
			var factory = new Factory<object, ITestComponent, TestComponent>(stubContext)
				.Create(ctx => new TestComponent());

			var instance1 = factory.Next();
			var instance2 = factory.Next();

			Assert.IsNotNull(instance1);
			Assert.IsNotNull(instance2);
			Assert.AreNotSame(instance1, instance2);
		}

		[Test]
		public void Inject_Next_ReturnsCorrectlyInjectedInstance()
		{
			string stringValue = "Adfadhsdfghfd";
			object stubContext = null;
			var factory = new Factory<object, ITestComponent, TestComponent>(stubContext)
				.Create(ctx => new TestComponent()
				{
					IntProperty = 5
				})
				.Inject((ctx, comp) =>
				{
					comp.StringProperty = stringValue;
				});

			var instance = factory.Next();

			Assert.IsNotNull(instance);
			Assert.AreEqual(5, instance.IntProperty);
			Assert.AreEqual(stringValue, instance.StringProperty);
		}

		[Test]
		public void Change_Next_ReturnsInstanceReturnedByChange()
		{
			object stubContext = null;
			var createdComponent = new TestComponent();
			var changedComponent = new TestComponent();
			var factory = new Factory<object, ITestComponent, TestComponent>(stubContext)
				.Create(ctx => createdComponent)
				.Change((ctx, comp) => changedComponent);

			var instance = factory.Next();

			Assert.IsNotNull(instance);
			Assert.AreSame(changedComponent, instance);
		}

		[Test]
		public void Next_ContextsInCreateInjectChangeAreInitialContext()
		{
			object stubContext = new object();
			object createContext = null;
			object injectContext = null;
			object changeContext = null;

			var factory = new Factory<object, ITestComponent, TestComponent>(stubContext)
				.Create(ctx =>
				{
					createContext = ctx;
					return new TestComponent();
				})
				.Inject((ctx, comp) =>
				{
					injectContext = ctx;
				})
				.Change((ctx, comp) =>
				{
					changeContext = ctx;
					return comp;
				});

			var instance = factory.Next();

			Assert.IsNotNull(instance);
			Assert.AreSame(stubContext, createContext);
			Assert.AreSame(stubContext, injectContext);
			Assert.AreSame(stubContext, changeContext);
		}
	}
}
