using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using global::ModuleInject.Registry;
    using NUnit.Framework;

    [TestFixture]
    public class RegistryModuleTest
    {
        private global::ModuleInject.Registry.Registry _registry;

        [SetUp]
        public void Setup()
        {
            _registry = new global::ModuleInject.Registry.Registry();
        }

        [TestCase]
        public void IsRegistered_ForRegisteredType_ReturnsTrue()
        {
            _registry.Register(() => 1);

            Assert.IsTrue(_registry.IsRegistered(typeof(int)));
        }

        [TestCase]
        public void GetComponent_ForRegisteredType_ReturnsInstance()
        {
            _registry.Register(() => 1);

            Assert.AreEqual(1, _registry.GetComponent(typeof(int)));
        }

        [TestCase]
        public void GetComponent_ForRegisteredType_ReturnsAlwaysSameInstance()
        {
            _registry.Register(() => new object());

            var instance1 = _registry.GetComponent(typeof(object));
            var instance2 = _registry.GetComponent(typeof(object));

            Assert.AreSame(instance1, instance2);
        }

        [TestCase]
        public void Merge_TwoRegistries_ContainsAllRegistrations()
        {
            var registry1 = new global::ModuleInject.Registry.Registry();
            var registry2 = new global::ModuleInject.Registry.Registry();

            registry1.Register(() => new object());
            registry2.Register(() => 1);

            var mergedRegistry = registry1.Merge(registry2);

            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(int)));
            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(object)));
        }

        [TestCase]
        public void Merge_TwoRegistries_OtherRegistryDoesNotOverrideFirstRegistry()
        {
            var registry1 = new global::ModuleInject.Registry.Registry();
            var registry2 = new global::ModuleInject.Registry.Registry();

            registry1.Register(() => new object());
            registry2.Register(() => new object());

            var mergedRegistry = registry1.Merge(registry2);

            var instance1 = registry1.GetComponent(typeof(object));
            var instance2 = registry2.GetComponent(typeof(object));
            var mergedInstance = mergedRegistry.GetComponent(typeof(object));

            Assert.AreNotSame(instance1, instance2);
            Assert.AreSame(instance1, mergedInstance);
        }

        [TestCase]
        public void Dispose_RegistryWithResolvedEntries_RegistryAndAllEntriesAreDisposed()
        {
            _registry.Register(() => new object());

            var instance = _registry.GetComponent(typeof(object));

            _registry.Dispose();

            Assert.IsTrue(_registry.IsDisposed);
            //foreach (var registrationEntry in _registry.GetRegistrationEntries())
            //{
            //    Assert.IsTrue(registrationEntry.IsDisposed);
            //}
        }
    }
}
