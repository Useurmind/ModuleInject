using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container.Interface;
    using global::ModuleInject.Container.Resolving;
    using global::ModuleInject.Container.Dependencies;
    using NSpec;

    public class inject_constant_value_via_lambda : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string value = "AFdsg";

            ITestClass instance = null;

            spec.BeforeEach = () =>
            {
                spec.Container.Inject(spec.Name, spec.RegisteredType, new LambdaDependencyInjection<TestClass>(spec.Container, (cont, t) => t.SetStringProperty(value)));
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
            };

            spec.It["the property should be set to the correct value"] = () => instance.StringProperty.should_be(value);
        }
    }

    public class inject_a_container_internal_resolved_value_via_lambda : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string propertyValueName = "adfsfg";
            Type propertyValueType = typeof(TestClass2);

            ITestClass instance = null;
            TestClass2 instance2 = null;

            spec.BeforeEach = () =>
            {
                spec.Container.Register(propertyValueName, propertyValueType, propertyValueType);

                spec.Container.Inject(spec.Name, spec.RegisteredType, new LambdaDependencyInjection<TestClass>(spec.Container, (cont, t) => t.SetComponent((TestClass2)cont.Resolve(propertyValueName, propertyValueType))));
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                instance2 = (TestClass2)spec.Container.Resolve(propertyValueName, propertyValueType);
            };

            spec.It["the property should be set to the instance in the container"] = () => instance.Component.should_be_same(instance2);
        }
    }
}
