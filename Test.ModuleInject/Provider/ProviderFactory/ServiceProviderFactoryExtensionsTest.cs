using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Provider;
using ModuleInject.Provider.ProviderFactory;
using Moq;
using NUnit.Framework;

namespace Test.ModuleInject.Provider.ProviderFactory
{
    [TestFixture]
    public class ServiceProviderFactoryExtensionsTest
    {
        private ServiceProvider serviceProvider;
        private ChildClass instance;

        [SetUp]
        public void Setup()
        {
            instance = new ChildClass()
            {
                Service1 = Mock.Of<IService1>(),
                Service2 = Mock.Of<IService2>(),
                Service3 = Mock.Of<IService3>(),
                Service4 = Mock.Of<IService4>()
            };

            serviceProvider = new ServiceProvider();
        }

        [Test]
        public void AddAllProperties_AddsAllProperties()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .Extract();

            Assert.AreSame(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllProperties_ExceptOne_ExceptedPropertyNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .ExceptProperty("Service1")
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllProperties_ExceptBaseType_ExceptedPropertiesNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .ExceptPropertiesFrom<BaseClass>()
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllProperties_ExceptOneAndBaseType_ExceptedPropertiesNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .ExceptProperty("Service4")
                .ExceptPropertiesFrom<BaseClass>()
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddAllProperties_ExceptNonRecursive_OnlyTopmostRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .ExceptPropertiesFrom<ChildClass>()
                .Extract();
            
            Assert.AreSame(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.IsFalse(serviceProvider.HasService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddAllProperties_ExceptRecursive_AllRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AddAllProperties()
                .ExceptPropertiesFrom<ChildClass>(true)
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.IsFalse(serviceProvider.HasService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        public interface IService1 { }
        public interface IService2 { }
        public interface IService3 { }
        public interface IService4 { }

        private interface IBaseClass
        {
            IService1 Service1 { get; set; }

            IService2 Service2 { get; set; }

        }

        private class BaseClass : IBaseClass
        {
            public IService1 Service1 { get; set; }

            public IService2 Service2 { get; set; }
        }

        private interface IChildClass
        {
            IService3 Service3 { get; set; }

            IService4 Service4 { get; set; }
        }

        private class ChildClass : BaseClass, IChildClass
        {
            public IService3 Service3 { get; set; }

            public IService4 Service4 { get; set; }
        }
    }
}
