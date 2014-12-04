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
    }

    public class UnitTestedModule2 : InjectionModule<IUnitTestedModule2, UnitTestedModule2>, IUnitTestedModule2
    {
        public IMainComponent2 MainComponent2 { get; set; }

        public UnitTestedModule2()
        {
            RegisterPublicComponent(x => x.MainComponent2).Construct<MainComponent2>();
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
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject(x => x.RegistryModule.MainComponent2).IntoProperty(x => x.MainComponent2);
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

            mockRegistry = new StandardRegistry();
            mockRegistry.Register<IUnitTestedModule2>(() => Mock.Of<IUnitTestedModule2>());
        }

        [Test]
        public void CreateMock_OfUnitTestedModules()
        {
            Mock.Of<IUnitTestedModule>();
            Mock.Of<IUnitTestedModule2>();
        }

        [Test]
        public void Resolve_UnitTestedModuleWithRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.Resolve();

            Assert.IsNotNull(testedModule.RegistryModule);
            Assert.IsNotNull(testedModule.MainComponent1);
            Assert.IsNotNull(testedModule.MainComponent1.MainComponent2);
            Assert.AreSame(testedModule.RegistryModule.MainComponent2, testedModule.MainComponent1.MainComponent2);
        }



        [Test]
        public void Resolve_UnitTestedModuleWithMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.Resolve();

            Assert.IsNotNull(testedModule.RegistryModule);
            Assert.IsNotNull(testedModule.MainComponent1);
            Assert.IsNotNull(testedModule.MainComponent1.MainComponent2);
            Assert.AreSame(testedModule.RegistryModule.MainComponent2, testedModule.MainComponent1.MainComponent2);
        }
    }
}
