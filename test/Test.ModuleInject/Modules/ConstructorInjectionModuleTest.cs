using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class ConstructorInjectionModuleTest
    {
        private ConstructorInjectionModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new ConstructorInjectionModule();
        }

        [TestCase]
        public void Resolved_RegisterWithDefaultConstructor_MainComponent2Empty()
        {
            this._module.RegisterWithDefaultConstructor();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent1);
            Assert.IsNotNull(this._module.MainComponent2);
            Assert.IsNull(this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolved_RegisterWithArgumentsInConstructor_MainComponent2Set()
        {
            this._module.RegisterWithArgumentsInConstructor();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent1);
            Assert.IsNotNull(this._module.MainComponent2);
            Assert.AreSame(this._module.MainComponent2, this._module.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolved_RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis_MainComponent2Set()
        {
            this._module.RegisterWithArgumentsInConstructorAndArgumentResolvedAfterThis();
            this._module.Resolve();

            Assert.IsNotNull(this._module.MainComponent1);
            Assert.IsNotNull(this._module.MainComponent3);
            Assert.AreSame(this._module.MainComponent3, this._module.MainComponent1.MainComponent2);
        }
    }
}
