using ModuleInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal class MainModule : InjectionModule<IMainModule, MainModule>, IMainModule
    {
        public IMainComponent1 Component1 { get; set; }
        public IMainComponent1 SecondComponent1 { get; set; }
        public IMainComponent2 Component2 { get; set; }

        public ISubModule SubModule { get; set; }

        public MainModule()
        {
            RegisterComponent<IMainComponent1, MainComponent1>(x => x.Component1)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1);
            RegisterComponent<IMainComponent1, MainComponent1>(x => x.SecondComponent1);
            RegisterComponent<IMainComponent2, MainComponent2>(x => x.Component2);

            SubModule = new Submodule();
        }
    }
}
