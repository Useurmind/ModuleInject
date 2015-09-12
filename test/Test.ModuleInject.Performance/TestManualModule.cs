using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Interfaces;
using ModuleInject.Modularity;
using Test.Performance.ModuleInject.Components;

namespace Test.Performance.ModuleInject
{
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
}
