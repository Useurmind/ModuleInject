using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    class AfterResolveModuleTest
    {
        private AfterResolveModule _module;

        [SetUp]
        public void Init()
        {
            _module = new AfterResolveModule();
        }

        [TestCase]
        public void TestSimpleInitializeCallWithOtherComponent_OtherComponentInjected()
        {
            _module.RegisterComponentsByInitializeWithOtherComponent();
            _module.Resolve();

            Assert.IsNotNull(_module.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainComponent1.MainComponent2);
            Assert.AreSame(_module.MainComponent2, _module.MainInstance1.MainComponent2);

            Assert.AreEqual(3, _module.MainComponent1List.Count);
            Assert.AreSame(_module.MainComponent1, _module.MainComponent1List[0]);
            Assert.NotNull(_module.MainComponent1List[1]);
            Assert.NotNull(_module.MainComponent1List[2]);
        }
    }
}
