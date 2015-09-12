using System.Linq;

using ModuleInject.Modularity.Registry;

using NUnit.Framework;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class RegistryModuleTest
    {
        private global::ModuleInject.Modularity.Registry.StandardRegistry _registry;

        [SetUp]
        public void Setup()
        {
            this._registry = new global::ModuleInject.Modularity.Registry.StandardRegistry();
        }

        [TestCase]
        public void IsRegistered_ForRegisteredType_ReturnsTrue()
        {
            this._registry.Register(() => 1);

            Assert.IsTrue(this._registry.IsRegistered(typeof(int)));
        }

        [TestCase]
        public void GetComponent_ForRegisteredType_ReturnsInstance()
        {
            this._registry.Register(() => 1);

            Assert.AreEqual(1, this._registry.GetComponent(typeof(int)));
        }

        [TestCase]
        public void GetComponent_ForRegisteredType_ReturnsAlwaysSameInstance()
        {
            this._registry.Register(() => new object());

            var instance1 = this._registry.GetComponent(typeof(object));
            var instance2 = this._registry.GetComponent(typeof(object));

            Assert.AreSame(instance1, instance2);
        }

        [TestCase]
        public void Merge_TwoRegistries_ContainsAllRegistrations()
        {
            var registry1 = new global::ModuleInject.Modularity.Registry.StandardRegistry();
            var registry2 = new global::ModuleInject.Modularity.Registry.StandardRegistry();

            registry1.Register(() => new object());
            registry2.Register(() => 1);

            var mergedRegistry = registry1.Merge(registry2);

            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(int)));
            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(object)));
        }

        [TestCase]
        public void Merge_TwoRegistries_OtherRegistryDoesNotOverrideFirstRegistry()
        {
            var registry1 = new global::ModuleInject.Modularity.Registry.StandardRegistry();
            var registry2 = new global::ModuleInject.Modularity.Registry.StandardRegistry();

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
            this._registry.Register(() => new object());

            var instance = this._registry.GetComponent(typeof(object));

            this._registry.Dispose();

            Assert.IsTrue(this._registry.IsDisposed);
            //foreach (var registrationEntry in _registry.GetRegistrationEntries())
            //{
            //    Assert.IsTrue(registrationEntry.IsDisposed);
            //}
        }
    }
}
