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
        public MainComponent1 FixedInstance { get; private set; }
        public int InjectedValue { get; private set; }

        public IMainComponent1 InstanceRegistrationComponent { get; set; }
        public IMainComponent1 InitWithPropertiesComponent { get; set; }
        public IMainComponent1 InitWithInitialize1Component { get; set; }
        public IMainComponent1 InitWithInitialize1FromSubComponent { get; set; }
        public IMainComponent1 InitWithInitialize2Component { get; set; }
        public IMainComponent1 InitWithInitialize3Component { get; set; }
        public IMainComponent1 InitWithInjectorComponent { get; set; }
        public IMainComponent2 Component2 { get; set; }
        public IMainComponent2 Component22 { get; set; }

        public ISubModule SubModule { get; set; }

        public MainModule()
        {
            FixedInstance = new MainComponent1();
            InjectedValue = 8;

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InstanceRegistrationComponent, FixedInstance)
                .PostInject(x => x.Component22).IntoProperty(x => x.MainComponent22)
                .PostInject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .PostInitializeWith(x => x.Component2);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithPropertiesComponent)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .Inject(InjectedValue).IntoProperty(x => x.InjectedValue);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1Component)
                .InitializeWith(x => x.Component2);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1FromSubComponent)
                .InitializeWith(x => x.SubModule.Component1);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize2Component)
                .InitializeWith(x => x.Component2, x => x.SubModule.Component1);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize3Component)
                .InitializeWith(x => x.Component2, x => x.Component22, x => x.SubModule.Component1);

            RegisterComponent<IMainComponent1, MainComponent1>(x => x.InitWithInjectorComponent)
                .AddInjector(new Injector<IMainComponent1, MainComponent1, IMainModule>(context =>
                {
                    context.Inject(InjectedValue).IntoProperty(x => x.InjectedValue);
                }))
                .AddInjector(new Injector<IMainComponent1, MainComponent1, IMainModule>(context =>
                {
                    context.Inject(x => x.Component2).IntoProperty(x => x.MainComponent2);
                    context.Inject(x => x.Component22).IntoProperty(x => x.MainComponent22);
                }))
                .AddInjector(new Injector<IMainComponent1, MainComponent1, IMainModule>(context =>
                {
                    context.Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1);
                }));

            RegisterComponent<IMainComponent2, MainComponent2>(x => x.Component2);
            RegisterComponent<IMainComponent2, MainComponent2>(x => x.Component22);
        }
    }
}
