using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Injection;
using Xunit;

namespace Test.ModuleInject.Modules
{
    
    public class DisposeTest
    {
        private interface IDisposableComponent1
        {
            int Disposes { get; }
            bool IsDisposed { get; }
        }

        private interface IDisposableComponent2 : IDisposable
        {
            int Disposes { get; }
            bool IsDisposed { get; }
        }

        private class DisposableComponent1 : IDisposableComponent1, IDisposable
        {
            public int Disposes { get; set; }
            public bool IsDisposed { get; set; }
            public void Dispose()
            {
                Disposes++;
                IsDisposed = true;
            }
        }

        private class DisposableComponent2 : IDisposableComponent2
        {
            public int Disposes { get; set; }
            public bool IsDisposed { get; set; }
            public void Dispose()
            {
                Disposes++;
                IsDisposed = true;
            }
        }

        private interface INonDisposableComponent { }
        private class NonDisposableComponent: INonDisposableComponent
        {

        }

        private class DisposeTestModule : InjectionModule<DisposeTestModule>
        {
            public INonDisposableComponent NonDisposableComponent { get { return GetSingleInstance<NonDisposableComponent>(); } }
            public IDisposableComponent1 DisposableComponent1 { get { return GetSingleInstance<DisposableComponent1>(); } }
            public IDisposableComponent2 DisposableComponent2 { get { return GetSingleInstance<DisposableComponent2>(); } }
            
            public INonDisposableComponent CreateNonDisposableCompontent()
            {
                return GetFactory<NonDisposableComponent>();
            }

            public IDisposableComponent1 CreateDisposableCompontent1()
            {
                return GetFactory<DisposableComponent1>();
            }

            public IDisposableComponent2 CreateDisposableCompontent2()
            {
                return GetFactory<DisposableComponent2>();
            }

            public IDisposableComponent1 CreateManagedDisposableCompontent1()
            {
                return GetFactory<DisposableComponent1>();
            }

            public IDisposableComponent2 CreateManagedDisposableCompontent2()
            {
                return GetFactory<DisposableComponent2>();
            }
        }

        [Fact]
        public void Dispose_AfterResolveAllComponents_DisposeWasCorrect()
        {
            var module = new DisposeTestModule();

            module.Resolve();

            var disposableComponent1 = module.DisposableComponent1;
            var disposableComponent2 = module.DisposableComponent2;
            var nonDisposableComponent2 = module.NonDisposableComponent;
            var createdDisposableComponent1 = module.CreateDisposableCompontent1();
            var createdDisposableComponent2 = module.CreateDisposableCompontent2();
            var createdNonDisposableComponent = module.CreateNonDisposableCompontent();

            module.Dispose(); 
            module.Dispose();

            Assert.True(disposableComponent1.IsDisposed);
            Assert.Equal(1, disposableComponent1.Disposes);
            Assert.True(disposableComponent2.IsDisposed);
            Assert.Equal(1, disposableComponent2.Disposes);
            Assert.False(createdDisposableComponent1.IsDisposed);
            Assert.Equal(0, createdDisposableComponent1.Disposes);
            Assert.False(createdDisposableComponent2.IsDisposed);
            Assert.Equal(0, createdDisposableComponent2.Disposes);
        }
    }
}
