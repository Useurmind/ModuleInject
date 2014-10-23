using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using NSpec;
    using NSpec.Domain;

    class and_registering_a_factory : when_using_the_DependencyContainer, IInjectionTestSpec
    {
        public string Name { get; set; }
        public Type RegisteredType { get; set; }
        public TestClass Instance { get; set; }

        void before_each()
        {
            this.Name = "Agfsgdfg";
            this.RegisteredType = typeof(TestClass);
            this.Instance = new TestClass();

            this.Container.Register(this.Name, this.RegisteredType,
                container =>
                    { return this.Instance; });
        }

       void resolving_the_instance()
       {
           TestClass instance = null;
           beforeEach = () =>
               { instance = (TestClass)Container.Resolve(this.Name, this.RegisteredType); };

           it["returns the instance created by the factory"] = () => instance.should_be_same(Instance);
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
