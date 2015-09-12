using ModuleInject.Injection;
using ModuleInject.Interfaces.Injection;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	
	public class BasicInjectionModuleTest
	{
		private interface ITestComponent
		{
			ITestSubComponent SubComponent1 { get; set; }
		}

		private interface ITestSubComponent
		{
			ITestSubSubComponent SubSubComponent1 { get; set; }
		}

		private interface ITestSubSubComponent
		{
		}

		private class TestComponent : ITestComponent
		{
			public ITestSubComponent SubComponent1 { get; set; }

			public ITestSubComponent SubComponent2 { get; set; }
			public ITestSubComponent SubComponent3 { get; set; }

			public TestComponent()
			{

			}

			public TestComponent(ITestSubComponent subComponent)
			{
				SubComponent1 = subComponent;
			}

			public void Inject(ITestSubComponent subComponent)
			{
				SubComponent1 = subComponent;
			}

			public void Inject2(ITestSubComponent subComponent2)
			{
				SubComponent2 = subComponent2;
			}
		}

		private class TestComponentWrapper : ITestComponent
		{
			public ITestSubComponent SubComponent1 { get; set; }
			public ITestComponent WrappedComponent { get; private set; }
			public int Number { get; private set; }

			public TestComponentWrapper(int number, ITestComponent wrappedComponent)
			{
				this.WrappedComponent = wrappedComponent;
				this.Number = number;
			}
		}

		private class TestSubComponent : ITestSubComponent
		{
			public ITestSubSubComponent SubSubComponent1 { get; set; }
		}

		private class TestSubSubComponent : ITestSubSubComponent
		{
		}

		private class TestModule : InjectionModule<TestModule>
		{
			private ISourceOf<ITestComponent> component1;
			private ISourceOf<ITestSubComponent> subComponent1;
			private ISourceOf<ITestSubSubComponent> subSubComponent1;
			private ISourceOf<ITestComponent> component2;
			private ISourceOf<ITestSubComponent> subComponent2;
			private ISourceOf<ITestSubSubComponent> subSubComponent2;


			public ITestComponent Component1 { get { return component1.Get(); } }
			public ITestSubComponent SubComponent1 { get { return subComponent1.Get(); } }
			public ITestSubSubComponent SubSubComponent1 { get { return subSubComponent1.Get(); } }
			public ITestComponent Component2 { get { return component2.Get(); } }
			public ITestSubComponent SubComponent2 { get { return subComponent2.Get(); } }
			public ITestSubSubComponent SubSubComponent2 { get { return subSubComponent2.Get(); } }

			public TestModule()
			{
				subSubComponent1 = CreateFactory<ITestSubSubComponent>()
					.Construct<TestSubSubComponent>();

				subComponent1 = CreateFactory<ITestSubComponent>()
					.Construct(m => new TestSubComponent()
					{
						SubSubComponent1 = m.SubSubComponent1
					});

				subSubComponent2 = CreateSingleInstance<ITestSubSubComponent>()
					.Construct<TestSubSubComponent>();

				subComponent2 = CreateSingleInstance<ITestSubComponent>()
					.Construct(m => new TestSubComponent()
					{
						SubSubComponent1 = m.SubSubComponent2
					});
			}

			public void UseConstructorInjection()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent1));
				component2 = CreateSingleInstance<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent2));
			}

			public void UsePropertyInjection()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.SubComponent1 = m.SubComponent1; });
				component2 = CreateSingleInstance<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.SubComponent1 = m.SubComponent2; });
			}

			public void UseMethodInjection()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.Inject(m.SubComponent1); });
				component2 = CreateSingleInstance<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.Inject(m.SubComponent2); });
			}

			public void ApplySingleWrapperAroundComponent()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent1); })
								.Change((m, c) => new TestComponentWrapper(1, c));
				component2 = CreateSingleInstance<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent2); })
								.Change((m, c) => new TestComponentWrapper(1, c));
			}

			public void ApplyMultipleWrapperAroundComponent()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent1); })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
				component2 = CreateSingleInstance<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent2); })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
			}

			public void CombineAllPossibleStuff()
			{
				component1 = CreateFactory<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent1))
								.Inject((m, c) => { c.Inject2(m.SubComponent1); })
								.Inject((m, c) => { c.SubComponent3 = m.SubComponent1; })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));

				component2 = CreateSingleInstance<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent2))
								.Inject((m, c) => { c.Inject2(m.SubComponent2); })
								.Inject((m, c) => { c.SubComponent3 = m.SubComponent2; })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
			}
		}

		[Fact]
		public void UseConstructorInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UseConstructorInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[Fact]
		public void UsePropertyInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UsePropertyInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[Fact]
		public void UseMethodInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UseMethodInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[Fact]
		public void ApplySingleWrapperAroundComponent_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.ApplySingleWrapperAroundComponent();
			testModule.Resolve();

			var component1Wrapper = testModule.Component1 as TestComponentWrapper;
			var component1 = component1Wrapper.WrappedComponent as TestComponent;

			Assert.NotNull(component1Wrapper);
			Assert.NotNull(component1);
			Assert.Equal(1, component1Wrapper.Number);
			Assert.IsType<TestComponentWrapper>(component1Wrapper);
			Assert.IsType<TestComponent>(component1);
			Assert.NotNull(component1.SubComponent1);
		}

		[Fact]
		public void ApplyMultipleWrapperAroundComponent_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.ApplyMultipleWrapperAroundComponent();
			testModule.Resolve();

			var wrapper2 = testModule.Component1 as TestComponentWrapper;
			var wrapper1 = wrapper2.WrappedComponent as TestComponentWrapper;
			var component1 = wrapper1.WrappedComponent as TestComponent;

			Assert.NotNull(wrapper2);
			Assert.NotNull(wrapper1);
			Assert.NotNull(component1);
			Assert.IsType<TestComponentWrapper>(wrapper2);
			Assert.IsType<TestComponentWrapper>(wrapper1);
			Assert.IsType<TestComponent>(component1);
			Assert.Equal(2, wrapper2.Number);
			Assert.Equal(1, wrapper1.Number);
			Assert.NotNull(component1.SubComponent1);
		}

		[Fact]
		public void CombineAllPossibleStuff_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.CombineAllPossibleStuff();
			testModule.Resolve();

			var wrapper2 = testModule.Component1 as TestComponentWrapper;
			var wrapper1 = wrapper2.WrappedComponent as TestComponentWrapper;
			var component1 = wrapper1.WrappedComponent as TestComponent;

			Assert.NotNull(wrapper2);
			Assert.NotNull(wrapper1);
			Assert.NotNull(component1);
			Assert.Equal(2, wrapper2.Number);
			Assert.Equal(1, wrapper1.Number);
			Assert.NotNull(component1.SubComponent1);
			Assert.NotNull(component1.SubComponent2);
			Assert.NotNull(component1.SubComponent3);
			Assert.NotNull(component1.SubComponent1.SubSubComponent1);
			Assert.NotNull(component1.SubComponent2.SubSubComponent1);
			Assert.NotNull(component1.SubComponent3.SubSubComponent1);
			Assert.NotEqual(component1.SubComponent1, component1.SubComponent2);
			Assert.NotEqual(component1.SubComponent1, component1.SubComponent3);
			Assert.NotEqual(component1.SubComponent2, component1.SubComponent3);
		}

		private static void AssertComponentsCorrectlyInjected(TestModule testModule)
		{
			Assert.NotNull(testModule.Component1);
			Assert.NotNull(testModule.SubSubComponent1);
			Assert.NotNull(testModule.SubComponent1);
			Assert.NotNull(testModule.Component1.SubComponent1);
			Assert.NotNull(testModule.Component1.SubComponent1.SubSubComponent1);

			Assert.NotNull(testModule.Component2);
			Assert.NotNull(testModule.SubSubComponent2);
			Assert.NotNull(testModule.SubComponent2);
			Assert.NotNull(testModule.Component2.SubComponent1);

			Assert.Same(testModule.Component2.SubComponent1, testModule.SubComponent2);
			Assert.Same(testModule.Component2.SubComponent1.SubSubComponent1, testModule.SubSubComponent2);
		}
	}
}
