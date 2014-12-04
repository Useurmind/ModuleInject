using ModuleInject;
using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IFunctionCallModule : IInjectionModule
    {
        IMainComponent1 MainComponent1 { get; }
        IMainComponent1 MainInstance1 { get; }

        IList<IMainComponent1> MainComponent1List { get; }
    }

    public class AfterResolveModule : InjectionModule<IFunctionCallModule, AfterResolveModule>, IFunctionCallModule
    {
        public IMainComponent1 MainComponent1 { get; private set; }
        public IMainComponent1 MainInstance1 { get; private set; }
        public IList<IMainComponent1> MainComponent1List { get; private set; }

        [PrivateComponent]
        internal IMainComponent2 MainComponent2 { get; private set; }

        [PrivateFactory]
        internal IMainComponent1 CreateMainComponent1()
        {
            return base.CreateInstance(x => x.CreateMainComponent1());
        }

        //[PrivateFactory]
        //internal IMainComponent2 CreateMainComponent2()
        //{
        //    return base.CreateInstance(x => x.CreateMainComponent2());
        //}

        public void RegisterComponentsByInitializeWithOtherComponent()
        {
            RegisterPrivateComponentFactory(x => x.CreateMainComponent1()).Construct<MainComponent1>();

            RegisterPrivateComponent(x => x.MainComponent2).Construct<MainComponent2>();

            RegisterPublicComponent(x => x.MainInstance1)
                .Construct(new MainComponent1())
                .Inject((comp, module) => comp.Initialize(module.MainComponent2));

            RegisterPublicComponent(x => x.MainComponent1)
                .Construct<MainComponent1>()
                .Inject((comp, module) => comp.Initialize(module.MainComponent2));

            RegisterPublicComponent(x => x.MainComponent1List)
                .Construct<List<IMainComponent1>>()
                .Inject((comp, module) => comp.Add(module.MainComponent1))
                .Inject((comp, module) => comp.Add(module.CreateMainComponent1()))
                .Inject((comp, module) => comp.Add(module.CreateMainComponent1()));
        }
    }
}
