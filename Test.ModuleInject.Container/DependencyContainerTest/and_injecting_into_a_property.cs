using ModuleInject.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Container;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    class and_injecting_into_a_property : and_registering_a_type
    {
        void a_constant_value()
        {
            string value = "afsdfg";
            string propertyName = Property.Get((TestClass x) => x.StringProperty);
            TestClass instance = null;

            beforeEach =
                () =>
                    {
                        this.container.InjectProperty(
                            this.name,
                            this.registeredType,
                            propertyName,
                            new ConstantValue(value));
                        instance = (TestClass)this.container.Resolve(this.name, this.registeredType);
                    };

            it["the property should be set to the given value"] = () => instance.StringProperty.should_be(value);
        }

        void a_container_internal_resolved_value()
        {
            string propertyValueName = "asgds";
            Type propertyValueType = typeof(TestClass2);

            string propertyName = Property.Get((TestClass x) => x.Component);
            TestClass instance = null;
            TestClass2 instance2 = null;

            beforeEach =
                () =>
                {
                    this.container.Register(propertyValueName, propertyValueType, propertyValueType);

                    this.container.InjectProperty(
                        this.name,
                        this.registeredType,
                        propertyName,
                        new ContainerReference(this.container, propertyValueName, propertyValueType));
                    instance = (TestClass)this.container.Resolve(this.name, this.registeredType);
                    instance2 = (TestClass2)this.container.Resolve(propertyValueName, propertyValueType);
                };

            it["the property should be set to the instance registered in the container"] = () => instance.Component.should_be_same(instance2);
        }

        void a_resolved_value_from_another_container()
        {

            string propertyValueName = "asgds";
            Type propertyValueType = typeof(TestClass2);

            string propertyName = Property.Get((TestClass x) => x.Component);
            TestClass instance = null;
            TestClass2 instance2 = null;

            beforeEach = () =>
                {
                    var container2 = new DependencyContainer();
                    container2.Register(propertyValueName, propertyValueType, propertyValueType);

                    this.container.InjectProperty(
                        this.name,
                        this.registeredType,
                        propertyName,
                        new ContainerReference(container2, propertyValueName, propertyValueType));
                    instance = (TestClass)this.container.Resolve(this.name, this.registeredType);
                    instance2 = (TestClass2)container2.Resolve(propertyValueName, propertyValueType);
                };

            it["the property should be set to the instance registered in the second container"] = () => instance.Component.should_be_same(instance2);
        }
    }
}
