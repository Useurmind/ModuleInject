using ModuleInject.Hooks;
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
        private interface IHookedModule { }

        private interface IHookedComponent { }

        private class TestModule1 : IHookedModule
        {

        }

        private class TestModule2 { }

        private class TestModule3 : TestModule1 { }

        private class TestComponent1 : IHookedComponent
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
            var registrationContext = new RegistrationContext
        }
    }
}
