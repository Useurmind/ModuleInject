﻿using ModuleInject.Common.Linq;
using ModuleInject.Interfaces;
using ModuleInject.Modularity.Registry;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Injection;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Injection;

namespace Test.ModuleInject.Hooks
{
	public class HookedModuleTest
	{
		private TestModule module;
		private StandardRegistry registry;
		private List<IInjectionRegister> registrationsHookedFromRegistry;
		private List<IInjectionRegister> registrationsHookedFromModule;

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

		private class SubTestModule1 : InjectionModule<SubTestModule1>, ISubTestModule1, IHookedModule
		{
			private ISourceOf<ITestComponent1> hookedComponent;

			public ITestComponent1 HookedComponent { get { return hookedComponent.Get(); } }

			public SubTestModule1()
			{
				hookedComponent = CreateSingleInstance<ITestComponent1>()
					.Construct<TestComponent1>()
					.AddMeta<string>(this.GetProperty(x => x.HookedComponent));
			}
		}
		private interface ISubTestModule2 : IModule
		{
		}

		private class SubTestModule2 : InjectionModule<SubTestModule2>, ISubTestModule2
		{
			private ISourceOf<ITestComponent1> hookedComponent;

			public ITestComponent1 HookedComponent { get { return hookedComponent.Get(); } }

			public SubTestModule2()
			{
				hookedComponent = CreateSingleInstance<ITestComponent1>()
					.Construct<TestComponent1>()
					.AddMeta<string>(this.GetProperty(x => x.HookedComponent));
			}
		}

		private class TestModule : InjectionModule<TestModule>, ITestModule, IHookedModule
		{
			private ISourceOf<ITestComponent1> publicHookedComponent;

			private ISourceOf<ITestComponent1> privateHookedComponent;

			private ISourceOf<ITestComponent2> privateNonHookedComponent;

			private ISourceOf<SubTestModule1> hookedSubModule;

			private ISourceOf<SubTestModule2> nonHookedSubModule;

			private ISourceOf<ITestComponent1> hookedComponentPrivateFactory;

			private ISourceOf<ITestComponent2> nonHookedComponentPrivateFactory;

			private ISourceOf<ITestComponent1> hookedComponentPublicFactory;

			public int ResolvedComponents { get; private set; }

			public ITestComponent1 PublicHookedComponent { get { return publicHookedComponent.Get(); } }

			public ITestComponent1 PrivateHookedComponent { get { return privateHookedComponent.Get(); } }

			public ITestComponent2 PrivateNonHookedComponent { get { return privateNonHookedComponent.Get(); } }

			public SubTestModule1 HookedSubModule { get { return hookedSubModule.Get(); } }

			public SubTestModule2 NonHookedSubModule { get { return nonHookedSubModule.Get(); } }

			public ITestComponent1 GetHookedComponentPrivate()
			{
				return hookedComponentPrivateFactory.Get();
			}

			public ITestComponent2 GetNonHookedComponentPrivate()
			{
				return nonHookedComponentPrivateFactory.Get();
			}

			public ITestComponent1 GetHookedComponentPublic()
			{
				return hookedComponentPublicFactory.Get();
			}

			public TestModule()
			{
				hookedSubModule = CreateSingleInstance<SubTestModule1>().Construct<SubTestModule1>()
					.AddMeta<string>(this.GetProperty(x => x.HookedSubModule));
				nonHookedSubModule = CreateSingleInstance<SubTestModule2>().Construct<SubTestModule2>()
					.AddMeta<string>(this.GetProperty(x => x.NonHookedSubModule));
				publicHookedComponent = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>()
					.AddMeta<string>(this.GetProperty(x => x.PublicHookedComponent));
				privateHookedComponent = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>()
					.AddMeta<string>(this.GetProperty(x => x.PrivateHookedComponent));
				privateNonHookedComponent = CreateSingleInstance<ITestComponent2>().Construct<TestComponent2>()
					.AddMeta<string>(this.GetProperty(x => x.PrivateNonHookedComponent));

				hookedComponentPrivateFactory = CreateFactory<ITestComponent1>().Construct<TestComponent1>()
					.AddMeta<string>(this.GetMethod(x => x.GetHookedComponentPrivate()));
				nonHookedComponentPrivateFactory = CreateFactory<ITestComponent2>().Construct<TestComponent2>()
					.AddMeta<string>(this.GetMethod(x => x.GetNonHookedComponentPrivate()));
				hookedComponentPublicFactory = CreateFactory<ITestComponent1>().Construct<TestComponent1>()
					.AddMeta<string>(this.GetMethod(x => x.GetHookedComponentPublic()));
			}

			protected override void OnResolved()
			{
				base.OnResolved();

				var a = PublicHookedComponent;
				var b = PrivateHookedComponent;
				var c = PrivateNonHookedComponent;

				var d = GetHookedComponentPrivate();
				var e = GetNonHookedComponentPrivate();
				var f = GetHookedComponentPublic();
			}

			protected override void OnComponentResolved(ObjectResolvedContext context)
			{
				ResolvedComponents++;
				base.OnComponentResolved(context);
			}
		}

