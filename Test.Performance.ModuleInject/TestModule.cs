using Microsoft.Practices.Unity;

using ModuleInject.Injection;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Performance.ModuleInject.Components;

namespace Test.Performance.ModuleInject
{
    public interface ITestModule : IModule
    {
        ITestComponent1 Component1 { get; }
        ITestComponent1 Component2 { get; }
        ITestComponent1 Component3 { get; }
        ITestComponent1 Component4 { get; }
        ITestComponent1 Component5 { get; }
        ITestComponent1 Component6 { get; }
        ITestComponent1 Component7 { get; }
        ITestComponent1 Component8 { get; }
        ITestComponent1 Component9 { get; }
        ITestComponent1 Component10 { get; }
    }

    public class TestInjectionModuleV2 : InjectionModule<TestInjectionModuleV2>, ITestModule
    {
        private ISourceOf<ITestComponent1> component1;

        private ISourceOf<ITestComponent1> component2;

        private ISourceOf<ITestComponent1> component3;

        private ISourceOf<ITestComponent1> component4;

        private ISourceOf<ITestComponent1> component5;

        private ISourceOf<ITestComponent1> component6;

        private ISourceOf<ITestComponent1> component7;

        private ISourceOf<ITestComponent1> component8;
        private ISourceOf<ITestComponent1> component9;

        private ISourceOf<ITestComponent1> component10;

        public ITestComponent1 Component1
        {
            get
            {
                return component1.Get();
            }
        }
        public ITestComponent1 Component2
        {
            get
            {
                return component2.Get();
            }
        }
        public ITestComponent1 Component3
        {
            get
            {
                return component3.Get();
            }
        }
        public ITestComponent1 Component4
        {
            get
            {
                return component4.Get();
            }
        }
        public ITestComponent1 Component5
        {
            get
            {
                return component5.Get();
            }
        }
        public ITestComponent1 Component6
        {
            get
            {
                return component6.Get();
            }
        }
        public ITestComponent1 Component7
        {
            get
            {
                return component7.Get();
            }
        }
        public ITestComponent1 Component8
        {
            get
            {
                return component8.Get();
            }
        }
        public ITestComponent1 Component9
        {
            get
            {
                return component9.Get();
            }
        }
        public ITestComponent1 Component10
        {
            get
            {
                return component10.Get();
            }
        }

        public TestInjectionModuleV2()
        {
            component1 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component2 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component3 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component4 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component5 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component6 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component7 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component8 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component9 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component10 = SingleInstance<ITestComponent1>().Construct<TestComponent1>();
        }
    }

	public class TestInjectionModuleV2NamedSourcesExpressions : InjectionModule<TestInjectionModuleV2NamedSourcesExpressions>, ITestModule
	{
		public ITestComponent1 Component1
		{
			get
			{
				return Get(m => m.Component1);
			}
		}
		public ITestComponent1 Component2
		{
			get
			{
				return Get(m => m.Component2);
			}
		}
		public ITestComponent1 Component3
		{
			get
			{
				return Get(m => m.Component3);
			}
		}
		public ITestComponent1 Component4
		{
			get
			{
				return Get(m => m.Component4);
			}
		}
		public ITestComponent1 Component5
		{
			get
			{
				return Get(m => m.Component5);
			}
		}
		public ITestComponent1 Component6
		{
			get
			{
				return Get(m => m.Component6);
			}
		}
		public ITestComponent1 Component7
		{
			get
			{
				return Get(m => m.Component7);
			}
		}
		public ITestComponent1 Component8
		{
			get
			{
				return Get(m => m.Component8);
			}
		}
		public ITestComponent1 Component9
		{
			get
			{
				return Get(m => m.Component9);
			}
		}
		public ITestComponent1 Component10
		{
			get
			{
				return Get(m => m.Component10);
			}
		}

		public TestInjectionModuleV2NamedSourcesExpressions()
		{
			SingleInstance(m => m.Component1).Construct<TestComponent1>();
			SingleInstance(m => m.Component2).Construct<TestComponent1>();
			SingleInstance(m => m.Component3).Construct<TestComponent1>();
			SingleInstance(m => m.Component4).Construct<TestComponent1>();
			SingleInstance(m => m.Component5).Construct<TestComponent1>();
			SingleInstance(m => m.Component6).Construct<TestComponent1>();
			SingleInstance(m => m.Component7).Construct<TestComponent1>();
			SingleInstance(m => m.Component8).Construct<TestComponent1>();
			SingleInstance(m => m.Component9).Construct<TestComponent1>();
			SingleInstance(m => m.Component10).Construct<TestComponent1>();
		}
	}

	public class TestInjectionModuleV2NamedSourcesWithStrings : InjectionModule<TestInjectionModuleV2NamedSourcesWithStrings>, ITestModule
	{
		public ITestComponent1 Component1
		{
			get
			{
				return Get<ITestComponent1>("Component1");
			}
		}
		public ITestComponent1 Component2
		{
			get
			{
				return Get<ITestComponent1>("Component2");
			}
		}
		public ITestComponent1 Component3
		{
			get
			{
				return Get<ITestComponent1>("Component3");
			}
		}
		public ITestComponent1 Component4
		{
			get
			{
				return Get<ITestComponent1>("Component4");
			}
		}
		public ITestComponent1 Component5
		{
			get
			{
				return Get<ITestComponent1>("Component5");
			}
		}
		public ITestComponent1 Component6
		{
			get
			{
				return Get<ITestComponent1>("Component6");
			}
		}
		public ITestComponent1 Component7
		{
			get
			{
				return Get<ITestComponent1>("Component7");
			}
		}
		public ITestComponent1 Component8
		{
			get
			{
				return Get<ITestComponent1>("Component8");
			}
		}
		public ITestComponent1 Component9
		{
			get
			{
				return Get<ITestComponent1>("Component9");
			}
		}
		public ITestComponent1 Component10
		{
			get
			{
				return Get<ITestComponent1>("Component10");
			}
		}

