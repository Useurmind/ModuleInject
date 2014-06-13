using ModuleInject;
using ModuleInject.Fluent;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ModuleInject.TestModules
{
    public interface IModuleWithRegistrationErrors : IInjectionModule
    {
        IMainComponent1 PublicComponent { get; }

        IMainComponent2 PublicComponent2 { get; }

        IMainComponent1 PublicFactory();
    }

    public class ModuleWithRegistrationErrors : InjectionModule<IModuleWithRegistrationErrors, ModuleWithRegistrationErrors>, IModuleWithRegistrationErrors
    {
        // private factor without attribute
        public IMainComponent1 PublicComponent { get; private set; }
        public IMainComponent2 PublicComponent2 { get; private set; }
        public IMainComponent2 Component22 { get; private set; }
        [PrivateComponent]
        private IMainComponent1 PrivateComponent { get; set; }
        private IMainComponent1 PrivateComponentWithoutAttribute { get; set; }
        private IMainComponent1 PrivateFactoryWithoutAttribute() { return null; }

        private ISubComponent1 SubComponent { get; set; }

        public IMainComponent1 PublicFactory() { return null; }
        private IMainComponent1 PrivateFactory() { return null; }

        public void RegisterPublicComponentsProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicComponent.RecursiveComponent1);
        }

        public void RegisterPublicComponentInstanceProperty()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => x.PublicComponent.RecursiveComponent1, new MainComponent1());
        }

        public void RegisterPrivateComponentsProperty()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.PrivateComponent.RecursiveComponent1);
        }

        public void RegisterPrivateComponentsInstanceProperty()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.PrivateComponent.RecursiveComponent1, new MainComponent1());
        }

        public void RegisterPublicFactoryOfComponent()
        {
            RegisterPublicComponentFactory<IMainComponent1, MainComponent1>(x => x.PublicFactory().RecursiveFactory1());
        }

        public void RegisterPrivateFactoryOfComponent()
        {
            RegisterPrivateComponentFactory<IMainComponent1, MainComponent1>(x => x.PrivateFactory().RecursiveFactory1());
        }

        public void RegisterPublicComponentAsPrivateComponent()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.PublicComponent);
        }

        public void RegisterPublicFactoryAsPrivateFactory()
        {
            RegisterPrivateComponentFactory<IMainComponent1, MainComponent1>(x => x.PublicFactory());
        }

        public void RegisterPrivateComponentWithoutAttribute()
        {
            RegisterPrivateComponent<IMainComponent1, MainComponent1>(x => x.PrivateComponentWithoutAttribute);
        }

        public void RegisterPrivateFactoryWithoutAttribute()
        {
            RegisterPrivateComponentFactory<IMainComponent1, MainComponent1>(x => x.PrivateFactoryWithoutAttribute());
        }

        public void RegisterWithFancyExpression1()
        {
            RegisterPublicComponent<IMainComponent1, MainComponent1>(x => new ModuleWithRegistrationErrors().PublicComponent);
        }

        public void RegisterWithCastToNonImplementedInterface()
        {
            RegisterPublicComponent<IMainComponent2, MainComponent2>(x => x.PublicComponent2)
                .InitializeWith(x => (IMainComponent2SubInterface)x.PublicComponent);
        }
    }
}
