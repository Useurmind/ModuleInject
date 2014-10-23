﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    public class inject_constant_value_via_method : NSpecTest<IInjectionTestSpec>
    {
        public override void Check(IInjectionTestSpec spec)
        {
            string value = "AFdsg";

            string methodName = Method.Get<TestClass>(x => x.SetStringProperty(""));
            TestClass instance = null;

            spec.BeforeEach = () =>
            {
                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ConstantValue(value) });
                instance = (TestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
            };

            spec.It["the property should be set to the correct value"] = () => instance.StringProperty.should_be(value);
        }
    }

    public class inject_a_container_internal_resolved_value_via_method : NSpecTest<IInjectionTestSpec>
    {
        public override void Check(IInjectionTestSpec spec)
        {
            string propertyValueName = "adfsfg";
            Type propertyValueType = typeof(TestClass2);

            string methodName = Method.Get<TestClass>(x => x.SetComponent(null));
            TestClass instance = null;
            TestClass2 instance2 = null;

            spec.BeforeEach = () =>
            {
                spec.Container.Register(propertyValueName, propertyValueType, propertyValueType);

                spec.Container.InjectMethod(spec.Name, spec.RegisteredType, methodName, new IResolvedValue[] { new ContainerReference(spec.Container, propertyValueName, propertyValueType) });
                instance = (TestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                instance2 = (TestClass2)spec.Container.Resolve(propertyValueName, propertyValueType);
            };

            spec.It["the property should be set to the instance in the container"] = () => instance.Component.should_be_same(instance2);        
        }
    }
}
