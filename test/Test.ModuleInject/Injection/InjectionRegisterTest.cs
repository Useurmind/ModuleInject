using ModuleInject.Injection;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	
	public class InjectionRegisterTest
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

		[Fact]
		public void Create_Next_ReturnsNewInstanceEachTime()
		{
			object stubContext = null;
			var injectionRegister = new InjectionRegister<object, ITestComponent, TestComponent>();
			injectionRegister.SetContext(stubContext);
			injectionRegister.Construct(ctx => new TestComponent());

			var instance1 = injectionRegister.GetInstance();
			var instance2 = injectionRegister.GetInstance();

			Assert.NotNull(instance1);
			Assert.NotNull(instance2);
			Assert.NotSame(instance1, instance2);
		}

		[Fact]
		public void Inject_Next_ReturnsCorrectlyInjectedInstance()
		{
			string stringValue = "Adfadhsdfghfd";
			object stubContext = null;
			var injectionRegister = new InjectionRegister<object, ITestComponent, TestComponent>();
			injectionRegister.SetContext(stubContext);
			injectionRegister.Construct(ctx => new TestComponent()
			{
				IntProperty = 5
			});
			injectionRegister.Inject((ctx, comp) =>
			{
				comp.StringProperty = stringValue;
			});

			var instance = injectionRegister.GetInstance();

			Assert.NotNull(instance);
			Assert.Equal(5, instance.IntProperty);
			Assert.Equal(stringValue, instance.StringProperty);
		}

		[Fact]
		public void Change_Next_ReturnsInstanceReturnedByChange()
		{
			object stubContext = null;
			var createdComponent = new TestComponent();
			var changedComponent = new TestComponent();
			var injectionRegister = new InjectionRegister<object, ITestComponent, TestComponent>();
			injectionRegister.SetContext(stubContext);
			injectionRegister.Construct(ctx => createdComponent);
			injectionRegister.Change((ctx, comp) => changedComponent);

			var instance = injectionRegister.GetInstance();

			Assert.NotNull(instance);
			Assert.Same(changedComponent, instance);
		}

		[Fact]
		public void Next_ContextsInCreateInjectChangeAreInitialContext()
		{
			object stubContext = new object();
			object createContext = null;
			object injectContext = null;
			object changeContext = null;

			var injectionRegister = new InjectionRegister<object, ITestComponent, TestComponent>();
			injectionRegister.SetContext(stubContext);
			injectionRegister.Construct(ctx =>
			{
				createContext = ctx;
				return new TestComponent();
			});
			injectionRegister.Inject((ctx, comp) =>
			{
				injectContext = ctx;
			});
			injectionRegister.Change((ctx, comp) =>
			{
				changeContext = ctx;
				return comp;
			});

			var instance = injectionRegister.GetInstance();

			Assert.NotNull(instance);
			Assert.Same(stubContext, createContext);
			Assert.Same(stubContext, injectContext);
			Assert.Same(stubContext, changeContext);
		}
	}
}