        public HookedModuleTest()
        {
			this.registrationsHookedFromModule = new List<IInjectionRegister>();
			this.registrationsHookedFromRegistry = new List<IInjectionRegister>();
			this.module = new TestModule();
			this.module.AddRegistrationHook<IHookedModule, IHookedComponent>(ctx =>
			{
				Assert.Equal(0, this.module.ResolvedComponents);
				this.registrationsHookedFromModule.Add(ctx.Register);
			});

			this.registry = new StandardRegistry();
			this.registry.AddRegistrationHook<IHookedModule, IHookedComponent>(ctx =>
			{
				if (ctx.Register.ContextType == this.module.GetType())
				{
					Assert.Equal(0, this.module.ResolvedComponents);
				}
				this.registrationsHookedFromRegistry.Add(ctx.Register);
			});
			this.module.Registry = this.registry;
		}

		[Fact]
		public void Resolve_PrivateHookedComponent_WasHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetProperty(x => x.PrivateHookedComponent));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetProperty(x => x.PrivateHookedComponent));

			Assert.Equal(1, resultsFromRegistry.Count());
			Assert.Equal(1, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_GetHookedComponentPrivate_WasHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPrivate()));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPrivate()));

			Assert.Equal(1, resultsFromRegistry.Count());
			Assert.Equal(1, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_GetNonHookedComponentPrivate_WasNotHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent2, TestModule>(this.module.GetMethod(x => x.GetNonHookedComponentPrivate()));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent2, TestModule>(this.module.GetMethod(x => x.GetNonHookedComponentPrivate()));

			Assert.Equal(0, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_GetHookedComponentPublic_WasHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPublic()));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetMethod(x => x.GetHookedComponentPublic()));

			Assert.Equal(1, resultsFromRegistry.Count());
			Assert.Equal(1, resultsFromModule.Count());
		}


		[Fact]
		public void Resolve_PrivateNonHookedComponent_WasNotHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent2, TestModule>(this.module.GetProperty(x => x.PrivateNonHookedComponent));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent2, TestModule>(this.module.GetProperty(x => x.PrivateNonHookedComponent));

			Assert.Equal(0, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_PublicHookedComponent_WasHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, TestModule>(this.module.GetProperty(x => x.PublicHookedComponent));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, TestModule>(this.module.GetProperty(x => x.PublicHookedComponent));

			Assert.Equal(1, resultsFromRegistry.Count());
			Assert.Equal(1, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_HookedSubModule_WasNotHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<SubTestModule1, TestModule>(this.module.GetProperty(x => x.HookedSubModule));
			var resultsFromModule = GetRegistrationsFromModule<SubTestModule1, TestModule>(this.module.GetProperty(x => x.HookedSubModule));

			Assert.Equal(0, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_NonHookedSubModule_WasNotHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<SubTestModule2, TestModule>(this.module.GetProperty(x => x.NonHookedSubModule));
			var resultsFromModule = GetRegistrationsFromModule<SubTestModule2, TestModule>(this.module.GetProperty(x => x.NonHookedSubModule));

			Assert.Equal(0, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_HookedComponentInHookedSubmodule_WasHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, SubTestModule1>(this.module.HookedSubModule.GetProperty(x => x.HookedComponent));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, SubTestModule1>(this.module.HookedSubModule.GetProperty(x => x.HookedComponent));

			Assert.Equal(1, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		[Fact]
		public void Resolve_HookedComponentInNonHookedSubmodule_WasNotHooked()
		{
			this.module.Resolve();
			var resultsFromRegistry = GetRegistrationsFromRegistry<TestComponent1, SubTestModule2>(this.module.NonHookedSubModule.GetProperty(x => x.HookedComponent));
			var resultsFromModule = GetRegistrationsFromModule<TestComponent1, SubTestModule2>(this.module.NonHookedSubModule.GetProperty(x => x.HookedComponent));

			Assert.Equal(0, resultsFromRegistry.Count());
			Assert.Equal(0, resultsFromModule.Count());
		}

		private IEnumerable<IInjectionRegister> GetRegistrationsFromRegistry<TComponet, TModule>(string componentName)
			where TModule : IModule
		{
			return GetRegistrationsFrom<TComponet, TModule>(this.registrationsHookedFromRegistry, componentName);
		}

		private IEnumerable<IInjectionRegister> GetRegistrationsFromModule<TComponet, TModule>(string componentName)
			where TModule : IModule
		{
			return GetRegistrationsFrom<TComponet, TModule>(this.registrationsHookedFromModule, componentName);
		}

		private IEnumerable<IInjectionRegister> GetRegistrationsFrom<TComponet, TModule>(IList<IInjectionRegister> hookedRegistrations, string componentName)
			where TModule : IModule
		{
			var componentType = typeof(TComponet);
			var moduleType = typeof(TModule);
			var componentName2 = componentName;

			var results = hookedRegistrations.Where(r =>
			{
				return r.ComponentType == componentType
					&& r.ContextType == moduleType
					&& r.MetaData.Contains(componentName2);
			});

			return results;
		}
	}
}
