using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    internal class CustomActionModule : InjectionModule<IEmptyModule, CustomActionModule>, IEmptyModule
    {
        [PrivateComponent]
        public IMainComponent1 MainComponent1 { get; private set; }

        [NonModuleProperty]
        public IMainComponent2 MainComponent2 { get; private set; }

        public CustomActionModule()
        {
            MainComponent2 = new MainComponent2();
        }

        public void RegisterWithClosure_ConstructFromType()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .AddCustomAction(c => c.MainComponent2 = this.MainComponent2);
        }

        public void RegisterInInterfaceInjector_ConstructFromInstance()
        {
            RegisterPrivateComponent(x => x.MainComponent1)
                .Construct(new MainComponent1())
                .AddInterfaceInjection<IMainComponent1, MainComponent1, IEmptyModule, CustomActionModule, IMainComponent1, IEmptyModule>(
                (context) =>
                {
                    context.AddCustomAction(c => c.MainComponent2 = this.MainComponent2);
                });
        }
    }
}
