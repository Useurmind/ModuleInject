using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Injection;
using ModuleInject.Provider;
using ModuleInject.Provider.ProviderFactory;
using Moq;
using NUnit.Framework;

namespace Test.ModuleInject.Provider.ProviderFactory
{
    [TestFixture]
    public class ModuleServiceProviderExtensionsTest
    {
        private ServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            this.serviceProvider = new ServiceProvider();
        }

        [Test]
        public void FromModule_BaseModule_CorrectlySetup()
        {
            var module = new TestBaseModule1();

            serviceProvider.FromModule(module);

            Assert.AreEqual(2, serviceProvider.NumberOfServices);
            Assert.AreSame(module.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(module.GetService2(), serviceProvider.GetService<IService2>());
        }

        [Test]
        public void FromModule_DerivedModule_CorrectlySetup()
        {
            var module = new TestDerivedModule();

            serviceProvider.FromModule(module);

            Assert.AreEqual(5, serviceProvider.NumberOfServices);
            Assert.AreSame(module.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(module.GetService2(), serviceProvider.GetService<IService2>());
            Assert.AreSame(module.Service4, serviceProvider.GetService<IService4>());
            Assert.AreSame(module.Service5, serviceProvider.GetService<IService5>());
            Assert.AreSame(module.GetService3(), serviceProvider.GetService<IService3>());
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

            public IService2 GetService2() { return service2; }
        }

        private class TestBaseModule1 : TestBaseModule<TestBaseModule1>
        { }

        private class TestDerivedModule : TestBaseModule<TestDerivedModule>
        {
            private IService3 service3 = Mock.Of<IService3>();

            public IService4 Service4 { get; set; } = Mock.Of<IService4>();

            public IService5 Service5 { get; set; } = Mock.Of<IService5>();

            public IService3 GetService3() { return service3; }
        }
    }
}
