using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.UnitTesting
{
    using global::ModuleInject;
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Fluent;
    using global::ModuleInject.Interfaces;
    using global::ModuleInject.Registry;

    using Moq;

    using NUnit.Framework;

    using Test.ModuleInject.TestModules;

    public interface IUnitTestedModule2 : IInjectionModule
    {
        IMainComponent2 MainComponent2 { get; set; }

        IMainComponent2 CreateMainComponent2();
    }

    public class UnitTestedModule2 : InjectionModule<IUnitTestedModule2, UnitTestedModule2>, IUnitTestedModule2
    {
        public IMainComponent2 MainComponent2 { get; set; }

        public IMainComponent2 CreateMainComponent2()
        {
            return this.CreateInstance(x => x.CreateMainComponent2());
        }

        public UnitTestedModule2()
        {
            RegisterPublicComponent(x => x.MainComponent2).Construct<MainComponent2>();
            RegisterPublicComponentFactory(x => x.CreateMainComponent2()).Construct<MainComponent2>();
        }
    }

    public interface IUnitTestedModule : IInjectionModule
    {

    }

    public class UnitTestedModule : InjectionModule<IUnitTestedModule, UnitTestedModule>, IUnitTestedModule
    {
        [PrivateComponent]
        public IMainComponent1 MainComponent1 { get; set; }

        [RegistryComponent]
        public IUnitTestedModule2 RegistryModule { get; set; }

        public UnitTestedModule()
        {
        }

        public void PerformPropertyInjectionWithComponent()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.RegistryModule.MainComponent2).IntoProperty(x => x.MainComponent2);
        }

        public void PerformMethodInjectionWithComponent()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((x, mod) => x.Initialize(RegistryModule.MainComponent2));
        }

        public void PerformConstructorInjectionWithComponent()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct(m => new MainComponent1(m.RegistryModule.MainComponent2));
        }

        public void PerformPropertyInjectionWithFactory()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.RegistryModule.CreateMainComponent2()).IntoProperty(x => x.MainComponent2);
        }

        public void PerformMethodInjectionWithFactory()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((x, mod) => x.Initialize(RegistryModule.CreateMainComponent2()));
        }

        public void PerformConstructorInjectionWithFactory()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct(m => new MainComponent1(m.RegistryModule.CreateMainComponent2()));
        }
    }

    [TestFixture]
    public class UnitTestedModuleTest
    {
        private UnitTestedModule testedModule;
        private StandardRegistry realRegistry;
        private StandardRegistry mockRegistry;

        [SetUp]
        public void Setup()
        {
            testedModule = new UnitTestedModule();

            realRegistry = new StandardRegistry();
            realRegistry.RegisterModule<IUnitTestedModule2, UnitTestedModule2>();

            var module2Mock = new Mock<IUnitTestedModule2>();
            module2Mock.SetupGet(x => x.MainComponent2).Returns(Mock.Of<IMainComponent2>());
            module2Mock.Setup(x => x.CreateMainComponent2()).Returns(Mock.Of<IMainComponent2>());

            mockRegistry = new StandardRegistry();
            mockRegistry.Register<IUnitTestedModule2>(() => module2Mock.Object);
        }

        [Test]
        public void CreateMock_OfUnitTestedModules()
        {
            Mock.Of<IUnitTestedModule>();
            Mock.Of<IUnitTestedModule2>();
        }

        [Test]
        public void Resolve_PropertyInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformPropertyInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }


        [Test]
        public void Resolve_PropertyInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformPropertyInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }

        [Test]
        public void Resolve_MethodInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformMethodInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }



        [Test]
        public void Resolve_MethodInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformMethodInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }

        [Test]
        public void Resolve_ConstructorInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformConstructorInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }
        
        [Test]
        public void Resolve_ConstructorInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformConstructorInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }



        [Test]
        public void Resolve_PropertyInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformPropertyInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }


        [Test]
        public void Resolve_PropertyInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformPropertyInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Test]
        public void Resolve_MethodInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformMethodInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }



        [Test]
        public void Resolve_MethodInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformMethodInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Test]
        public void Resolve_ConstructorInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformConstructorInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Test]
        public void Resolve_ConstructorInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformConstructorInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        private void AssertInjectionsCorrect(bool sameInstance = true)
        {
            Assert.IsNotNull(testedModule.RegistryModule);
            Assert.IsNotNull(testedModule.MainComponent1);
            Assert.IsNotNull(testedModule.MainComponent1.MainComponent2);

            if (sameInstance)
            {
                Assert.AreSame(testedModule.RegistryModule.MainComponent2, testedModule.MainComponent1.MainComponent2);
            }
        }
    }
}
