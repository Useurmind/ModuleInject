using System.Linq;

using ModuleInject.Injection;
using ModuleInject.Interfaces;
using ModuleInject.Modularity;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IMethodCallModule : IModule
    {
        IMainComponent1 MainComponent1 { get; }

        ISubModule SubModule { get; set; }
    }

    public class MethodCallModule : InjectionModule<MethodCallModule>, IMethodCallModule
    {
        public IMainComponent1 MainComponent1 { get { return Get<IMainComponent1>(); } }
        
        public IMainComponent2 MainComponent2 { get { return Get<IMainComponent2>(); } }
        
        [FromRegistry]
        public ISubModule SubModule { get; set; }

        public MethodCallModule()
        {
            this.SubModule = new Submodule();
            SingleInstance(m => m.MainComponent2).Construct<MainComponent2>();
        }

        public void RegisterPublicComponentWithPrivateComponentByMethodCall()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.Initialize(module.MainComponent2));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.CallWithConstant(module.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.CallWithConstant((IMainComponent2)module.MainComponent2, (int)5.0));
        }

        public void RegisterPublicComponentWithSubmoduleComponentByMethodCall()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.Initialize(module.SubModule.Component1));
        }

        public void RegisterPublicComponentWithPropertyOfThis()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.CallWithConstant(this.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithStackVariable()
        {
            MainComponent2 mainComponent2 = new MainComponent2();

            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.CallWithConstant(mainComponent2, 5));
        }

        public void RegisterPublicComponentWithInlineNew()
        {
            SingleInstance(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((module, comp) => comp.CallWithConstant(new MainComponent2(), 5));
        }
    }
}
