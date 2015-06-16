using System.Linq;

using ModuleInject.Injection;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class PrerequisiteTest
    {
        private TestModule testModule;

        private class TestModule : InjectionModule<TestModule>, IEmptyModule
        {
            public IMainComponent1 Component { get { return this.Get<IMainComponent1>(); } }
            
            public IMainComponent2 ZPrerequisiteComponent { get { return GetSingleInstance<IMainComponent2>(m => new MainComponent2()); } }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaMethodInjection()
            {
                this.SingleInstance(x => x.Component).Construct<MainComponent1>()
                                    .Inject((m, c) => c.Initialize(this.ZPrerequisiteComponent));
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaPropertyInjection()
            {
                this.SingleInstance(x => x.Component).Construct<MainComponent1>()
                                    .Inject((m, c) => c.MainComponent2 = this.ZPrerequisiteComponent);
            }

            public void RegisterComponentWithPrerequisiteAndThisReferenceViaConstructorInjection()
            {
                this.SingleInstance(x => x.Component)
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
