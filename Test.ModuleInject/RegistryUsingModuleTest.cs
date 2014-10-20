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
    public class RegistryUsingModuleTest
    {
        private RegistryUsingModule _module;

        [SetUp]
        public void Setup()
        {
            _module = new RegistryUsingModule();
        }

        [Test]
        [ExpectedException(typeof(ModuleInjectException))]
        public void Resolve_NoRegistrySet_ExceptionForMissingSubmodule()
        {
            _module.Resolve();
        }

        [Test]
        public void Resolved_AfterApplyingRegistryWithSubmodule_SubmoduleIsResolved()
        {
            _module.ApplyRegistry();
            _module.Resolve();

            var registryInstance = _module.Registry.GetComponent(typeof(ISubModule));

            Assert.IsNotNull(_module.PropertyModule.SubModule);
            Assert.AreSame(registryInstance, _module.PropertyModule.SubModule);
        }

        [Test]
        public void Dispose__RegistryIsDisposed()
        {
            _module.ApplyRegistry();
            _module.Resolve();

            _module.Dispose();

            Assert.IsTrue(_module.Registry.IsDisposed);
        }
    }
}
