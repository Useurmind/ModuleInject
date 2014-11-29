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
        [PrivateComponent]
        private IMainComponent1 PrivateComponent { get; set; }
        private IMainComponent1 PrivateComponentWithoutAttribute { get; set; }
        private IMainComponent1 PrivateFactoryWithoutAttribute() { return null; }

        public IMainComponent1 PublicFactory() { return null; }
        private IMainComponent1 PrivateFactory() { return null; }

        public void RegisterPublicComponentsProperty()
        {
            RegisterPublicComponent(x => x.PublicComponent.RecursiveComponent1).Construct<MainComponent1>();
        }

        public void RegisterPublicComponentInstanceProperty()
        {
            RegisterPublicComponent(x => x.PublicComponent.RecursiveComponent1)
                .Construct(new MainComponent1());
        }

        public void RegisterPrivateComponentsProperty()
        {
            RegisterPrivateComponent(x => x.PrivateComponent.RecursiveComponent1).Construct<MainComponent1>();
        }

        public void RegisterPrivateComponentsInstanceProperty()
        {
            RegisterPrivateComponent(x => x.PrivateComponent.RecursiveComponent1)
                .Construct(new MainComponent1());
        }

        public void RegisterPublicFactoryOfComponent()
        {
            RegisterPublicComponentFactory(x => x.PublicFactory().RecursiveFactory1()).Construct<MainComponent1>();
        }

        public void RegisterPrivateFactoryOfComponent()
        {
            RegisterPrivateComponentFactory(x => x.PrivateFactory().RecursiveFactory1()).Construct<MainComponent1>();
        }

        public void RegisterPublicComponentAsPrivateComponent()
        {
            RegisterPrivateComponent(x => x.PublicComponent).Construct<MainComponent1>();
        }

        public void RegisterPublicFactoryAsPrivateFactory()
        {
            RegisterPrivateComponentFactory(x => x.PublicFactory()).Construct<MainComponent1>();
        }

        public void RegisterPrivateComponentWithoutAttribute()
        {
            RegisterPrivateComponent(x => x.PrivateComponentWithoutAttribute).Construct<MainComponent1>();
        }

        public void RegisterPrivateFactoryWithoutAttribute()
        {
            RegisterPrivateComponentFactory(x => x.PrivateFactoryWithoutAttribute()).Construct<MainComponent1>();
        }

        public void RegisterWithFancyExpression1()
        {
            RegisterPublicComponent(x => new ModuleWithRegistrationErrors().PublicComponent)
                .Construct<MainComponent1>();
        }

        public void RegisterWithCastToNonImplementedInterface()
        {
            RegisterPublicComponent(x => x.PublicComponent2)
                .Construct<MainComponent2>()
                .InitializeWith(x => (IMainComponent2SubInterface)x.PublicComponent);
        }
    }
}
