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
                .AllProperties()
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
                .AllProperties()
                .Where(x => x.Name != "Service1")
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
                .AllProperties()
                .ExceptFrom<BaseClass>()
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
                .AllProperties()
                .Where(x => x.Name != "Service4")
                .ExceptFrom<BaseClass>()
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
                .AllProperties()
                .ExceptFrom<ChildClass>()
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
                .AllProperties()
                .ExceptFrom<ChildClass>(true)
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.IsFalse(serviceProvider.HasService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_AddsAllMethods()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Extract();

            Assert.AreSame(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_ExceptOne_ExceptedMethodNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Where(x => x.Name != "GetService1")
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_ExceptBaseType_ExceptedMethodsNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.AreSame(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_ExceptOneAndBaseType_ExceptedMethodsNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Where(x => x.Name != "GetService4")
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.AreSame(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_ExceptNonRecursive_OnlyTopmostRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<ChildClass>()
                .Extract();

            Assert.AreSame(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.AreSame(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.IsFalse(serviceProvider.HasService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddAllGetMethods_ExceptRecursive_AllRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<ChildClass>(true)
                .Extract();

            Assert.IsFalse(serviceProvider.HasService<IService1>());
            Assert.IsFalse(serviceProvider.HasService<IService2>());
            Assert.IsFalse(serviceProvider.HasService<IService3>());
            Assert.IsFalse(serviceProvider.HasService<IService4>());
        }

        [Test]
        public void AddLambdaSource_CorrectlyAdded()
        {
            var service1 = Mock.Of<IService1>();

            serviceProvider.AddServiceSource<IService1>(() => service1);

            var result = serviceProvider.GetService<IService1>();

            Assert.AreSame(service1, result);
        }

        public interface IService1 { }
        public interface IService2 { }
        public interface IService3 { }
        public interface IService4 { }

        private interface IBaseClass
        {
            IService1 Service1 { get; set; }

            IService2 Service2 { get; set; }

            IService1 GetService1();
            IService2 GetService2();
        }

        private class BaseClass : IBaseClass
        {
            public IService1 Service1 { get; set; }

            public IService2 Service2 { get; set; }

            public IService1 GetService1() { return Service1; }
            public IService2 GetService2() { return Service2; }
        }

        private interface IChildClass
        {
            IService3 Service3 { get; set; }

            IService4 Service4 { get; set; }

            IService3 GetService3();
            IService4 GetService4();
            IService3 GetService3(int a, float b);

            IService3 GetService3WithArgs(int a);

            void NonGetterNoArgs();

            void NonGetterWithArgs(int a);
        }

        private class ChildClass : BaseClass, IChildClass
        {
            public IService3 Service3 { get; set; }

            public IService4 Service4 { get; set; }

            public ChildClass()
            {
            }

            public IService3 GetService3() { return Service3; }
            public IService4 GetService4() { return Service4; }

            public IService3 GetService3(int a, float b) { return Service3; }

            public IService3 GetService3WithArgs(int a) { return Service3; }

            public void NonGetterNoArgs() { }

            public void NonGetterWithArgs(int a) { }
        }
    }
}
