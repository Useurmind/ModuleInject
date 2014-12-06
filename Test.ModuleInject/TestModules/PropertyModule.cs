﻿using ModuleInject;
using ModuleInject.Fluent;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    using global::ModuleInject.Decoration;
    using global::ModuleInject.Interfaces;

    public interface IPropertyModule : IInjectionModule
    {
        IMainComponent1 InstanceRegistrationComponent { get; }
        IMainComponent1 InitWithPropertiesComponent { get; }
        IMainComponent1 InitWithInitialize1Component { get; }
        IMainComponent1 InitWithInitialize1FromSubComponent { get; }
        IMainComponent1 InitWithInitialize2Component { get; }
        IMainComponent1 InitWithInitialize3Component { get; }
        IMainComponent1 InitWithInjectorComponent { get; }
        IMainComponent2 Component2 { get; }
        IMainComponent2 Component22 { get; }

        IMainComponent1 AlsoRegisterForComponent { get; }

        ISubModule SubModule { get; set; }

        IMainComponent2 CreateComponent2();
    }

    internal class PropertyModule : InjectionModule<IPropertyModule, PropertyModule>, IPropertyModule
    {
        [NonModuleProperty]
        public MainComponent1 FixedInstance { get; private set; }
        [NonModuleProperty]
        public int InjectedValue { get; private set; }

        public IMainComponent1 InstanceRegistrationComponent { get; private set; }
        public IMainComponent1 InitWithPropertiesComponent { get; private set; }
        public IMainComponent1 InitWithInitialize1Component { get; private set; }
        public IMainComponent1 InitWithInitialize1FromSubComponent { get; private set; }
        public IMainComponent1 InitWithInitialize2Component { get; private set; }
        public IMainComponent1 InitWithInitialize3Component { get; private set; }
        public IMainComponent1 InitWithInjectorComponent { get; private set; }
        public IMainComponent2 Component2 { get; private set; }
        public IMainComponent2 Component22 { get; private set; }

        public IMainComponent1 AlsoRegisterForComponent { get; set; }

        [PrivateComponent]
        public IMainComponent1 PrivateComponentInjectedProperties { get; set; }
        [PrivateComponent]
        public IMainComponent2 PrivateComponent { get; set; }
        [PrivateComponent]
        public IMainComponent2 PrivateInstanceComponent { get; private set; }

        [NonModuleProperty]
        [RegistryComponent]
        public ISubModule SubModule { get; set; }

        public PropertyModule()
        {
            FixedInstance = new MainComponent1();
            InjectedValue = 8;

            RegisterPublicComponent(x => x.InstanceRegistrationComponent)
                .Construct(FixedInstance)
                .Inject(x => x.Component22).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent23)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .Inject((c, m) => c.Initialize(m.Component2));

            RegisterPublicComponent(x => x.InitWithPropertiesComponent)
                .Construct<MainComponent1>()
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .Inject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.PrivateInstanceComponent).IntoProperty(x => x.MainComponent23)
                .Inject(x => x.Component2).IntoProperty(x => x.ComponentViaSubinterface)
                .Inject(InjectedValue).IntoProperty(x => x.InjectedValue);

            RegisterPublicComponent(x => x.InitWithInitialize1Component)
                .Construct<MainComponent1>()
                .Inject((c, m) => c.Initialize(m.Component2))
                .AlsoRegisterFor(x => x.AlsoRegisterForComponent);

            RegisterPublicComponent(x => x.InitWithInitialize1FromSubComponent)
                .Construct<MainComponent1>()
                .Inject((c, m) => c.Initialize(m.SubModule.Component1));

            RegisterPublicComponent(x => x.InitWithInitialize2Component)
                .Construct<MainComponent1>()
                .Inject((c, m) => c.Initialize(m.Component2, m.SubModule.Component1));

            RegisterPublicComponent(x => x.InitWithInitialize3Component)
                .Construct<MainComponent1>()
                .Inject((c, m) => c.Initialize(m.Component2, m.Component22, m.SubModule.Component1));

            RegisterPublicComponent(x => x.InitWithInjectorComponent)
                .Construct<MainComponent1>()
                .AddInjector(new ClassInjector<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(context =>
                {
                    context.Inject(InjectedValue).IntoProperty(x => x.InjectedValue);
                }))
                .AddInjector(new ClassInjector<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(context =>
                {
                    context.Inject(x => x.Component2).IntoProperty(x => x.MainComponent2);
                    context.Inject(x => x.Component22).IntoProperty(x => x.MainComponent22);
                }))
                .AddInjector(new ClassInjector<IMainComponent1, MainComponent1, IPropertyModule, PropertyModule>(context =>
                {
                    context.Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1);
                }));

            RegisterPublicComponent(x => x.Component2).Construct<MainComponent2>();
            RegisterPublicComponent(x => x.Component22)
                .Construct<MainComponent2>()
                .Inject((c, m) => c.Initialize((IMainComponent2SubInterface)m.Component2));
            RegisterPrivateComponent(x => x.PrivateComponent)
            .Construct<MainComponent2>();
            RegisterPrivateComponent(x => x.PrivateInstanceComponent)
                .Construct(new MainComponent2());

            RegisterPrivateComponent(x => x.PrivateComponentInjectedProperties)
                .Construct<MainComponent1>()
                .Inject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.PrivateInstanceComponent).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent23);

            RegisterPublicComponentFactory(x => x.CreateComponent2()).Construct<MainComponent2>();
        }

        public IMainComponent2 CreateComponent2()
        {
            return CreateInstance(x => x.CreateComponent2());
        }

        #region Methods that should throw exceptions

        public void RegisterUnattributedPrivateProperty()
        {
            RegisterPrivateComponent(x => x.FixedInstance).Construct<MainComponent1>();
        }

        public void RegisterInterfacePropertyAsPrivate()
        {
            RegisterPrivateComponent(x => x.InitWithPropertiesComponent).Construct<MainComponent1>();
        }

        public void RegisterUnattributedPrivatePropertyWithInstance()
        {
            RegisterPrivateComponent(x => x.FixedInstance).Construct(new MainComponent1());
        }

        public void RegisterInterfacePropertyAsPrivateWithInstance()
        {
            RegisterPrivateComponent(x => x.InitWithPropertiesComponent).Construct(new MainComponent1());
        }

        #endregion
    }
}
