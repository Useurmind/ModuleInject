using System.Linq;

using ModuleInject.Modularity.Registry;

using Xunit;

namespace Test.ModuleInject.Modules
{
    
    public class RegistryModuleTest
    {
        private global::ModuleInject.Modularity.Registry.StandardRegistry _registry;

        public RegistryModuleTest()
        {
            this._registry = new global::ModuleInject.Modularity.Registry.StandardRegistry();
        }

        [Fact]
        public void IsRegistered_ForRegisteredType_ReturnsTrue()
        {
            this._registry.Register(() => 1);

            Assert.True(this._registry.IsRegistered(typeof(int)));
        }

        [Fact]
        public void GetComponent_ForRegisteredType_ReturnsInstance()
        {
            this._registry.Register(() => 1);

            Assert.Equal(1, this._registry.GetComponent(typeof(int)));
        }

        [Fact]
        public void GetComponent_ForRegisteredType_ReturnsAlwaysSameInstance()
        {
            this._registry.Register(() => new object());

            var instance1 = this._registry.GetComponent(typeof(object));
            var instance2 = this._registry.GetComponent(typeof(object));

            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Merge_TwoRegistries_ContainsAllRegistrations()
        {
            var registry1 = new global::ModuleInject.Modularity.Registry.StandardRegistry();
            var registry2 = new global::ModuleInject.Modularity.Registry.StandardRegistry();

            registry1.Register(() => new object());
            registry2.Register(() => 1);

            var mergedRegistry = registry1.Merge(registry2);

            Assert.True(mergedRegistry.IsRegistered(typeof(int)));
            Assert.True(mergedRegistry.IsRegistered(typeof(object)));
        }

        [Fact]
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

            Assert.NotSame(instance1, instance2);
            Assert.Same(instance1, mergedInstance);
        }

        [Fact]
        public void Dispose_RegistryWithResolvedEntries_RegistryAndAllEntriesAreDisposed()
        {
            this._registry.Register(() => new object());

            var instance = this._registry.GetComponent(typeof(object));

            this._registry.Dispose();

            Assert.True(this._registry.IsDisposed);
            //foreach (var registrationEntry in _registry.GetRegistrationEntries())
            //{
            //    Assert.True(registrationEntry.IsDisposed);
            //}
        }
    }
}
