using ModuleInject;
using ModuleInject.Decoration;
using ModuleInject.Fluent;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class PrerequisiteTest
    {
        private TestModule testModule;

        private class TestModule : InjectionModule<IEmptyModule, TestModule>, IEmptyModule
        {
            [PrivateComponent]
            public IMainComponent1 Component { get; set; }

            [PrivateComponent]
            public IMainComponent2 ZPrerequisiteComponent { get; set; }

            public TestModule()
            {
                RegisterPrivateComponent(x => x.ZPrerequisiteComponent).Construct<MainComponent2>();
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection()
            {
                RegisterPrivateComponent(x => x.Component).Construct<MainComponent1>()
                                    .Inject((c, m) => c.Initialize(this.ZPrerequisiteComponent));
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection()
            {
                RegisterPrivateComponent(x => x.Component).Construct<MainComponent1>()
                                    .Inject(m => this.ZPrerequisiteComponent).IntoProperty(c => c.MainComponent2);
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection()
            {
                RegisterPrivateComponent(x => x.Component)
                    .Construct(m => new MainComponent1(this.ZPrerequisiteComponent));
            }
        }

        [SetUp]
        public void Init()
        {
            testModule = new TestModule();
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection_PrerequisiteResolvedCorrectly()
        {
            testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection();
            testModule.Resolve();

            Assert.IsNotNull(testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(testModule.Component);
            Assert.AreSame(testModule.ZPrerequisiteComponent, testModule.Component.MainComponent2);
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection_PrerequisiteResolvedCorrectly()
        {
            testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection();
            testModule.Resolve();

            Assert.IsNotNull(testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(testModule.Component);
            Assert.AreSame(testModule.ZPrerequisiteComponent, testModule.Component.MainComponent2);
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection_PrerequisiteResolvedCorrectly()
        {
            testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection();
            testModule.Resolve();

            Assert.IsNotNull(testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(testModule.Component);
            Assert.AreSame(testModule.ZPrerequisiteComponent, testModule.Component.MainComponent2);
        }
    }
}
