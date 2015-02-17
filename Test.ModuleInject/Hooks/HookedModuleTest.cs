using ModuleInject.Common.Linq;
using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Modularity.Registry;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;
using ModuleInject.Hooks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Container.Interface;

namespace Test.ModuleInject.Hooks
{
    public class HookedModuleTest
    {
        private TestModule module;
        private StandardRegistry registry;
        private List<IRegistrationContext> registrationsHookedFromRegistry;
        private List<IRegistrationContext> registrationsHookedFromModule;

        private interface IHookedComponent { }
        private interface ITestComponent1 { }
        private interface ITestComponent2 { }

        private class TestComponent1 : ITestComponent1, IHookedComponent { }
        private class TestComponent2 : ITestComponent2 { }
        private interface IHookedModule { }
        private interface ITestModule : IModule
        {
            ITestComponent1 PublicHookedComponent { get; }
            ITestComponent1 GetHookedComponentPublic();
        }
        private interface ISubTestModule1 : IModule
        {
        }

        private class SubTestModule1 : InjectionModule<ISubTestModule1, SubTestModule1>, ISubTestModule1, IHookedModule
        {
            [PrivateComponent]
            public ITestComponent1 HookedComponent { get; set; }

            public SubTestModule1()
            {
                this.RegisterPrivateComponent(x => x.HookedComponent).Construct<TestComponent1>();
            }
        }
        private interface ISubTestModule2 : IModule
        {
        }

        private class SubTestModule2 : InjectionModule<ISubTestModule2, SubTestModule2>, ISubTestModule2
        {
            [PrivateComponent]
            public ITestComponent1 HookedComponent { get; set; }

            public SubTestModule2()
            {
                this.RegisterPrivateComponent(x => x.HookedComponent).Construct<TestComponent1>();
            }
        }

        private class TestModule : InjectionModule<ITestModule, TestModule>, ITestModule, IHookedModule
        {
            [NonModuleProperty]
            public int ResolvedComponents { get; private set; }

            public ITestComponent1 PublicHookedComponent { get; set; }

            [PrivateComponent]
            public ITestComponent1 PrivateHookedComponent { get; set; }

            [PrivateComponent]
            public ITestComponent2 PrivateNonHookedComponent { get; set; }

            [PrivateComponent]
            public SubTestModule1 HookedSubModule { get; set; }

            [PrivateComponent]
            public SubTestModule2 NonHookedSubModule { get; set; }

            [PrivateFactory]
            public ITestComponent1 GetHookedComponentPrivate()
            {
                return this.CreateInstance(x => x.GetHookedComponentPrivate());
            }

            [PrivateFactory]
            public ITestComponent2 GetNonHookedComponentPrivate()
            {
                return this.CreateInstance(x => x.GetNonHookedComponentPrivate());
            }

            public ITestComponent1 GetHookedComponentPublic()
            {
                return this.CreateInstance(x => x.GetHookedComponentPublic());
            }

            public TestModule()
            {
                this.RegisterPrivateComponent(x => x.HookedSubModule).Construct<SubTestModule1>();
                this.RegisterPrivateComponent(x => x.NonHookedSubModule).Construct<SubTestModule2>();
                this.RegisterPublicComponent(x => x.PublicHookedComponent).Construct<TestComponent1>();
                this.RegisterPrivateComponent(x => x.PrivateHookedComponent).Construct<TestComponent1>();
                this.RegisterPrivateComponent(x => x.PrivateNonHookedComponent).Construct<TestComponent2>();
                this.RegisterPrivateComponentFactory(x => x.GetHookedComponentPrivate()).Construct<TestComponent1>();
                this.RegisterPrivateComponentFactory(x => x.GetNonHookedComponentPrivate()).Construct<TestComponent2>();
                this.RegisterPublicComponentFactory(x => x.GetHookedComponentPublic()).Construct<TestComponent1>();
            }

            internal override void OnComponentResolved(ObjectResolvedContext context)
            {
                ResolvedComponents++;
                base.OnComponentResolved(context);
            }
        }

