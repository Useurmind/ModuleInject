using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Modularity;
using Test.Performance.ModuleInject.Components;

namespace Test.Performance.ModuleInject
{

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
