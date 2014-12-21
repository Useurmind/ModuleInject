using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
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
