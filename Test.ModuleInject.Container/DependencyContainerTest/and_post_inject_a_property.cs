using ModuleInject.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using global::ModuleInject.Container;
    using global::ModuleInject.Container.Resolving;

    using NSpec;

    class and_post_injecting_into_a_property : and_registering_an_instance
    {
        void a_constant_value()
        {
            new inject_constant_value_into_property().Check(this);
        }

        void a_container_internal_resolved_value()
        {
            new inject_a_container_internal_resolved_value_into_property().Check(this);
        }

        void a_resolved_value_from_another_container()
        {
            new inject_a_resolved_value_from_another_container_into_property().Check(this);
        }
    }
}

