using ModuleInject;
using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal class MainModule : InjectionModule<IMainModule, MainModule>, IMainModule
    {
        public IMainComponent1 InstanceRegistrationComponent { get; set; }
        public IMainComponent1 InitWithPropertiesComponent { get; set; }
        public IMainComponent1 InitWithInitialize1Component { get; set; }
        public IMainComponent1 InitWithInitialize1FromSubComponent { get; set; }
        public IMainComponent1 InitWithInitialize2Component { get; set; }
        public IMainComponent1 InitWithInitialize3Component { get; set; }
        public IMainComponent2 Component2 { get; set; }
        public IMainComponent2 Component22 { get; set; }

        public ISubModule SubModule { get; set; }

        public MainModule()
        {
            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InstanceRegistrationComponent, new MainComponent1())
                .Inject(x => x.Component22).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .InitializeWith(x => x.Component2);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithPropertiesComponent)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .Inject(5).IntoProperty(x => x.InjectedValue);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1Component)
                .InitializeWith(x => x.Component2);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1FromSubComponent)
                .InitializeWith(x => x.SubModule.Component1);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize2Component)
                .InitializeWith(x => x.Component2, x => x.SubModule.Component1);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize3Component)
                .InitializeWith(x => x.Component2, x => x.Component22, x => x.SubModule.Component1);

            RegisterComponent<IMainComponent2, MainComponent2>(x => x.Component2);
            RegisterComponent<IMainComponent2, MainComponent2>(x => x.Component22);

            SubModule = new Submodule();
        }
    }
}
