using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    public class inject_constant_value_into_property : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string value = "afsdfg";
            string propertyName = Property.Get((TestClass x) => x.StringProperty);
            TestClass instance = null;

            spec.BeforeEach =
                () =>
                {
                    spec.Container.InjectProperty(
                        spec.Name,
                        spec.RegisteredType,
                        propertyName,
                        new ConstantValue<string>(value));

                    instance = (TestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                };

            spec.It["the property should be set to the given value"] = () => instance.StringProperty.should_be(value);
        }
    }

    public class inject_a_container_internal_resolved_value_into_property : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string propertyValueName = "asgds";
            Type propertyValueType = typeof(TestClass2);

            string propertyName = Property.Get((TestClass x) => x.Component);
            TestClass instance = null;
            TestClass2 instance2 = null;

            spec.BeforeEach =
                () =>
                {
                    spec.Container.Register(propertyValueName, propertyValueType, propertyValueType);

                    spec.Container.InjectProperty(
                        spec.Name,
                        spec.RegisteredType,
                        propertyName,
                        new ContainerReference(spec.Container, propertyValueName, propertyValueType));
                    instance = (TestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                    instance2 = (TestClass2)spec.Container.Resolve(propertyValueName, propertyValueType);
                };

            spec.It["the property should be set to the instance registered in the container"] = () => instance.Component.should_be_same(instance2);
        }
    }

    public class inject_a_resolved_value_from_another_container_into_property : INSpecTest<IInjectionTestSpec>
    {
        public void Check(IInjectionTestSpec spec)
        {
            string propertyValueName = "asgds";
            Type propertyValueType = typeof(TestClass2);

            string propertyName = Property.Get((TestClass x) => x.Component);
            TestClass instance = null;
            TestClass2 instance2 = null;

            spec.BeforeEach = () =>
                {
                    var container2 = new DependencyContainer();
                    container2.Register(propertyValueName, propertyValueType, propertyValueType);

                    spec.Container.InjectProperty(
                        spec.Name,
                        spec.RegisteredType,
                        propertyName,
                        new ContainerReference(container2, propertyValueName, propertyValueType));
                    instance = (TestClass)spec.Container.Resolve(spec.Name, spec.RegisteredType);
                    instance2 = (TestClass2)container2.Resolve(propertyValueName, propertyValueType);
                };

            spec.It["the property should be set to the instance registered in the second container"] = () => instance.Component.should_be_same(instance2);
        }
    }


}
