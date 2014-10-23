using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Container.Lifetime;

    using NSpec;

    class and_registering_a_type : when_using_the_DependencyContainer, IInjectionTestSpec
    {
        public Type RegisteredType { get; set; }
        public string Name { get; set; }

        void before_each()
        {
            this.RegisteredType = typeof(TestClass);
            this.Name = "sdflkdsf";

            this.Container.Register(this.Name, this.RegisteredType, this.RegisteredType);
        }

        void the_registration()
        {
            it["is visible in the container"] = () => this.Container.Registrations.Count().should_be(1);
            it["contains the correct registered type"] =
                () => this.Container.Registrations.ElementAt(0).RegisteredType.should_be(this.RegisteredType);
            it["contains the correct actual type"] =
                () => this.Container.Registrations.ElementAt(0).RegisteredType.should_be(this.RegisteredType);
            it["contains the correct name"] =
                () => this.Container.Registrations.ElementAt(0).Name.should_be(this.Name);
        }

        void and_resolving_the_type()
        {
            TestClass instance = null;

            beforeEach = () =>
                { instance = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType); };

            it["the instance should not be null"] = () => instance.should_not_be_null();

            context["and resolving again"] = () =>
                {
                    TestClass instance2 = null;

                    beforeEach = () =>
                        { instance2 = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType); };

                    it["the instance should be the same"] = () => instance2.should_be_same(instance);
                };

        }

        void and_calling_register_again()
        {
            beforeEach = () => this.Container.Register(this.Name, this.RegisteredType, this.RegisteredType);

            it["no duplicate registration creted"] = () => this.Container.Registrations.Count().should_be(1);
        }

        void and_applying_factory_lifetime_management_before_resolving_twice()
        {
            TestClass instance1 = null;
            TestClass instance2 = null;

            beforeEach = () =>
                {
                    this.Container.SetLifetime(this.Name, this.RegisteredType, new DynamicLifetime());
                    instance1 = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType);
                    instance2 = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType);
                };

            it["then both instances should not be the same"] = () => instance1.should_not_be_same(instance2);
        }
    }
}
