using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container.Interface;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    public class inject_constant_value_via_method : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string value = "AFdsg";

            string methodName = Method.Get<ITestClass>(x => x.SetStringProperty(""));
            ITestClass instance = null;

            spec.BeforeEach = () =>
            {
                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ConstantValue<string>(value) });
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
            };

            spec.It["the property should be set to the correct value"] = () => instance.StringProperty.should_be(value);
        }
    }

    public class inject_a_container_internal_resolved_value_via_method : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string propertyValueName = "adfsfg";
            Type propertyValueType = typeof(TestClass2);

            string methodName = Method.Get<ITestClass>(x => x.SetComponent(null));
            ITestClass instance = null;
            TestClass2 instance2 = null;

            spec.BeforeEach = () =>
            {
                spec.Container.Register(propertyValueName, propertyValueType, propertyValueType);

                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ContainerReference(spec.Container, propertyValueName, propertyValueType) });
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                instance2 = (TestClass2)spec.Container.Resolve(propertyValueName, propertyValueType);
            };

            spec.It["the property should be set to the instance in the container"] = () => instance.Component.should_be_same(instance2);
        }
    }

    public class call_an_overloaded_funtion_without_arguments : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string methodName = Method.Get<ITestClass>(x => x.OverloadedFunction());
            ITestClass instance = null;

            spec.BeforeEach = () =>
                {
                    spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[0]);
                    instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                };

            spec.It["no exception is thrown"] = () => { };
        }
    }

    public class call_an_overloaded_funtion_with_single_argument : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string methodName = Method.Get<ITestClass>(x => x.OverloadedFunction());
            ITestClass instance = null;

            spec.BeforeEach = () =>
            {
                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ConstantValue<string>("") });
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
            };

            spec.It["no exception is thrown"] = () => { };
        }
    }

    public class call_an_overloaded_funtion_with_two_arguments : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string methodName = Method.Get<ITestClass>(x => x.OverloadedFunction());
            ITestClass instance = null;

            spec.BeforeEach = () =>
            {
                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ConstantValue<string>(""), new ConstantValue<int>(1)  });
                instance = (ITestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
            };

            spec.It["no exception is thrown"] = () => { };
        }
    }
}