		public TestInjectionModuleV2NamedSourcesWithStrings()
		{
			SingleInstance2<ITestComponent1>("Component1").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component2").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component3").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component4").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component5").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component6").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component7").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component8").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component9").Construct<TestComponent1>();
			SingleInstance2<ITestComponent1>("Component10").Construct<TestComponent1>();
		}
	}

	public class TestInjectionModuleV2NamedSourcesMixed : InjectionModule<TestInjectionModuleV2NamedSourcesMixed>, ITestModule
	{
		public ITestComponent1 Component1
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component2
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component3
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component4
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component5
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component6
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component7
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component8
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component9
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}
		public ITestComponent1 Component10
		{
			get
			{
				return Get<ITestComponent1>();
			}
		}

		public TestInjectionModuleV2NamedSourcesMixed()
		{
			SingleInstance(m => m.Component1).Construct<TestComponent1>();
			SingleInstance(m => m.Component2).Construct<TestComponent1>();
			SingleInstance(m => m.Component3).Construct<TestComponent1>();
			SingleInstance(m => m.Component4).Construct<TestComponent1>();
			SingleInstance(m => m.Component5).Construct<TestComponent1>();
			SingleInstance(m => m.Component6).Construct<TestComponent1>();
			SingleInstance(m => m.Component7).Construct<TestComponent1>();
			SingleInstance(m => m.Component8).Construct<TestComponent1>();
			SingleInstance(m => m.Component9).Construct<TestComponent1>();
			SingleInstance(m => m.Component10).Construct<TestComponent1>();
		}
	}

	public class TestInjectionModuleV2NamedSourcesAllInGet : InjectionModule<TestInjectionModuleV2NamedSourcesAllInGet>, ITestModule
	{
		public ITestComponent1 Component1
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component2
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component3
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component4
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component5
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component6
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component7
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component8
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component9
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
		public ITestComponent1 Component10
		{
			get
			{
				return GetSingleInstance<ITestComponent1>(cc => cc.Construct<TestComponent1>());
			}
		}
	}

	public class TestManualModule : Module, ITestModule
    {
        public ITestComponent1 Component1 { get; set; }
        public ITestComponent1 Component2 { get; set; }
        public ITestComponent1 Component3 { get; set; }
        public ITestComponent1 Component4 { get; set; }
        public ITestComponent1 Component5 { get; set; }
        public ITestComponent1 Component6 { get; set; }
        public ITestComponent1 Component7 { get; set; }
        public ITestComponent1 Component8 { get; set; }
        public ITestComponent1 Component9 { get; set; }
        public ITestComponent1 Component10 { get; set; }

        public TestManualModule()
        {
        }

        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            Component1 = new TestComponent1();
            Component2 = new TestComponent1();
            Component3 = new TestComponent1();
            Component4 = new TestComponent1();
            Component5 = new TestComponent1();
            Component6 = new TestComponent1();
            Component7 = new TestComponent1();
            Component8 = new TestComponent1();
            Component9 = new TestComponent1();
            Component10 = new TestComponent1();
        }
    }

    public class TestUnityModule : Module, ITestModule
    {
        private IUnityContainer container;

        public ITestComponent1 Component1 { get; set; }
        public ITestComponent1 Component2 { get; set; }
        public ITestComponent1 Component3 { get; set; }
        public ITestComponent1 Component4 { get; set; }
        public ITestComponent1 Component5 { get; set; }
        public ITestComponent1 Component6 { get; set; }
        public ITestComponent1 Component7 { get; set; }
        public ITestComponent1 Component8 { get; set; }
        public ITestComponent1 Component9 { get; set; }
        public ITestComponent1 Component10 { get; set; }

        public TestUnityModule()
        {
            container = new UnityContainer();

            container.RegisterType<ITestComponent1, TestComponent1>("Component1");
            container.RegisterType<ITestComponent1, TestComponent1>("Component2");
            container.RegisterType<ITestComponent1, TestComponent1>("Component3");
            container.RegisterType<ITestComponent1, TestComponent1>("Component4");
            container.RegisterType<ITestComponent1, TestComponent1>("Component5");
            container.RegisterType<ITestComponent1, TestComponent1>("Component6");
            container.RegisterType<ITestComponent1, TestComponent1>("Component7");
            container.RegisterType<ITestComponent1, TestComponent1>("Component8");
            container.RegisterType<ITestComponent1, TestComponent1>("Component9");
            container.RegisterType<ITestComponent1, TestComponent1>("Component10");
        }

        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            Component1 = container.Resolve<ITestComponent1>("Component1");
            Component2 = container.Resolve<ITestComponent1>("Component2");
            Component3 = container.Resolve<ITestComponent1>("Component3");
            Component4 = container.Resolve<ITestComponent1>("Component4");
            Component5 = container.Resolve<ITestComponent1>("Component5");
            Component6 = container.Resolve<ITestComponent1>("Component6");
            Component7 = container.Resolve<ITestComponent1>("Component7");
            Component8 = container.Resolve<ITestComponent1>("Component8");
            Component9 = container.Resolve<ITestComponent1>("Component9");
            Component10 = container.Resolve<ITestComponent1>("Component10");
        }
    }
}
