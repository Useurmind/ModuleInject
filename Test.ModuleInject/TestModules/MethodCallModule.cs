using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IMethodCallModule : IInjectionModule
    {
        IMainComponent1 MainComponent1 { get; }

        ISubModule SubModule { get; set; }
    }

    public class MethodCallModule : InjectionModule<IMethodCallModule, MethodCallModule>, IMethodCallModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }

        [PrivateComponent]
        public IMainComponent2 MainComponent2 { get; private set; }

        public ISubModule SubModule { get; set; }

        public MethodCallModule()
        {
            SubModule = new Submodule();
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.MainComponent2);
        }

        public void RegisterPublicComponentWithPrivateComponentByMethodCall()
        {

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.Initialize(module.MainComponent2));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantValueByMethodCall()
        {

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.CallWithConstant(module.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithPrivateComponentAndConstantAndCastValueByMethodCall()
        {

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.CallWithConstant((IMainComponent2)module.MainComponent2, (int)5.0));
        }

        public void RegisterPublicComponentWithSubmoduleComponentByMethodCall()
        {

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.Initialize(module.SubModule.Component1));
        }

        public void RegisterPublicComponentWithPropertyOfThis()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.CallWithConstant(this.MainComponent2, 5));
        }

        public void RegisterPublicComponentWithStackVariable()
        {
            MainComponent2 mainComponent2 = new MainComponent2();

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.CallWithConstant(mainComponent2, 5));
        }

        public void RegisterPublicComponentWithInlineNew()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.MainComponent1)
                .CallMethod((comp, module) => comp.CallWithConstant(new MainComponent2(), 5));
        }

    }
}
