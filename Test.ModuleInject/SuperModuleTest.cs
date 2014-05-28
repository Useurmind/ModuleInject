using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class SuperModuleTest
    {
        private TestSuperModule _superModule;

        [SetUp]
        public void Init()
        {
            _superModule = new TestSuperModule();
        }

        [TestCase]
        public void Resolve_SubModuleDistributed()
        {
            _superModule.Resolve();

            Assert.IsNotNull(_superModule.MainModule);
            Assert.IsNotNull(_superModule.SubModule);
            Assert.AreSame(_superModule.SubModule, _superModule.MainModule.SubModule);
        }
    }
}
