using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Container.Interface;
    using global::ModuleInject.Container.Resolving;
    using NSpec;

    class and_registering_an_instance : when_using_the_DependencyContainer, IInjectionTestSpec
    {
        public string Name { get; set; }
        public Type RegisteredType { get; set; }
        public TestClass Instance { get; set; }

        void before_each()
        {
            this.Name = "Agfsgdfg";
            this.RegisteredType = typeof(TestClass);
            this.Instance = new TestClass();

            this.Container.Register(this.Name, this.RegisteredType, this.Instance);
        }

        void the_resolved_instance()
        {
            TestClass instance = null;

            beforeEach = () =>
                { instance = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType); };

            it["is the one that was registered"] = () => instance.should_be_same(this.Instance);
        }

        void and_registering_a_constructor()
        {
            beforeEach = () =>
                { this.Container.InjectConstructor(this.Name, this.RegisteredType, new IResolvedValue[0]); };

            it["an exception is thrown"] = this.expect<ModuleInjectException>();
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
    }
}
