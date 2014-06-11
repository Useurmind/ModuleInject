using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
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

        public IMainComponent1 AlsoRegisterForComponent { get; set; }

        [PrivateComponent]
        public IMainComponent1 PrivateComponentInjectedProperties { get; set; }
        [PrivateComponent]
        public IMainComponent2 PrivateComponent { get; set; }
        [PrivateComponent]
        public IMainComponent2 PrivateInstanceComponent { get; private set; }

        public ISubModule SubModule { get; set; }

        public PropertyModule()
        {
            FixedInstance = new MainComponent1();
            InjectedValue = 8;

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InstanceRegistrationComponent, FixedInstance)
                .PostInject(x => x.Component22).IntoProperty(x => x.MainComponent22)
                .PostInject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent23)
                .PostInject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .PostInitializeWith(x => x.Component2);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithPropertiesComponent)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.SubModule.Component1).IntoProperty(x => x.SubComponent1)
                .Inject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.PrivateInstanceComponent).IntoProperty(x => x.MainComponent23)
                .Inject(InjectedValue).IntoProperty(x => x.InjectedValue);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1Component)
                .InitializeWith(x => x.Component2)
                .AlsoRegisterFor(x => x.AlsoRegisterForComponent);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize1FromSubComponent)
                .InitializeWith(x => x.SubModule.Component1);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize2Component)
                .InitializeWith(x => x.Component2, x => x.SubModule.Component1);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithInitialize3Component)
                .InitializeWith(x => x.Component2, x => x.Component22, x => x.SubModule.Component1);

            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.InitWithInjectorComponent)
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

            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.Component2);
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.Component22);
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.PrivateComponent);
            RegisterPrivateComponent<IMainComponent2, MainComponent2>(x => x.PrivateInstanceComponent, new MainComponent2());

            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.PrivateComponentInjectedProperties)
                .Inject(x => x.PrivateComponent).IntoProperty(x => x.MainComponent2)
                .Inject(x => x.PrivateInstanceComponent).IntoProperty(x => x.MainComponent22)
                .Inject(x => x.Component2).IntoProperty(x => x.MainComponent23);

            RegisterPublicComponentFactory<IMainComponent2, MainComponent2>(x => x.CreateComponent2());
        }

        public IMainComponent2 CreateComponent2()
        {
            return CreateInstance(x => x.CreateComponent2());
        }

        #region Methods that should throw exceptions

        public void RegisterUnattributedPrivateProperty() {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.FixedInstance);
        }

        public void RegisterInterfacePropertyAsPrivate()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.InitWithPropertiesComponent);
        }

        public void RegisterUnattributedPrivatePropertyWithInstance()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.FixedInstance, new MainComponent1());
        }

        public void RegisterInterfacePropertyAsPrivateWithInstance()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.InitWithPropertiesComponent, new MainComponent1());
        }

        #endregion
    }
}
