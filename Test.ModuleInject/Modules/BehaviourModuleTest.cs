using System.Linq;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
{
    [TestFixture]
    public class BehaviourModuleTest
    {
        private BehaviourModule _module;

        [SetUp]
        public void Init()
        {
            this._module = new BehaviourModule();
        }

        [TestCase]
        public void CallIntFunctionReturns5_OnComponentWithChangeReturnBehaviour_ReturnsChangedReturnValue()
        {
            this._module.RegisterBehaviour();
            this._module.Resolve();

            var result = this._module.InterceptedWithChangeReturnValueComponent.FunctionReturns5();

            Assert.AreEqual(ChangeReturnValueBehaviour.ReturnValue, result);
        }
    }
}