        [SetUp]
        public void Init()
        {
            this.registrationsHookedFromModule = new List<IRegistrationContext>();
            this.registrationsHookedFromRegistry = new List<IRegistrationContext>();
            this.module = new TestModule();
            this.module.AddRegistrationHook<IHookedComponent, IHookedModule>(ctx =>
            {
                Assert.AreEqual(0, this.module.ResolvedComponents);
                this.registrationsHookedFromModule.Add(ctx.Context);
            });

            this.registry = new StandardRegistry();
            this.registry.AddRegistrationHook<IHookedComponent, IHookedModule>(ctx =>
            {
                if (ctx.Context.RegistrationTypes.TModule == this.module.GetType())
                {
                    Assert.AreEqual(0, this.module.ResolvedComponents);
                }
                this.registrationsHookedFromRegistry.Add(ctx.Context);
            });
            this.module.Registry = this.registry;
        }

        [Test]
        public void Resolve_PrivateHookedComponent_WasHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetProperty(x => x.PrivateHookedComponent));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetProperty(x => x.PrivateHookedComponent));

            Assert.AreEqual(1, resultsFromRegistry.Count());
            Assert.AreEqual(1, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_GetHookedComponentPrivate_WasHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPrivate()));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPrivate()));

            Assert.AreEqual(1, resultsFromRegistry.Count());
            Assert.AreEqual(1, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_GetNonHookedComponentPrivate_WasNotHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent2, TestModule>(this.module.GetMethod(x => x.GetNonHookedComponentPrivate()));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent2, TestModule>(this.module.GetMethod(x => x.GetNonHookedComponentPrivate()));

            Assert.AreEqual(0, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_GetHookedComponentPublic_WasHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPublic()));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPublic()));

            Assert.AreEqual(1, resultsFromRegistry.Count());
            Assert.AreEqual(1, resultsFromModule.Count());
        }


        [Test]
        public void Resolve_PrivateNonHookedComponent_WasNotHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent2, TestModule>(this.module.GetProperty(x => x.PrivateNonHookedComponent));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent2, TestModule>(this.module.GetProperty(x => x.PrivateNonHookedComponent));

            Assert.AreEqual(0, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_PublicHookedComponent_WasHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetProperty(x => x.PublicHookedComponent));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetProperty(x => x.PublicHookedComponent));

            Assert.AreEqual(1, resultsFromRegistry.Count());
            Assert.AreEqual(1, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_HookedSubModule_WasNotHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<SubTestModule1, TestModule>(this.module.GetProperty(x => x.HookedSubModule));
            var resultsFromModule = GetRegistrationsFromModule<SubTestModule1, TestModule>(this.module.GetProperty(x => x.HookedSubModule));

            Assert.AreEqual(0, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_NonHookedSubModule_WasNotHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<SubTestModule2, TestModule>(this.module.GetProperty(x => x.NonHookedSubModule));
            var resultsFromModule = GetRegistrationsFromModule<SubTestModule2, TestModule>(this.module.GetProperty(x => x.NonHookedSubModule));

            Assert.AreEqual(0, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_HookedComponentInHookedSubmodule_WasHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, SubTestModule1>(this.module.HookedSubModule.GetProperty(x => x.HookedComponent));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, SubTestModule1>(this.module.HookedSubModule.GetProperty(x => x.HookedComponent));

            Assert.AreEqual(1, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        [Test]
        public void Resolve_HookedComponentInNonHookedSubmodule_WasNotHooked()
        {
            this.module.Resolve();
            var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, SubTestModule2>(this.module.NonHookedSubModule.GetProperty(x => x.HookedComponent));
            var resultsFromModule = GetRegistrationsFromModule<TestComponent1, SubTestModule2>(this.module.NonHookedSubModule.GetProperty(x => x.HookedComponent));

            Assert.AreEqual(0, resultsFromRegistry.Count());
            Assert.AreEqual(0, resultsFromModule.Count());
        }

        private IEnumerable<IRegistrationContext> GetRegistrationsFromRegistry<TComponet, TModule>(string componentName)
        {
            return GetRegistrationsFrom<TComponet, TModule>(this.registrationsHookedFromRegistry, componentName);
        }

        private IEnumerable<IRegistrationContext> GetRegistrationsFromModule<TComponet, TModule>(string componentName)
        {
            return GetRegistrationsFrom<TComponet, TModule>(this.registrationsHookedFromModule, componentName);
        }

        private IEnumerable<IRegistrationContext> GetRegistrationsFrom<TComponet, TModule>(IList<IRegistrationContext> hookedRegistrations, string componentName)
        {
            var results = hookedRegistrations.Where(r =>
            {
                return r.RegistrationTypes.TComponent == typeof(TComponet)
                    && r.RegistrationTypes.TModule == typeof(TModule)
                    && r.RegistrationName == componentName;
            });

            return results;
        }
    }
}
