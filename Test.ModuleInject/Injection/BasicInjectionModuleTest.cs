using ModuleInject.Injection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Injection
{
	[TestFixture]
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


			public ITestComponent Component1 { get { return component1.GetInstance(); } }
			public ITestSubComponent SubComponent1 { get { return subComponent1.GetInstance(); } }
			public ITestSubSubComponent SubSubComponent1 { get { return subSubComponent1.GetInstance(); } }
			public ITestComponent Component2 { get { return component2.GetInstance(); } }
			public ITestSubComponent SubComponent2 { get { return subComponent2.GetInstance(); } }
			public ITestSubSubComponent SubSubComponent2 { get { return subSubComponent2.GetInstance(); } }

			public TestModule()
			{
				subSubComponent1 = Factory<ITestSubSubComponent>()
					.Construct<TestSubSubComponent>();

				subComponent1 = Factory<ITestSubComponent>()
					.Construct(m => new TestSubComponent()
					{
						SubSubComponent1 = m.SubSubComponent1
					});

				subSubComponent2 = SingleInstance<ITestSubSubComponent>()
					.Construct<TestSubSubComponent>();

				subComponent2 = SingleInstance<ITestSubComponent>()
					.Construct(m => new TestSubComponent()
					{
						SubSubComponent1 = m.SubSubComponent2
					});
			}

			public void UseConstructorInjection()
			{
				component1 = Factory<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent1));
				component2 = SingleInstance<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent2));
			}

			public void UsePropertyInjection()
			{
				component1 = Factory<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.SubComponent1 = m.SubComponent1; });
				component2 = SingleInstance<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.SubComponent1 = m.SubComponent2; });
			}

			public void UseMethodInjection()
			{
				component1 = Factory<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.Inject(m.SubComponent1); });
				component2 = SingleInstance<ITestComponent>()
								.Construct(m => new TestComponent())
								.Inject((m, c) => { c.Inject(m.SubComponent2); });
			}

			public void ApplySingleWrapperAroundComponent()
			{
				component1 = Factory<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent1); })
								.Change((m, c) => new TestComponentWrapper(1, c));
				component2 = SingleInstance<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent2); })
								.Change((m, c) => new TestComponentWrapper(1, c));
			}

			public void ApplyMultipleWrapperAroundComponent()
			{
				component1 = Factory<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent1); })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
				component2 = SingleInstance<ITestComponent>()
								.Construct<TestComponent>()
								.Inject((m, c) => { c.Inject(m.SubComponent2); })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
			}

			public void CombineAllPossibleStuff()
			{
				component1 = Factory<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent1))
								.Inject((m, c) => { c.Inject2(m.SubComponent1); })
								.Inject((m, c) => { c.SubComponent3 = m.SubComponent1; })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));

				component2 = SingleInstance<ITestComponent>()
								.Construct(m => new TestComponent(m.SubComponent2))
								.Inject((m, c) => { c.Inject2(m.SubComponent2); })
								.Inject((m, c) => { c.SubComponent3 = m.SubComponent2; })
								.Change((m, c) => new TestComponentWrapper(1, c))
								.Change((m, c) => new TestComponentWrapper(2, c));
			}
		}

		[TestCase]
		public void UseConstructorInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UseConstructorInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[TestCase]
		public void UsePropertyInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UsePropertyInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[TestCase]
		public void UseMethodInjection_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.UseMethodInjection();
			testModule.Resolve();

			AssertComponentsCorrectlyInjected(testModule);
		}

		[TestCase]
		public void ApplySingleWrapperAroundComponent_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.ApplySingleWrapperAroundComponent();
			testModule.Resolve();

			var component1Wrapper = testModule.Component1 as TestComponentWrapper;
			var component1 = component1Wrapper.WrappedComponent as TestComponent;

			Assert.IsNotNull(component1Wrapper);
			Assert.IsNotNull(component1);
			Assert.AreEqual(1, component1Wrapper.Number);
			Assert.IsInstanceOf<TestComponentWrapper>(component1Wrapper);
			Assert.IsInstanceOf<TestComponent>(component1);
			Assert.IsNotNull(component1.SubComponent1);
		}

		[TestCase]
		public void ApplyMultipleWrapperAroundComponent_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.ApplyMultipleWrapperAroundComponent();
			testModule.Resolve();

			var wrapper2 = testModule.Component1 as TestComponentWrapper;
			var wrapper1 = wrapper2.WrappedComponent as TestComponentWrapper;
			var component1 = wrapper1.WrappedComponent as TestComponent;

			Assert.IsNotNull(wrapper2);
			Assert.IsNotNull(wrapper1);
			Assert.IsNotNull(component1);
			Assert.IsInstanceOf<TestComponentWrapper>(wrapper2);
			Assert.IsInstanceOf<TestComponentWrapper>(wrapper1);
			Assert.IsInstanceOf<TestComponent>(component1);
			Assert.AreEqual(2, wrapper2.Number);
			Assert.AreEqual(1, wrapper1.Number);
			Assert.IsNotNull(component1.SubComponent1);
		}

		[TestCase]
		public void CombineAllPossibleStuff_WorksCorrectly()
		{
			var testModule = new TestModule();

			testModule.CombineAllPossibleStuff();
			testModule.Resolve();

			var wrapper2 = testModule.Component1 as TestComponentWrapper;
			var wrapper1 = wrapper2.WrappedComponent as TestComponentWrapper;
			var component1 = wrapper1.WrappedComponent as TestComponent;

			Assert.IsNotNull(wrapper2);
			Assert.IsNotNull(wrapper1);
			Assert.IsNotNull(component1);
			Assert.AreEqual(2, wrapper2.Number);
			Assert.AreEqual(1, wrapper1.Number);
			Assert.IsNotNull(component1.SubComponent1);
			Assert.IsNotNull(component1.SubComponent2);
			Assert.IsNotNull(component1.SubComponent3);
			Assert.IsNotNull(component1.SubComponent1.SubSubComponent1);
			Assert.IsNotNull(component1.SubComponent2.SubSubComponent1);
			Assert.IsNotNull(component1.SubComponent3.SubSubComponent1);
			Assert.AreNotEqual(component1.SubComponent1, component1.SubComponent2);
			Assert.AreNotEqual(component1.SubComponent1, component1.SubComponent3);
			Assert.AreNotEqual(component1.SubComponent2, component1.SubComponent3);
		}

		private static void AssertComponentsCorrectlyInjected(TestModule testModule)
		{
			Assert.IsNotNull(testModule.Component1);
			Assert.IsNotNull(testModule.SubSubComponent1);
			Assert.IsNotNull(testModule.SubComponent1);
			Assert.IsNotNull(testModule.Component1.SubComponent1);
			Assert.IsNotNull(testModule.Component1.SubComponent1.SubSubComponent1);

			Assert.IsNotNull(testModule.Component2);
			Assert.IsNotNull(testModule.SubSubComponent2);
			Assert.IsNotNull(testModule.SubComponent2);
			Assert.IsNotNull(testModule.Component2.SubComponent1);

			Assert.AreSame(testModule.Component2.SubComponent1, testModule.SubComponent2);
			Assert.AreSame(testModule.Component2.SubComponent1.SubSubComponent1, testModule.SubSubComponent2);
		}
	}
}
