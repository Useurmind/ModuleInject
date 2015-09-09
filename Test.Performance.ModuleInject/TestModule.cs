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
            component1 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component2 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component3 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component4 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component5 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component6 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component7 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component8 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component9 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
            component10 = CreateSingleInstance<ITestComponent1>().Construct<TestComponent1>();
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
			SingleInstance<ITestComponent1>("Component1").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component2").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component3").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component4").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component5").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component6").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component7").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component8").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component9").Construct<TestComponent1>();
			SingleInstance<ITestComponent1>("Component10").Construct<TestComponent1>();
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
}
