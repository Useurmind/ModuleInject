using ModuleInject;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IMethodCallModule : IModule
    {
        IMainComponent1 MainComponent1 { get; }

        ISubModule SubModule { get; set; }
    }

    public class MethodCallModule : InjectionModule<IMethodCallModule, MethodCallModule>, IMethodCallModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set; }

        [NonModuleProperty]
        public ISubModule SubModule { get; set; }

        public MethodCallModule()
        {
            SubModule = new Submodule();
            RegisterPrivateComponent(x => x.MainComponent2).Construct<MainComponent2>();
        }

        public void RegisterPublicComponentWithPrivateComponentByMethodCall()
        {

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.Initialize(module.MainComponent2));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall()
        {

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.CallWithConstant(module.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall()
        {

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.CallWithConstant((IMainComponent2)module.MainComponent2, (int)5.0));
        }

        public void RegisterPublicComponentWithSubmoduleComponentByMethodCall()
        {

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.Initialize(module.SubModule.Component1));
        }

        public void RegisterPublicComponentWithPropertyOfThis()
        {
            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.CallWithConstant(this.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithStackVariable()
        {
            MainComponent2 mainComponent2 = new MainComponent2();

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.CallWithConstant(mainComponent2, 5));
        }

        public void RegisterPublicComponentWithInlineNew()
        {
            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.CallWithConstant(new MainComponent2(), 5));
        }
    }
}
