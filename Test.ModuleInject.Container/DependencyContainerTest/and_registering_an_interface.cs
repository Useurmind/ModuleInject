using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Container.Lifetime;

    using Microsoft.Practices.Unity.InterceptionExtension;

    using NSpec;

    class and_registering_an_interface : when_using_the_DependencyContainer, IInjectionTestSpec
    {
        public Type RegisteredType { get; set; }
        public string Name { get; set; }

        void before_each()
        {
            this.RegisteredType = typeof(ITestClass);
            this.Name = "sdflkdsf";

            this.Container.Register(this.Name, this.RegisteredType, typeof(TestClass));
        }

        void the_registration()
        {
            it["is visible in the container"] = () => this.Container.Registrations.Count().should_be(1);
            it["contains the correct registered type"] =
                () => this.Container.Registrations.ElementAt(0).RegisteredType.should_be(this.RegisteredType);
            it["contains the correct actual type"] =
                () => this.Container.Registrations.ElementAt(0).ActualType.should_be(typeof(TestClass));
            it["contains the correct name"] =
                () => this.Container.Registrations.ElementAt(0).Name.should_be(this.Name);
        }

        void and_resolving_the_type()
        {
            ITestClass instance = null;

            beforeEach = () =>
            { instance = (ITestClass)this.Container.Resolve(this.Name, this.RegisteredType); };

            it["the instance should not be null"] = () => instance.should_not_be_null();

            context["and resolving again"] = () =>
                {
                    ITestClass instance2 = null;

                    beforeEach = () =>
                    { instance2 = (ITestClass)this.Container.Resolve(this.Name, this.RegisteredType); };

                    it["the instance should be the same"] = () => instance2.should_be_same(instance);
                };

        }

        void and_calling_register_again()
        {
            beforeEach = () => this.Container.Register(this.Name, this.RegisteredType, typeof(TestClass));

            it["no duplicate registration creted"] = () => this.Container.Registrations.Count().should_be(1);
        }

        void and_applying_factory_lifetime_management_before_resolving_twice()
        {
            ITestClass instance1 = null;
            ITestClass instance2 = null;

            beforeEach = () =>
                {
                    this.Container.SetLifetime(this.Name, this.RegisteredType, new DynamicLifetime());
                    instance1 = (ITestClass)this.Container.Resolve(this.Name, this.RegisteredType);
                    instance2 = (ITestClass)this.Container.Resolve(this.Name, this.RegisteredType);
                };

            it["then both instances should not be the same"] = () => instance1.should_not_be_same(instance2);
        }

        void and_call_an_overloaded_funtion_without_arguments()
        {
            new call_an_overloaded_funtion_without_arguments().Check(this);
        }

        void and_call_an_overloaded_funtion_with_single_argument()
        {
            new call_an_overloaded_funtion_with_single_argument().Check(this);
        }

        void and_call_an_overloaded_funtion_with_two_arguments()
        {
            new call_an_overloaded_funtion_with_two_arguments().Check(this);
        }

        private class TestBehaviour : IInterceptionBehavior
        {
            public bool WasInvoked { get; set; }

            public IEnumerable<Type> GetRequiredInterfaces()
            {
                return new Type[] { typeof(ITestClass) };
            }

            public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
            {
                WasInvoked = true;

                return getNext().Invoke(input, getNext);
            }

            public bool WillExecute
            {
                get
                {
                    return true;
                }
            }
        }


        void and_adding_a_behaviour()
        {
            ITestClass instance = null;
                    var testBehaviour = new TestBehaviour();

            beforeEach = () =>
                {
                    this.Container.AddBehaviour(this.Name, this.RegisteredType, testBehaviour);
                    instance = (ITestClass)this.Container.Resolve(Name, RegisteredType);
                };

            It["the resolved instance will be a unity interceptor"] =
                () => (instance as IInterceptingProxy).should_not_be_null();

            Context["and invoking a function on the instance"] = () =>
                {
                    var component = new TestClass2();

                    beforeEach = () => { instance.SetComponent(component); };

                    It["the behaviour should be invoked"] = () => testBehaviour.WasInvoked.should_be_true();
                    It["the method should have been called on the instance"] =
                        () => instance.Component.should_be_same(component);
                };

        }
    }
}
