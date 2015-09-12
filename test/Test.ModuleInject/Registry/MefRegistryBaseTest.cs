using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modularity.Registry;

namespace Test.ModuleInject.Registry
{
    using Xunit;
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

    
    public class MefRegistryBaseTest
    {
        private RegistryBaseDerivate registry;
        
        public MefRegistryBaseTest()
        {
            this.registry = new RegistryBaseDerivate();
        }

        [Fact]
        public void Compose_ImportingParts_AllImportedPartsResolved()
        {
            this.registry.Compose();

            Assert.NotNull(this.registry.ImportedPart);
        }

        [Fact]
        public void IsRegistered_ForImportedPart_ReturnsTrue()
        {
            bool result = this.registry.IsRegistered(typeof(ICanBeImported));

            Assert.True(result);
        }

        [Fact]
        public void IsRegistered_ForRegisteredPart_ReturnsTrue()
        {
            bool result = this.registry.IsRegistered(typeof(ICanBeRegistered));

            Assert.True(result);
        }

        [Fact]
        public void GetComponent_ForImportedPart_ReturnsImportedPartInstance()
        {
            object result = this.registry.GetComponent(typeof(ICanBeImported));

            Assert.Same(this.registry.ImportedPart, result);
        }

        [Fact]
        public void GetComponent_ForRegisteredPart_ReturnsRegisteredPartInstance()
        {
            object result = this.registry.GetComponent(typeof(ICanBeRegistered));

            Assert.Same(this.registry.RegisteredPart, result);
        }

        [Fact]
        public void GetComponent_ForRegisteredPart_ReturnsAlwaysSameInstance()
        {
            var instance1 = registry.GetComponent(typeof(ICanBeRegistered));
            var instance2 = registry.GetComponent(typeof(ICanBeRegistered));

            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void Merge_TwoRegistries_ContainsAllRegistrations()
        {
            var registry1 = new RegistryBaseDerivate();
            var registry2 = new RegistryBaseDerivate();

            registry2.Register(() => 1);

            var mergedRegistry = registry1.Merge(registry2);

            Assert.True(mergedRegistry.IsRegistered(typeof(ICanBeRegistered)));
            Assert.True(mergedRegistry.IsRegistered(typeof(int)));
        }

        [Fact]
        public void Merge_TwoRegistries_OtherRegistryDoesNotOverrideFirstRegistry()
        {
            var registry1 = new RegistryBaseDerivate();
            var registry2 = new RegistryBaseDerivate();

            var mergedRegistry = registry1.Merge(registry2);

            var instance1 = registry1.GetComponent(typeof(ICanBeRegistered));
            var instance2 = registry2.GetComponent(typeof(ICanBeRegistered));
            var mergedInstance = mergedRegistry.GetComponent(typeof(ICanBeRegistered));

            Assert.NotSame(instance1, instance2);
            Assert.Same(instance1, mergedInstance);
        }
    }
}