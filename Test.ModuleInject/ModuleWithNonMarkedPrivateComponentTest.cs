using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using global::ModuleInject.Utility;

    using NUnit.Framework;
    using Test.ModuleInject.TestModules;

    [TestFixture]
    public class ModuleWithNonMarkedPrivateComponentTest
    {
        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NonMarkedPrivateComponent_ExceptionThrown()
        {
            var module = new ModuleWithNonMarkedPrivateComponent();

            module.Resolve();
        }
    }
}
