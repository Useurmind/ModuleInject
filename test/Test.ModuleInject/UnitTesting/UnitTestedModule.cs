﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modularity.Registry;
using Test.ModuleInject.Modules.TestModules;
using global::ModuleInject;
using global::ModuleInject.Injection;
using global::ModuleInject.Interfaces;

using Moq;

using Xunit;
using ModuleInject.Modularity;

namespace Test.ModuleInject.UnitTesting
{
    public interface IUnitTestedModule2 : IModule
    {
        IMainComponent2 MainComponent2 { get; }

        IMainComponent2 CreateMainComponent2();
    }

    public class UnitTestedModule2 : InjectionModule<UnitTestedModule2>, IUnitTestedModule2
    {
        public IMainComponent2 MainComponent2 { get { return GetSingleInstance<MainComponent2>(); } }

        public IMainComponent2 CreateMainComponent2()
        {
            return this.GetFactoryWithConstruct(x => new MainComponent2());
        }
    }

    public interface IUnitTestedModule : IModule
    {

    }

    public class UnitTestedModule : InjectionModule<UnitTestedModule>, IUnitTestedModule
    {
        public IMainComponent1 MainComponent1 { get { return Get<IMainComponent1>(); } }
        
        public IMainComponent2 ZMainComponent2 { get { return Get<IMainComponent2>(); } }

        [FromRegistry]
        public IUnitTestedModule2 RegistryModule { get; set; }

        public UnitTestedModule()
        {
            SingleInstance(x => x.ZMainComponent2).Construct<MainComponent2>();
        }

        public void PerformPropertyInjectionWithComponent()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = m.RegistryModule.MainComponent2);
        }

        public void PerformMethodInjectionWithComponent()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((mod, x) => x.Initialize(RegistryModule.MainComponent2));
        }

        public void PerformConstructorInjectionWithComponent()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct(m => new MainComponent1(m.RegistryModule.MainComponent2));
        }

        public void PerformPropertyInjectionWithFactory()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = m.RegistryModule.CreateMainComponent2());
        }

        public void PerformMethodInjectionWithFactory()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((mod, x) => x.Initialize(RegistryModule.CreateMainComponent2()));
        }

        public void PerformConstructorInjectionWithFactory()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct(m => new MainComponent1(m.RegistryModule.CreateMainComponent2()));
        }

        public void SimpleConstructWithoutInjection()
        {
            SingleInstance(x => x.MainComponent1).Construct<MainComponent1>()
                .Inject((m, c) => c.MainComponent2 = m.ZMainComponent2);
        }
    }

    
    public class UnitTestedModuleTest
    {
        private UnitTestedModule testedModule;
        private StandardRegistry realRegistry;
        private StandardRegistry mockRegistry;

        public UnitTestedModuleTest()
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

        [Fact]
        public void CreateMock_OfUnitTestedModules()
        {
            Mock.Of<IUnitTestedModule>();
            Mock.Of<IUnitTestedModule2>();
        }

        [Fact]
        public void Resolve_PropertyInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformPropertyInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }


        [Fact]
        public void Resolve_PropertyInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformPropertyInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }

        [Fact]
        public void Resolve_MethodInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformMethodInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }



        [Fact]
        public void Resolve_MethodInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformMethodInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }

        [Fact]
        public void Resolve_ConstructorInjectionWithComponentAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformConstructorInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }

        [Fact]
        public void Resolve_ConstructorInjectionWithComponentAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformConstructorInjectionWithComponent();
            testedModule.Resolve();

            AssertInjectionsCorrect();
        }



        [Fact]
        public void Resolve_PropertyInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformPropertyInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }


        [Fact]
        public void Resolve_PropertyInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformPropertyInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Fact]
        public void Resolve_MethodInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformMethodInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }



        [Fact]
        public void Resolve_MethodInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformMethodInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Fact]
        public void Resolve_ConstructorInjectionWithFactoryAndRealRegistry_Succeeds()
        {
            testedModule.Registry = realRegistry;

            testedModule.PerformConstructorInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        [Fact]
        public void Resolve_ConstructorInjectionWithFactoryAndMockRegistry_Succeeds()
        {
            testedModule.Registry = mockRegistry;

            testedModule.PerformConstructorInjectionWithFactory();
            testedModule.Resolve();

            AssertInjectionsCorrect(false);
        }

        //[Fact]
        //public void Resolve_SimpleConstructAfterSettingInstanceFromExternal_ExternalSetWins()
        //{
        //    var externalComponent = new MainComponent1();

        //    testedModule.Registry = mockRegistry;
        //    testedModule.MainComponent1 = externalComponent;

        //    testedModule.SimpleConstructWithoutInjection();
        //    testedModule.Resolve();

        //    Assert.Same(externalComponent, testedModule.MainComponent1);
        //}

        //[Fact]
        //public void Resolve_SimpleConstructAfterSettingPrerequisiteFromExternal_ExternalSetWins()
        //{
        //    var externalComponent = new MainComponent2();

        //    testedModule.Registry = mockRegistry;
        //    testedModule.ZMainComponent2 = externalComponent;

        //    testedModule.SimpleConstructWithoutInjection();
        //    testedModule.Resolve();

        //    Assert.Same(externalComponent, testedModule.ZMainComponent2);
        //    Assert.Same(externalComponent, testedModule.MainComponent1.MainComponent2);
        //}

        private void AssertInjectionsCorrect(bool sameInstance = true)
        {
            Assert.NotNull(testedModule.RegistryModule);
            Assert.NotNull(testedModule.MainComponent1);
            Assert.NotNull(testedModule.MainComponent1.MainComponent2);

            if (sameInstance)
            {
                Assert.Same(testedModule.RegistryModule.MainComponent2, testedModule.MainComponent1.MainComponent2);
            }
        }
    }
}
