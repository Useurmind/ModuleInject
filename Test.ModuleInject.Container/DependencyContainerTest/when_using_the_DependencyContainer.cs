using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Container;

    using NSpec;

    class when_using_the_DependencyContainer : nspec
    {
        protected DependencyContainer container;

        void before_each()
        {
            container = new DependencyContainer();
        }

        void and_resolving_unregistered_type()
        {
            beforeEach = () =>
                { container.Resolve(null, typeof(object)); };

            it["an exception is thrown"] = this.expect<ModuleInjectException>();
        }
    }
}
