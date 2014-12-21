using System.Linq;

using ModuleInject.Common.Exceptions;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
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
