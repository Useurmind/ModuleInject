using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.ModuleInject.TestModules;
using NUnit.Framework;

namespace Test.ModuleInject
{
    [TestFixture]
    public class ModuleWithNonModulePropertyInjectTest
    {
        private class ModuleWithNonModulePropertyInject : InjectionModule<IEmptyModule, ModuleWithNonModulePropertyInject>, IEmptyModule
        {
            [PrivateComponent]
            public IMainComponent1 MainComponent1 { get; private set; }

            [NonModuleProperty]
            public IMainComponent2 MainComponent2NonModule { get; private set; }

            public ModuleWithNonModulePropertyInject()
            {
                MainComponent2NonModule = new MainComponent2();

                RegisterPrivateComponent(x => x.MainComponent1).Construct<MainComponent1>()
                    .Inject(x => x.MainComponent2NonModule).IntoProperty(x => x.MainComponent2);
            }
        }

        [Test]
        public void Resolve_NonModulePropertyCorrectlyResolved()
        {
            var module = new ModuleWithNonModulePropertyInject();

            module.Resolve();

            Assert.IsNotNull(module.MainComponent1);
            Assert.IsNotNull(module.MainComponent2NonModule);
            Assert.AreSame(module.MainComponent2NonModule, module.MainComponent1.MainComponent2);
        }
    }
}
