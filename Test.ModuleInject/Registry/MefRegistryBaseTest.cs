#if !NET_35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modularity.Registry;

namespace Test.ModuleInject.Registry
{
    using NUnit.Framework;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    public interface ICanBeRegistered { }
    public class CanBeRegistered : ICanBeRegistered { }

    public interface ICanBeImported
    {
        
    }

    [Export(typeof(ICanBeImported))]
    public class CanBeImported : ICanBeImported { }

    public class RegistryBaseDerivate : MefRegistryBase
    {
        [Import(typeof(ICanBeImported))]
        public ICanBeImported ImportedPart { get; set; }

        public ICanBeRegistered RegisteredPart { get; set; }

        public RegistryBaseDerivate()
        {            
            var catalog = new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly());
            this.AddCatalog(catalog);

            RegisteredPart = new CanBeRegistered();

            this.Register(() => this.RegisteredPart);
        }
    }

    [TestFixture]
    public class MefRegistryBaseTest
    {
        private RegistryBaseDerivate registry;

        [SetUp]
        public void Setup()
        {
            this.registry = new RegistryBaseDerivate();
        }

        [TestCase]
        public void Compose_ImportingParts_AllImportedPartsResolved()
        {
            this.registry.Compose();

            Assert.IsNotNull(this.registry.ImportedPart);
        }

        [TestCase]
        public void IsRegistered_ForImportedPart_ReturnsTrue()
        {
            bool result = this.registry.IsRegistered(typeof(ICanBeImported));

            Assert.IsTrue(result);
        }

        [TestCase]
        public void IsRegistered_ForRegisteredPart_ReturnsTrue()
        {
            bool result = this.registry.IsRegistered(typeof(ICanBeRegistered));

            Assert.IsTrue(result);
        }

        [TestCase]
        public void GetComponent_ForImportedPart_ReturnsImportedPartInstance()
        {
            object result = this.registry.GetComponent(typeof(ICanBeImported));

            Assert.AreSame(this.registry.ImportedPart, result);
        }

        [TestCase]
        public void GetComponent_ForRegisteredPart_ReturnsRegisteredPartInstance()
        {
            object result = this.registry.GetComponent(typeof(ICanBeRegistered));

            Assert.AreSame(this.registry.RegisteredPart, result);
        }

        [TestCase]
        public void GetComponent_ForRegisteredPart_ReturnsAlwaysSameInstance()
        {
            var instance1 = registry.GetComponent(typeof(ICanBeRegistered));
            var instance2 = registry.GetComponent(typeof(ICanBeRegistered));

            Assert.AreSame(instance1, instance2);
        }

        [TestCase]
        public void Merge_TwoRegistries_ContainsAllRegistrations()
        {
            var registry1 = new RegistryBaseDerivate();
            var registry2 = new RegistryBaseDerivate();

            registry2.Register(() => 1);

            var mergedRegistry = registry1.Merge(registry2);

            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(ICanBeRegistered)));
            Assert.IsTrue(mergedRegistry.IsRegistered(typeof(int)));
        }

        [TestCase]
        public void Merge_TwoRegistries_OtherRegistryDoesNotOverrideFirstRegistry()
        {
            var registry1 = new RegistryBaseDerivate();
            var registry2 = new RegistryBaseDerivate();

            var mergedRegistry = registry1.Merge(registry2);

            var instance1 = registry1.GetComponent(typeof(ICanBeRegistered));
            var instance2 = registry2.GetComponent(typeof(ICanBeRegistered));
            var mergedInstance = mergedRegistry.GetComponent(typeof(ICanBeRegistered));

            Assert.AreNotSame(instance1, instance2);
            Assert.AreSame(instance1, mergedInstance);
        }
    }
}
#endif