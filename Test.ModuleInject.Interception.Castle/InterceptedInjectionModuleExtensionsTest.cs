using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ModuleInject.Interfaces;
using ModuleInject.Injection;
using Castle.DynamicProxy;
using ModuleInject.Interception.Castle;

namespace Test.ModuleInject.Interception.Castle
{
    [TestFixture]
    public class InterceptedInjectionModuleExtensionsTest
    {
        [Test]
        public void InterceptSingleInstance_WithSingleInterceptor()
        {
            var module = new TestModule();

            module.Resolve();

            var callStack = module.CallStack;
            var testComponent = module.TestComponentWithSingleInterceptor;

            testComponent.DoSomething();

            Assert.IsNotInstanceOf<TestComponent>(testComponent);
            Assert.IsInstanceOf<TestInterceptor>(callStack.Pop());
            Assert.IsInstanceOf<TestComponent>(callStack.Pop());
            Assert.IsInstanceOf<TestInterceptor>(callStack.Pop());
        }

        [Test]
        public void InterceptSingleInstance_WithMultipleInterceptors()
        {
            var module = new TestModule();

            module.Resolve();

            var callStack = module.CallStack;
            var testComponent = module.TestComponentWithMultipleInterceptors;

            testComponent.DoSomething();

            var interceptor1 = (TestInterceptor)callStack.Pop();
            Assert.AreEqual(1, interceptor1.Number);

            var interceptor2 = (TestInterceptor)callStack.Pop();
            Assert.AreEqual(2, interceptor2.Number);

            Assert.IsInstanceOf<TestComponent>(callStack.Pop());

            interceptor2 = (TestInterceptor)callStack.Pop();
            Assert.AreEqual(2, interceptor2.Number);

            interceptor1 = (TestInterceptor)callStack.Pop();
            Assert.AreEqual(1, interceptor1.Number);
        }

        [Test]
        public void InterceptFactory_WithSingleInterceptor()
        {
            var module = new TestModule();

            module.Resolve();

            var callStack = module.CallStack;
            var testComponent1 = module.CreateTestComponentWithSingleInterceptor();
            var testComponent2 = module.CreateTestComponentWithSingleInterceptor();

            testComponent1.DoSomething();
            testComponent2.DoSomething();

            var interceptor2After = (TestInterceptor)callStack.Pop();
            var component2 = (TestComponent)callStack.Pop();
            var interceptor2Before = (TestInterceptor)callStack.Pop();
            var interceptor1After = (TestInterceptor)callStack.Pop();
            var component1 = (TestComponent)callStack.Pop();
            var interceptor1Before = (TestInterceptor)callStack.Pop();

            Assert.AreNotSame(testComponent1, testComponent2);
            Assert.AreNotSame(component1, component2);
            Assert.AreSame(interceptor1Before, interceptor1After);
            Assert.AreSame(interceptor2Before, interceptor2After);
            Assert.AreNotSame(interceptor1After, interceptor2After);
        }

        public interface ITestComponent
        {
            void DoSomething();
        }

        private class TestComponent : ITestComponent
        {
            public Stack<object> CallStack { get; set; }

            public void DoSomething()
            {
                CallStack.Push(this);
            }
        }

        private class TestInterceptor : IInterceptor
        {
            public Stack<object> CallStack { get; set; }

            public int Number { get; set; }

            public void Intercept(IInvocation invocation)
            {
                CallStack.Push(this);
                invocation.Proceed();
                CallStack.Push(this);
            }
        }

        private interface ITestModule : IModule
        {

        }

        private class TestModule : InjectionModule<TestModule>, ITestModule
        {
            public ITestComponent TestComponentWithSingleInterceptor
            {
                get
                {
                    return this.GetSingleInstance<ITestComponent>(cc =>
                    {
                        cc.Construct(m => new TestComponent()
                        {
                            CallStack = m.CallStack
                        })
                        .AddInterceptor(m => new TestInterceptor()
                        {
                            CallStack = m.CallStack
                        });
                    });
                }
            }

            public ITestComponent TestComponentWithMultipleInterceptors
            {
                get
                {
                    return this.GetSingleInstance<ITestComponent>(cc =>
                    {
                        cc.Construct(m => new TestComponent()
                        {
                            CallStack = m.CallStack
                        })
                        .AddInterceptor(m => new TestInterceptor()
                        {
                            CallStack = m.CallStack,
                            Number = 1
                        })
                        .AddInterceptor(m => new TestInterceptor()
                        {
                            CallStack = m.CallStack,
                            Number = 2
                        });
                    });
                }
            }

            public ITestComponent CreateTestComponentWithSingleInterceptor()
            {
                return this.GetFactory<ITestComponent>(cc =>
                {
                    cc.Construct(m => new TestComponent()
                    {
                        CallStack = m.CallStack
                    })
                    .AddInterceptor(m => new TestInterceptor()
                    {
                        CallStack = this.CallStack
                    });
                });
            }

            public Stack<object> CallStack
            {
                get
                {
                    return this.GetSingleInstance<Stack<Object>>();
                }
            }
        }
    }
}
