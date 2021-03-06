﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Provider;
using ModuleInject.Provider.ProviderFactory;
using Moq;
using Xunit;

namespace Test.ModuleInject.Provider.ProviderFactory
{
    
    public class ServiceProviderFactoryExtensionsTest
    {
        private ServiceProvider serviceProvider;
        private ChildClass instance;

        public ServiceProviderFactoryExtensionsTest()
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

        [Fact]
        public void AddAllProperties_AddsAllProperties()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .Extract();

            Assert.Same(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllProperties_ExceptOne_ExceptedPropertyNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .Where(x => x.Name != "Service1")
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllProperties_ExceptBaseType_ExceptedPropertiesNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllProperties_ExceptOneAndBaseType_ExceptedPropertiesNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .Where(x => x.Name != "Service4")
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllProperties_ExceptNonRecursive_OnlyTopmostRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .ExceptFrom<ChildClass>()
                .Extract();

            Assert.Same(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.False(serviceProvider.HasService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllProperties_ExceptRecursive_AllRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllProperties()
                .ExceptFrom<ChildClass>(true)
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.False(serviceProvider.HasService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_AddsAllMethods()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Extract();

            Assert.Same(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_ExceptOne_ExceptedMethodNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Where(x => x.Name != "GetService1")
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_ExceptBaseType_ExceptedMethodsNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.Service4, serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_ExceptOneAndBaseType_ExceptedMethodsNotAdded()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .Where(x => x.Name != "GetService4")
                .ExceptFrom<BaseClass>()
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_ExceptNonRecursive_OnlyTopmostRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<ChildClass>()
                .Extract();

            Assert.Same(instance.Service1, serviceProvider.GetService<IService1>());
            Assert.Same(instance.Service2, serviceProvider.GetService<IService2>());
            Assert.False(serviceProvider.HasService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllGetMethods_ExceptRecursive_AllRemoved()
        {
            serviceProvider.FromInstance(instance)
                .AllGetMethods()
                .ExceptFrom<ChildClass>(true)
                .Extract();

            Assert.False(serviceProvider.HasService<IService1>());
            Assert.False(serviceProvider.HasService<IService2>());
            Assert.False(serviceProvider.HasService<IService3>());
            Assert.False(serviceProvider.HasService<IService4>());
        }

        [Fact]
        public void AddAllMethodsAndProperties_FromConstrainedInterface_OnlyInterfaceAdded()
        {
            serviceProvider.FromInstance<IConstrainedChildClass>(instance)
                .AllProperties()
                .Extract()
                .AllGetMethods()
                .Extract();

            Assert.Equal(2, serviceProvider.NumberOfServices);
            Assert.Same(instance.Service3, serviceProvider.GetService<IService3>());
            Assert.Same(instance.GetService4(), serviceProvider.GetService<IService4>());
        }

        [Fact]
        public void AddLambdaSource_CorrectlyAdded()
        {
            var service1 = Mock.Of<IService1>();

            serviceProvider.AddServiceSource<IService1>(() => service1);

            var result = serviceProvider.GetService<IService1>();

            Assert.Same(service1, result);
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

        private interface IConstrainedChildClass
        {
            IService3 Service3 { get; set; }

            IService4 GetService4();
        }

        private class ChildClass : BaseClass, IChildClass, IConstrainedChildClass
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
