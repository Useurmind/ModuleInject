using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Linq;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    class and_injecting_through_a_method : and_registering_a_type
    {
        void a_constant_value()
        {
            string value = "AFdsg";

            string methodName = Method.Get<TestClass>(x => x.SetStringProperty(""));
            TestClass instance = null;

            beforeEach = () =>
                {
                    this.container.InjectMethod(this.name, this.registeredType, methodName, new IResolvedValue[] { new ConstantValue(value) });
                    instance = (TestClass)this.container.Resolve(this.name, this.registeredType);
                };

            it["the property should be set to the correct value"] = () => instance.StringProperty.should_be(value);
        }

        void a_container_internal_resolved_value()
        {
            string propertyValueName = "adfsfg";
            Type propertyValueType = typeof(TestClass2);

            string methodName = Method.Get<TestClass>(x => x.SetComponent(null));
            TestClass instance = null;
            TestClass2 instance2 = null;

            beforeEach = () =>
            {
                this.container.Register(propertyValueName, propertyValueType, propertyValueType);

                this.container.InjectMethod(this.name, this.registeredType, methodName, new IResolvedValue[] { new ContainerReference(this.container, propertyValueName, propertyValueType) });
                instance = (TestClass)this.container.Resolve(this.name, this.registeredType);
                instance2 = (TestClass2)this.container.Resolve(propertyValueName, propertyValueType);
            };

            it["the property should be set to the instance in the container"] = () => instance.Component.should_be_same(instance2);
        }
    }
}
