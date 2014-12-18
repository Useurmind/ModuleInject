using ModuleInject;
using ModuleInject.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Modules;

namespace Test.ModuleInject.TestModules
{
    interface IModifyInstancesModule
    {
        ISubModule Submodule { get; }
    }

    class ModifyInstancesModule : InjectionModule<IEmptyModule, ModifyInstancesModule>, IEmptyModule,IModifyInstancesModule
    {
        [PrivateComponent]
        public IMainComponent1 Component { get; private set; }

        [NonModuleProperty]
        public ISubComponent2 SubComponent { get; private set; }

        [PrivateComponent]
        public ISubModule Submodule { get; private set; }

        [PrivateComponent]
        public IMainComponent2 Component2 { get; private set; }

        [NonModuleProperty]
        public int IntValue { get; private set; }

        public ModifyInstancesModule()
        {
            IntValue = 134563456;
            SubComponent = new SubComponent2();
            RegisterPrivateComponent(x => x.Submodule).Construct<Submodule>();
            RegisterPrivateComponent(x => x.Component2).Construct<MainComponent2>();
        }

        //public void RegisterComponentAndModifySubModulePropertyOnPropertyInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
        //        .ModifyDependencyBy(x => x.Submodule.Component1, x => x.SubComponent2 = SubComponent)
        //        .Inject(x => x.Submodule.Component1)
        //        .IntoProperty(x => x.SubComponent1);
        //}

        //public void RegisterInstanceAndModifySubModulePropertyOnPropertyInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component, new MainComponent1())
        //        .ModifyDependencyBy(x => x.Submodule.Component1, x => x.SubComponent2 = SubComponent)
        //        .Inject(x => x.Submodule.Component1)
        //        .IntoProperty(x => x.SubComponent1);
        //}

        //public void RegisterComponentAndModifySubModulePropertyOnConstructorInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
        //        .ModifyDependencyBy(x => x.Component2, x => x.IntProperty = IntValue)
        //        .Construct(mod => new MainComponent1(mod.Component2));
        //}

        //public void RegisterComponentAndModifySubModulePropertyOnMethodInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
        //        .ModifyDependencyBy(x => x.Component2, x => x.IntProperty = IntValue)
        //        .Inject((x, mod) => x.Initialize(mod.Component2));
        //}

        //public void RegisterComponentAndModifySubModulePropertyOnStrongInterfaceInjectorInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
        //        .ModifyDependencyBy(x => x.Component2, x => x.IntProperty = IntValue)
        //        .AddInterfaceInjection(
        //            context =>
        //            {
        //                context.ModifyDependencyBy(x => x.Submodule.Component1, x => x.SubComponent2 = SubComponent)
        //                    .Inject(x => x.Submodule.Component1)
        //                    .IntoProperty(x => x.SubComponent1);
        //            });
        //}

        //public void RegisterComponentAndModifySubModulePropertyOnWeakInterfaceInjectorInjection()
        //{
        //    RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
        //        .ModifyDependencyBy(x => x.Component2, x => x.IntProperty = IntValue)
        //        .AddInterfaceInjection<IMainComponent1, MainComponent1, IEmptyModule, ModifyInstancesModule, IMainComponent1, IModifyInstancesModule>(
        //            context =>
        //            {
        //                context.ModifyDependencyBy(x => x.Submodule.Component1, x => x.SubComponent2 = SubComponent)
        //                    .Inject(x => x.Submodule.Component1)
        //                    .IntoProperty(x => x.SubComponent1);
        //            });
        //}
    }
}
