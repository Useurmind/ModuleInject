using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using NSpec;

    class when_using_the_DependencyContainer : nspec
    {
        protected DependencyContainer container;

        void before_each()
        {
            container = new DependencyContainer();
        }
    }
}
