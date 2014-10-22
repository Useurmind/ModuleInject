using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Container.Lifetime;

    using NSpec;

    class TestClass
    {
        public string StringProperty { get; set; }
        public TestClass2 Component { get; set; }

        public TestClass()
        {
            
        }

        public TestClass(string value)
        {
            StringProperty = value;
        }

        public TestClass(TestClass2 component)
        {
            Component = component;
        }

        public void SetStringProperty(string value)
        {
            StringProperty = value;
        }

        public void SetComponent(TestClass2 value)
        {
            Component = value;
        }
    }

    class TestClass2 { }

    class and_registering_a_type : when_using_the_DependencyContainer
    {
        protected Type registeredType;
        protected string name;

        void before_each()
        {
            this.registeredType = typeof(TestClass);
            this.name = "sdflkdsf";

            this.container.Register(this.name, this.registeredType, this.registeredType);
        }

        void the_registration()
        {
            it["is visible in the container"] = () => container.Registrations.Count().should_be(1);
            it["contains the correct registered type"] =
                () => container.Registrations.ElementAt(0).RegisteredType.should_be(this.registeredType);
            it["contains the correct actual type"] =
                () => container.Registrations.ElementAt(0).RegisteredType.should_be(this.registeredType);
            it["contains the correct name"] =
                () => container.Registrations.ElementAt(0).Name.should_be(name);
        }

        void and_resolving_the_type()
        {
            TestClass instance = null;

            beforeEach = () =>
                { instance = (TestClass)container.Resolve(this.name, this.registeredType); };

            it["the instance should not be null"] = () => instance.should_not_be_null();

            context["and resolving again"] = () =>
                {
                    TestClass instance2 = null;

                    beforeEach = () =>
                        { instance2 = (TestClass)container.Resolve(this.name, this.registeredType); };

                    it["the instance should be the same"] = () => instance2.should_be_same(instance);
                };

        }

        void and_calling_register_again()
        {
            beforeEach = () => this.container.Register(this.name, this.registeredType, this.registeredType);

            it["no duplicate registration creted"] = () => container.Registrations.Count().should_be(1);
        }

        void and_applying_factory_lifetime_management_before_resolving_twice()
        {
            TestClass instance1 = null;
            TestClass instance2 = null;

            beforeEach = () =>
                {
                    this.container.SetLifetime(this.name, this.registeredType, new DynamicLifetime());
                    instance1 = (TestClass)this.container.Resolve(this.name, this.registeredType);
                    instance2 = (TestClass)this.container.Resolve(this.name, this.registeredType);
                };

            it["then both instances should not be the same"] = () => instance1.should_not_be_same(instance2);
        }
    }
}
