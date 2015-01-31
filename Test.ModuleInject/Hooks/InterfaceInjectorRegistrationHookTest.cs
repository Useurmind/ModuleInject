using ModuleInject.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Modularity;
using ModuleInject.Modules.Fluent;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Hooks
{
    [TestFixture]
    public class InterfaceInjectorRegistrationHookTest
    {
        private interface IHookedModule : IModule { }

        private interface IHookedComponent { }
        private interface INonHookedComponent2 { }

        private class TestModule1 : Module, IHookedModule
        {
            protected override void OnRegistryResolved(IRegistry usedRegistry)
            {
                throw new NotImplementedException();
            }
        }

        private class TestModule2 { }

        private class TestModule3 : TestModule1 { }

        private class TestComponent1 : IHookedComponent
        {

        }

        private class TestComponent2 : INonHookedComponent2
        {

        }


        [Test]
        public void AppliesToModule_ForSimpleImplementingModule_ReturnsTrue()
        {
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(null);

            var result = hook.AppliesToModule(new TestModule1());

            Assert.IsTrue(result);
        }

        [Test]
        public void AppliesToModule_ForNonImplementingModule_ReturnsFalse()
        {
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(null);

            var result = hook.AppliesToModule(new TestModule2());

            Assert.IsFalse(result);
        }

        [Test]
        public void AppliesToModule_ForInheritingImplementingModule_ReturnsTrue()
        {
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(null);

            var result = hook.AppliesToModule(new TestModule3());

            Assert.IsTrue(result);
        }

        [Test]
        public void AppliesToComponent_ForSimpleImplemetingComponent_ReturnsTrue()
        {
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(null);
            var registrationContext = new RegistrationContext("asd", new TestModule1(), null, new RegistrationTypes()
            {
                IComponent = typeof(IHookedComponent),
                TComponent = typeof(TestComponent1),
                IModule = typeof(IHookedModule),
                TModule = typeof(TestModule1)
            });

            var result = hook.AppliesToRegistration(registrationContext);

            Assert.IsTrue(result);
        }

        [Test]
        public void AppliesToComponent_ForSimpleNonImplemetingComponent_ReturnsFalse()
        {
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(null);
            var registrationContext = new RegistrationContext("asd", new TestModule1(), null, new RegistrationTypes()
            {
                IComponent = typeof(INonHookedComponent2),
                TComponent = typeof(TestComponent2),
                IModule = typeof(IHookedModule),
                TModule = typeof(TestModule1)
            });

            var result = hook.AppliesToRegistration(registrationContext);

            Assert.IsFalse(result);
        }

        [Test]
        public void Execute__InjectIntoCalled()
        {
            IInterfaceRegistrationContext<IHookedComponent, IHookedModule> usedContext = null;
            var injector = new InterfaceInjector<IHookedComponent, IHookedModule>(ctx => usedContext = ctx);
            var registrationContextMock = new Mock<IRegistrationContext>();
            var hook = new InterfaceInjectorRegistrationHook<IHookedComponent, IHookedModule>(injector);

            hook.Execute(registrationContextMock.Object);

            Assert.IsNotNull(usedContext);
            Assert.AreSame(registrationContextMock.Object, usedContext.Context);
        }
    }
}
