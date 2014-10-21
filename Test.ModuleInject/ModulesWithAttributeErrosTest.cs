using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using global::ModuleInject.Common.Exceptions;
    using global::ModuleInject.Utility;

    using NUnit.Framework;
    using Test.ModuleInject.TestModules;

    [TestFixture]
    public class ModulesWithAttributeErrosTest
    {
        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonMarkedPrivateComponent_ExceptionThrown()
        {
            var module = new ModuleWithNonMarkedPrivateComponent();

            module.Resolve();
        }

        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonMarkedExternalComponent_ExceptionThrown()
        {
            var module = new ModuleWithNonMarkedExternalComponent();

            module.Resolve();
        }

        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonMarkedRegistryComponent_ExceptionThrown()
        {
            var module = new ModuleWithNonMarkedRegistryComponent();

            module.Resolve();
        }
    }
}
