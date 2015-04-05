using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Performance.ModuleInject.Components;

namespace Test.Performance.ModuleInject
{
    public class TestAutofacModule : global::ModuleInject.Modularity.Module, ITestModule
    {
        private IContainer container;

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

        public TestAutofacModule()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component1");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component2");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component3");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component4");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component5");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component6");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component7");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component8");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component9");
            builder.RegisterType<TestComponent1>().Named<ITestComponent1>("Component10");

            container = builder.Build();
        }

        protected override void OnRegistryResolved(global::ModuleInject.Interfaces.IRegistry usedRegistry)
        {
            Component1 = container.ResolveNamed<ITestComponent1>("Component1");
            Component2 = container.ResolveNamed<ITestComponent1>("Component2");
            Component3 = container.ResolveNamed<ITestComponent1>("Component3");
            Component4 = container.ResolveNamed<ITestComponent1>("Component4");
            Component5 = container.ResolveNamed<ITestComponent1>("Component5");
            Component6 = container.ResolveNamed<ITestComponent1>("Component6");
            Component7 = container.ResolveNamed<ITestComponent1>("Component7");
            Component8 = container.ResolveNamed<ITestComponent1>("Component8");
            Component9 = container.ResolveNamed<ITestComponent1>("Component9");
            Component10 = container.ResolveNamed<ITestComponent1>("Component10");
        }
    }
}
