using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Modularity.Registry;
using Moq;
using NUnit.Framework;

namespace Test.ModuleInject.Injection
{
    [TestFixture]
    public class InjectionContainerTest
    {
        private InjectionContainer container;

        [SetUp]
        public void Setup()
        {
            this.container = new InjectionContainer();
        }

        [TestCase(typeof(object), "")]
        [TestCase(typeof(object), "fdghdfghjgf")]
        public void TestRegisterObject(Type type, string name)
        {
            TestRegistration(type, name, () => new object());
        }

        [TestCase(typeof(int), "")]
        [TestCase(typeof(int), "asdfg")]
        public void TestRegisterInt(Type type, string name)
        {
            TestRegistration(type, name, () => 234435);
        }

        [Test]
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

        [Test]
        public void GetComponent_NonRegisteredComponent_ExceptionThrown()
        {
            var type = typeof(object);
            var name = "asdrffgd";

            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                this.container.GetComponent(type, name);
            });
        }

        [Test]
        public void Dispose_WithDisposableComponentResolved_ComponentDisposed()
        {
            var type = typeof(object);
            var name = "asdrffgd";

            var disposableMock = new Mock<ITestDisposable>();

            this.container.Register(type, name, () => disposableMock.Object);

            var mockObject = this.container.GetComponent(type, name);

            this.container.Dispose();

            Assert.AreSame(disposableMock.Object, mockObject);
            disposableMock.Verify(x => x.Dispose(), Times.Once);
        }

        private void TestRegistration(Type type, string name, Func<object> factoryFunc)
        {
            this.container.Register(type, name, factoryFunc);

            bool isRegistered = this.container.IsRegistered(type, name);
            object instance1 = this.container.GetComponent(type, name);
            object instance2 = this.container.GetComponent(type, name);

            Assert.IsTrue(isRegistered);
            Assert.IsNotNull(instance1);
            Assert.AreSame(instance1, instance2);
        }

        public interface ITestDisposable : IDisposable { }
    }
}
