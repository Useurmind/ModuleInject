using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Injection;
using ModuleInject.Modularity;
using ModuleInject.Provider;
using ModuleInject.Provider.ProviderFactory;
using Moq;
using Xunit;

namespace Test.ModuleInject.Provider.ProviderFactory
{
    
    public class ModuleServiceProviderExtensionsTest
    {
        private ServiceProvider serviceProvider;

        public ModuleServiceProviderExtensionsTest()
        {
            this.serviceProvider = new ServiceProvider();
        }

        [Fact]
        public void FromModule_BaseModule_CorrectlySetup()
        {
            var module = new TestBaseModule1();

            serviceProvider.FromModuleExtractAll(module);

            Assert.Equal(2, serviceProvider.NumberOfServices);
            Assert.Same(module.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(module.GetService2(), serviceProvider.GetService<IService2>());
        }

        [Fact]
        public void FromModule_DerivedModule_CorrectlySetup()
        {
            var module = new TestDerivedModule();

            serviceProvider.FromModuleExtractAll(module);

            Assert.Equal(5, serviceProvider.NumberOfServices);
            Assert.Same(module.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(module.GetService2(), serviceProvider.GetService<IService2>());
            Assert.Same(module.Service4, serviceProvider.GetService<IService4>());
            Assert.Same(module.Service5, serviceProvider.GetService<IService5>());
            Assert.Same(module.GetService3(), serviceProvider.GetService<IService3>());
        }

        public interface IService1 { }
        public interface IService2 { }
        public interface IService3 { }
        public interface IService4 { }
        public interface IService5 { }

        private class TestBaseModule<TModule> : InjectionModule<TModule>
            where TModule : TestBaseModule<TModule>
        {
            private IService2 service2 = Mock.Of<IService2>();

            public IService1 Service1 { get; set; } = Mock.Of<IService1>();

            [FromRegistry]
            public IService1 Service1FromRegistry { get; set; }

            public IService2 GetService2() { return service2; }
        }

        private class TestBaseModule1 : TestBaseModule<TestBaseModule1>
        { }

        private class TestDerivedModule : TestBaseModule<TestDerivedModule>
        {
            private IService3 service3 = Mock.Of<IService3>();

            [FromRegistry]
            public IService4 Service4FromRegistry { get; set; }

            public IService4 Service4 { get; set; } = Mock.Of<IService4>();

            public IService5 Service5 { get; set; } = Mock.Of<IService5>();

            public IService3 GetService3() { return service3; }
        }
    }
}
