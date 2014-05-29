using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    [TestFixture]
    public class ModulePostResolveBuilderTest
    {
        private Mock<IInjectionModule> _moduleMock;
        private DoubleKeyDictionary<Type, string, IInstanceRegistrationContext> _instanceRegistrations;
        private Mock<IInstanceRegistrationContext> _instanceRegistrationMock;
        private Mock<IPostResolveAssembler> _assembler;
        private string _componentName;
        public object _instance;

        [SetUp]
        public void Init()
        {
            _moduleMock = new Mock<IInjectionModule>();
            _instanceRegistrations = new DoubleKeyDictionary<Type, string, IInstanceRegistrationContext>();
            _instanceRegistrationMock = new Mock<IInstanceRegistrationContext>();
            _assembler = new Mock<IPostResolveAssembler>();
            _componentName = "coasdfkdng";
            _instance = new object();

            _moduleMock.Setup(x => x.GetComponent(typeof(object), _componentName)).Returns(_instance);

            _instanceRegistrationMock.Setup(x => x.PostResolveAssemblers)
                .Returns(new IPostResolveAssembler[] { _assembler.Object });

            _instanceRegistrations.Add(typeof(object), _componentName, _instanceRegistrationMock.Object);
        }

        [TestCase]
        public void PerformPostResolveAssembly_WithOneAssemblerAndRegisteredInstance_AssemblePerformedOnInstance()
        {
            ModulePostResolveBuilder.PerformPostResolveAssembly(_moduleMock.Object, _instanceRegistrations);

            _assembler.Verify(x => x.Assemble(_instance, _moduleMock.Object), Times.Once);
        }
    }
}
