﻿using ModuleInject.Container.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Container.DependencyContainerTest
{
    using NSpec;

    class and_injecting_through_a_constructor : and_registering_a_type
    {
        void a_constant_value()
        {
            string value = "AFDSG";

            TestClass instance = null;

            beforeEach = () =>
                {
                    this.Container.InjectConstructor(
                        this.Name,
                        this.RegisteredType,
                        new IResolvedValue[] { new ConstantValue(value) });

                    instance = (TestClass)this.Container.Resolve(this.Name, this.RegisteredType);
                };

            it["the property should be set to the correct value"] = () => instance.StringProperty.should_be(value);
        }
    }
}
