using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    internal class CustomActionModule : InjectionModule<IEmptyModule, CustomActionModule>, IEmptyModule
    {
        [PrivateComponent]
        public IMainComponent1 MainComponent1 { get; private set; }

        [NonModuleProperty]
        public IMainComponent2 MainComponent2 { get; private set; }

        public CustomActionModule()
        {
            this.MainComponent2 = new MainComponent2();
        }

        public void RegisterWithClosure_ConstructFromType()
        {
            this.RegisterPrivateComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .AddCustomAction(c => c.MainComponent2 = this.MainComponent2);
        }

        public void RegisterInInterfaceInjector_ConstructFromInstance()
        {
            this.RegisterPrivateComponent(x => x.MainComponent1)
                .Construct(new MainComponent1())
                .AddInterfaceInjection<IMainComponent1, MainComponent1, IEmptyModule, CustomActionModule, IMainComponent1, IEmptyModule>(
                (context) =>
                {
                    context.AddCustomAction(c => c.MainComponent2 = this.MainComponent2);
                });
        }
    }
}
