using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class CustomActionModuleTest
    {
        private CustomActionModule _customActionModule;

        [SetUp]
        public void Setup()
        {
            this._customActionModule = new CustomActionModule();
        }

        [TestCase]
        public void Resolve_RegisterWithClosure_ConstructFromType_CorrectlyResolved()
        {
            this._customActionModule.RegisterWithClosure_ConstructFromType();
            this._customActionModule.Resolve();

            Assert.IsNotNull(this._customActionModule.MainComponent1);
            Assert.IsNotNull(this._customActionModule.MainComponent2);
            Assert.AreSame(this._customActionModule.MainComponent2, this._customActionModule.MainComponent1.MainComponent2);
        }

        [TestCase]
        public void Resolve_RegisterInInterfaceInjector_ConstructFromInstance_CorrectlyResolved()
        {
            this._customActionModule.RegisterInInterfaceInjector_ConstructFromInstance();
            this._customActionModule.Resolve();

            Assert.IsNotNull(this._customActionModule.MainComponent1);
            Assert.IsNotNull(this._customActionModule.MainComponent2);
            Assert.AreSame(this._customActionModule.MainComponent2, this._customActionModule.MainComponent1.MainComponent2);
        }
    }
}
