using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using NSpec;

    class TestClass
    {
        
    }

    class and_registering_a_type : when_using_the_DependencyContainer
    {
        void before_each()
        {
            
        }

        void without_interface()
        {
            beforeEach = () => this.container.Register<TestClass>();

            it["the registration is visible in the container"] = () => container.Registrations.Count().should_be(1);
            it["the registration is for the registered type"] =
                () => container.Registrations.ElementAt(0).ActualType.should_be(typeof(TestClass));
        }
    }
}
