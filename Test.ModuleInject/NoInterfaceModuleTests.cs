using ModuleInject.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;

namespace Test.ModuleInject
{
    [TestFixture]
    public class NoInterfaceModuleTests
    {
        [TestCase]
        [ExpectedException(typeof(ModuleInjectException))]
        public void NoInterfaceModule_Constructed_ExceptionThrown()
        {
            var module = new NoInterfaceModule();
        }
    }
}
