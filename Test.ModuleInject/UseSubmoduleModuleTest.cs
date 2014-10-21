using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject
{
    using NUnit.Framework;

    using Test.ModuleInject.TestModules;

    [TestFixture]
    class UseSubmoduleModuleTest
    {
        private UseSubmoduleModule _module;

        [SetUp]
        public void Setup()
        {
            _module = new UseSubmoduleModule();
        }

        [TestCase]
        public void Resolve_InjectingSubmoduleProperty_PropertySetCorrectly()
        {
            _module.RegisterMainComponent_Injecting_SubmoduleProperty();
            _module.Resolve();

            Assert.IsNotNull(_module.SubModule.Component1);
            Assert.AreSame(_module.SubModule.Component1, _module.MainComponent.SubComponent1);
        }
    }
}
