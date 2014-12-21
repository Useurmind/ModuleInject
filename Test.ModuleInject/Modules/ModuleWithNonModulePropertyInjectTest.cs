using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

using NUnit.Framework;

using Test.ModuleInject.Modules.TestModules;

namespace Test.ModuleInject.Modules
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
                this.MainComponent2NonModule = new MainComponent2();

                this.RegisterPrivateComponent(x => x.MainComponent1).Construct<MainComponent1>()
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
