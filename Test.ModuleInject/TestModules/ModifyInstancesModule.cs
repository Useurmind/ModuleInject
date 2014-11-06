using ModuleInject;
using ModuleInject.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Fluent;

    class ModifyInstancesModule : InjectionModule<IEmptyModule, ModifyInstancesModule>, IEmptyModule
    {
        [PrivateComponent]
        public IMainComponent1 Component { get; private set; }

        [NonModuleProperty]
        public ISubComponent2 SubComponent { get; private set; }

        [PrivateComponent]
        public ISubModule Submodule { get; private set; }

        public ModifyInstancesModule()
        {
            SubComponent = new SubComponent2();
            RegisterPrivateComponent<ISubModule, Submodule>(x => x.Submodule);
        }

        public void RegisterComponentAndModifySubModulePropertyOnPropertyInjection()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component)
                .Inject(x => x.Submodule.Component1)
                .ModifiedBy(x => x.SubComponent2 = SubComponent)
                .IntoProperty(x => x.SubComponent1);
        }

        public void RegisterInstanceAndModifySubModulePropertyOnPropertyInjection()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.Component, new MainComponent1())
                .Inject(x => x.Submodule.Component1)
                .ModifiedBy(x => x.SubComponent2 = SubComponent)
                .IntoProperty(x => x.SubComponent1);
        }
    }
}
