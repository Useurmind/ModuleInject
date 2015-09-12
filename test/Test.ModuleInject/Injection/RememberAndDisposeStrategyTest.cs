using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Injection;
using Moq;
using Xunit;

namespace Test.ModuleInject.Injection
{
    
    public class RememberAndDisposeStrategyTest
    {
        [Fact]
        public void AddInstancesAndDispose_AllDisposablesDisposed()
        {
            var disposableMock = new Mock<IDisposable>();
            var disposableMock2 = new Mock<IDisposable>();
            var nonDisposableMock = new Mock<IEnumerable<object>>();
            var nonDisposableMock2 = new Mock<IEnumerable<object>>();

            var strategy = new RememberAndDisposeStrategy();

            strategy.OnInstance(disposableMock.Object);
            strategy.OnInstance(disposableMock.Object);
            strategy.OnInstance(nonDisposableMock.Object);
            strategy.OnInstance(disposableMock2.Object);
            strategy.OnInstance(nonDisposableMock2.Object);

            strategy.Dispose();
            strategy.Dispose();

            disposableMock.Verify(x => x.Dispose(), Times.Once);
            disposableMock2.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
