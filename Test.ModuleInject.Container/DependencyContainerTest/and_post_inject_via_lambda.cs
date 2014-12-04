using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    namespace Test.ModuleInject.Container.DependencyContainerTest
    {
        using global::ModuleInject.Common.Linq;
        using global::ModuleInject.Container.Resolving;

        using NSpec;

        class and_post_inject_via_lambda : and_registering_an_instance
        {
            void a_constant_value()
            {
                new inject_constant_value_via_lambda().Check(this);
            }

            void a_container_internal_resolved_value()
            {
                new inject_a_container_internal_resolved_value_via_lambda().Check(this);
            }
        }
    }

}
