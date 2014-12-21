using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
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
                this.RegisterPrivateComponent(x => x.ZPrerequisiteComponent).Construct<MainComponent2>();
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection()
            {
                this.RegisterPrivateComponent(x => x.Component).Construct<MainComponent1>()
                                    .Inject((c, m) => c.Initialize(this.ZPrerequisiteComponent));
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection()
            {
                this.RegisterPrivateComponent(x => x.Component).Construct<MainComponent1>()
                                    .Inject(m => this.ZPrerequisiteComponent).IntoProperty(c => c.MainComponent2);
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection()
            {
                this.RegisterPrivateComponent(x => x.Component)
                    .Construct(m => new MainComponent1(this.ZPrerequisiteComponent));
            }
        }

        [SetUp]
        public void Init()
        {
            this.testModule = new TestModule();
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection_PrerequisiteResolvedCorrectly()
        {
            this.testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection();
            this.testModule.Resolve();

            Assert.IsNotNull(this.testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(this.testModule.Component);
            Assert.AreSame(this.testModule.ZPrerequisiteComponent, this.testModule.Component.MainComponent2);
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection_PrerequisiteResolvedCorrectly()
        {
            this.testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection();
            this.testModule.Resolve();

            Assert.IsNotNull(this.testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(this.testModule.Component);
            Assert.AreSame(this.testModule.ZPrerequisiteComponent, this.testModule.Component.MainComponent2);
        }

        [Test]
        public void RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection_PrerequisiteResolvedCorrectly()
        {
            this.testModule.RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection();
            this.testModule.Resolve();

            Assert.IsNotNull(this.testModule.ZPrerequisiteComponent);
            Assert.IsNotNull(this.testModule.Component);
            Assert.AreSame(this.testModule.ZPrerequisiteComponent, this.testModule.Component.MainComponent2);
        }
    }
}
