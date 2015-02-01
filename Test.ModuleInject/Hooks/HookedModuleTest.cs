using ModuleInject.Common.Linq;
using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Modularity.Registry;
using ModuleInject.Modules;
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
        private List<IRegistrationContext> hookedRegistrations;

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
            this.hookedRegistrations = new List<IRegistrationContext>();
            this.module = new TestModule();
            this.registry = new StandardRegistry();
            this.registry.AddRegistrationHook<IHookedComponent, IHookedModule>(ctx =>
            {
                if (ctx.Context.RegistrationTypes.TModule == this.module.GetType())
                {
                    Assert.AreEqual(0, this.module.ResolvedComponents);
                }
                this.hookedRegistrations.Add(ctx.Context);
            });
            this.module.Registry = this.registry;
        }

        [Test]
        public void Resolve_PrivateHookedComponent_WasHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, TestModule>(this.module.GetProperty(x => x.PrivateHookedComponent));

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Resolve_GetHookedComponentPrivate_WasHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPrivate()));

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Resolve_GetNonHookedComponentPrivate_WasNotHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent2, TestModule>(this.module.GetMethod(x => x.GetNonHookedComponentPrivate()));

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Resolve_GetHookedComponentPublic_WasHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPublic()));

            Assert.AreEqual(1, results.Count());
        }


        [Test]
        public void Resolve_PrivateNonHookedComponent_WasNotHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent2, TestModule>(this.module.GetProperty(x => x.PrivateNonHookedComponent));

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Resolve_PublicHookedComponent_WasHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, TestModule>(this.module.GetProperty(x => x.PublicHookedComponent));

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Resolve_HookedSubModule_WasNotHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<SubTestModule1, TestModule>(this.module.GetProperty(x => x.HookedSubModule));

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Resolve_NonHookedSubModule_WasNotHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<SubTestModule2, TestModule>(this.module.GetProperty(x => x.NonHookedSubModule));

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Resolve_HookedComponentInHookedSubmodule_WasHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, SubTestModule1>(this.module.HookedSubModule.GetProperty(x => x.HookedComponent));

            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void Resolve_HookedComponentInNonHookedSubmodule_WasNotHooked()
        {
            this.module.Resolve();
            var results = GetHookedRegistration<TestComponent1, SubTestModule2>(this.module.NonHookedSubModule.GetProperty(x => x.HookedComponent));

            Assert.AreEqual(0, results.Count());
        }

        private IEnumerable<IRegistrationContext> GetHookedRegistration<TComponet, TModule>(string componentName)
        {
            var results = this.hookedRegistrations.Where(r =>
            {
                return r.RegistrationTypes.TComponent == typeof(TComponet)
                    && r.RegistrationTypes.TModule == typeof(TModule)
                    && r.RegistrationName == componentName;
            });

            return results;
        }
    }
}
