using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Modularity.Registry;
using Moq;
using Xunit;

namespace Test.ModuleInject.Injection
{
    
    public class InjectionContainerTest
    {
        private InjectionContainer container;

        public InjectionContainerTest()
        {
            this.container = new InjectionContainer();
        }

        [Theory]
        [InlineData(typeof(object), "")]
        [InlineData(typeof(object), "fdghdfghjgf")]
        public void TestRegisterObject(Type type, string name)
        {
            TestRegistration(type, name, () => new object());
        }

        [Theory]
        [InlineData(typeof(int), "")]
        [InlineData(typeof(int), "asdfg")]
        public void TestRegisterInt(Type type, string name)
        {
            TestRegistration(type, name, () => 234435);
        }

        [Fact]
        public void DoubleRegister_ExceptionThrown()
        {
            var type = typeof(object);
            var name = "asdrffgd";
            Func<object> factoryFunc = () => new object();

            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this.container.Register(type, name, factoryFunc);
                this.container.Register(type, name, factoryFunc);
            });            
        }

        [Fact]
        public void GetComponent_NonRegisteredComponent_ExceptionThrown()
        {
            var type = typeof(object);
            var name = "asdrffgd";

            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this.container.GetComponent(type, name);
            });
        }

        [Fact]
        public void Dispose_WithDisposableComponentResolved_ComponentDisposed()
        {
            var type = typeof(object);
            var name = "asdrffgd";

            var disposableMock = new Mock<ITestDisposable>();

            this.container.Register(type, name, () => disposableMock.Object);

            var mockObject = this.container.GetComponent(type, name);

            this.container.Dispose();

            Assert.Same(disposableMock.Object, mockObject);
            disposableMock.Verify(x => x.Dispose(), Times.Once);
        }

        private void TestRegistration(Type type, string name, Func<object> factoryFunc)
        {
            this.container.Register(type, name, factoryFunc);

            bool isRegistered = this.container.IsRegistered(type, name);
            object instance1 = this.container.GetComponent(type, name);
            object instance2 = this.container.GetComponent(type, name);

            Assert.True(isRegistered);
            Assert.NotNull(instance1);
            Assert.Same(instance1, instance2);
        }

        public interface ITestDisposable : IDisposable { }
    }
}
