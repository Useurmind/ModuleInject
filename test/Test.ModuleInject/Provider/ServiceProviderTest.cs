using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces.Provider;
using ModuleInject.Provider;
using Moq;
using Xunit;

namespace Test.ModuleInject.Provider
{
    
    public class ServiceProviderTest
    {
        private ServiceProvider serviceProvider;

        public ServiceProviderTest()
        {
            this.serviceProvider = new ServiceProvider();
        }

        [Fact]
        public void GetService_CorrectSetup_InstanceReturned()
        {
            serviceProvider.AddServiceSource(SourceOf<ICloneable>());
            serviceProvider.AddServiceSource(SourceOf<IAsyncResult>());

            var result = serviceProvider.GetService<ICloneable>();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICloneable>(result);
        }

        [Fact]
        public void AddServiceSource_Twice_ThrowsException()
        {
            Assert.Throws(typeof(ModuleInjectException), () =>
            {
                serviceProvider.AddServiceSource(SourceOf<ICloneable>());
                serviceProvider.AddServiceSource(SourceOf<ICloneable>());
            });
        }

        [Fact]
        public void GetService_MissingRegistration_ReturnsNull()
        {
            serviceProvider.AddServiceSource(SourceOf<ICloneable>());
            serviceProvider.AddServiceSource(SourceOf<IAsyncResult>());

            var result = serviceProvider.GetService<IAppDomainSetup>();

            Assert.Null(result);
        }

        [Fact]
        public void GetService_NoRegistration_ReturnsNull()
        {
            var result = serviceProvider.GetService<ICloneable>();

            Assert.Null(result);
        }

        private static ISourceOfService SourceOf<TService>()
            where TService : class
        {
            var sourceMock = new Mock<ISourceOfService>();
            sourceMock.SetupGet(x => x.Type).Returns(typeof(TService));
            sourceMock.Setup(x => x.Get()).Returns(Mock.Of<TService>());
            return sourceMock.Object;
        }
    }
}
