using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class InjectorModuleTest
    {
        private InjectorModule _module;
        [SetUp]
        public void Init()
        {
            this._module = new InjectorModule();
        }

        [TestCase]
        public void Resolve_RegisterClassInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            this._module.RegisterClassInjectorWithPropertyValueAndInitialize1();

            this._module.Resolve();

            Assert.IsNotNull(this._module.Component1);
            Assert.IsNotNull(this._module.Component1.MainComponent2);
            Assert.IsNotNull(this._module.Component1.MainComponent22);
            Assert.IsNotNull(this._module.Component1.MainComponent23);

            Assert.AreEqual(this._module.Component2, this._module.Component1.MainComponent2);
            Assert.AreEqual(this._module.Component22, this._module.Component1.MainComponent22);
        }

        [TestCase]
        public void Resolve_RegisterInterfaceInjectorWithPropertyValueAndInitialize1_EverythingIsCorrect()
        {
            this._module.RegisterInterfaceInjectorWithPropertyValueAndInitialize1();

            this._module.Resolve();

            Assert.IsNotNull(this._module.Component1);
            Assert.IsNotNull(this._module.Component1.MainComponent2);
            Assert.IsNotNull(this._module.Component1.MainComponent22);
            Assert.IsNotNull(this._module.Component1.MainComponent23);

            Assert.AreEqual(this._module.Component2, this._module.Component1.MainComponent2);
            Assert.AreEqual(this._module.Component22, this._module.Component1.MainComponent22);
        }

        [TestCase]
        public void Resolve_GetWrappedComponent_EverythingIsCorrect()
        {
            this._module.Resolve();

            var wrappedComponent = _module.WrappedComponent as MainComponent2Wrapper;

            Assert.IsNotNull(wrappedComponent);
            Assert.AreEqual(2, wrappedComponent.IntProperty);
            Assert.AreSame(_module.Component2, wrappedComponent.WrappedComponent);
        }
    }
}
