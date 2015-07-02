using ModuleInject.Injection;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Modularity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.Hooks
{
    [TestFixture]
    public class ExactInterfaceInjectorRegistrationHookTest
    {
        private interface IHookedModule : IModule { }

        private interface IHookedComponent { }
        private interface INonHookedComponent2 { }

        private interface ISomeRandomInterface { }

        private class TestModule1 : Module, IHookedModule
        {
            protected override void OnRegistryResolved(IRegistry usedRegistry)
            {
                throw new NotImplementedException();
            }
        }

		private class TestModule2 : Module
		{
			protected override void OnRegistryResolved(IRegistry usedRegistry)
			{
			}
		}

		private class TestModule3 : TestModule1 { }

        private class TestComponent1 : IHookedComponent, ISomeRandomInterface
        {

        }

        private class TestComponent2 : INonHookedComponent2
        {

        }


        [Test]
        public void AppliesToModule_ForSimpleImplementingModule_ReturnsTrue()
        {
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);

            var result = hook.AppliesToModule(new TestModule1());

            Assert.IsTrue(result);
        }

		[Test]
		public void AppliesToModule_ForSimpleNonImplementingModule_ReturnsFalse()
		{
			var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);

			var result = hook.AppliesToModule(new TestModule2());

			Assert.IsFalse(result);
		}

		[Test]
        public void AppliesToModule_ForInheritingImplementingModule_ReturnsTrue()
        {
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);

            var result = hook.AppliesToModule(new TestModule3());

            Assert.IsTrue(result);
        }

        [Test]
        public void AppliesToComponent_ForSimpleImplemetingComponent_ReturnsTrue()
        {
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);
            var injectionRegister = new InjectionRegister(null, typeof(TestModule1), typeof(IHookedComponent), typeof(TestComponent1));

			injectionRegister.SetContext(new TestModule1());
			injectionRegister.AddMeta("asd");

            var result = hook.AppliesToRegistration(injectionRegister);

            Assert.IsTrue(result);
        }


        [Test]
        public void AppliesToComponent_ForNonExactImplemetingComponent_ReturnsFalse()
        {
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);
            var injectionRegister = new InjectionRegister(null, typeof(TestModule1), typeof(ISomeRandomInterface), typeof(TestComponent1));

            injectionRegister.SetContext(new TestModule1());
            injectionRegister.AddMeta("asd");

            var result = hook.AppliesToRegistration(injectionRegister);

            Assert.IsFalse(result);
        }

        [Test]
        public void AppliesToComponent_ForSimpleNonImplemetingComponent_ReturnsFalse()
        {
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(null);
			var injectionRegister = new InjectionRegister(typeof(TestModule1), typeof(INonHookedComponent2), typeof(TestComponent2));

			injectionRegister.SetContext(new TestModule1());
			injectionRegister.AddMeta("asd");

			var result = hook.AppliesToRegistration(injectionRegister);

            Assert.IsFalse(result);
        }

        [Test]
        public void Execute__InjectIntoCalled()
        {
            IExactInterfaceModificationContext<IHookedModule, IHookedComponent> usedContext = null;
            var injector = new ExactInterfaceInjector<IHookedModule, IHookedComponent>(ctx => usedContext = ctx);
            var injectionRegisterMock = new Mock<IInjectionRegister>();
            var hook = new ExactInterfaceInjectorRegistrationHook<IHookedModule, IHookedComponent>(injector);

            hook.Execute(injectionRegisterMock.Object);

            Assert.IsNotNull(usedContext);
            Assert.AreSame(injectionRegisterMock.Object, usedContext.Register);
        }
    }
}
