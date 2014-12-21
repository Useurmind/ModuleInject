using System.Linq;

using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using ModuleInject.Modules;
using ModuleInject.Modules.Fluent;

namespace Test.ModuleInject.Modules.TestModules
{
    public interface IModuleWithRegistrationErrors : IModule
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
            this.RegisterPublicComponent(x => x.PublicComponent.RecursiveComponent1).Construct<MainComponent1>();
        }

        public void RegisterPublicComponentInstanceProperty()
        {
            this.RegisterPublicComponent(x => x.PublicComponent.RecursiveComponent1)
                .Construct(new MainComponent1());
        }

        public void RegisterPrivateComponentsProperty()
        {
            this.RegisterPrivateComponent(x => x.PrivateComponent.RecursiveComponent1).Construct<MainComponent1>();
        }

        public void RegisterPrivateComponentsInstanceProperty()
        {
            this.RegisterPrivateComponent(x => x.PrivateComponent.RecursiveComponent1)
                .Construct(new MainComponent1());
        }

        public void RegisterPublicFactoryOfComponent()
        {
            this.RegisterPublicComponentFactory(x => x.PublicFactory().RecursiveFactory1()).Construct<MainComponent1>();
        }

        public void RegisterPrivateFactoryOfComponent()
        {
            this.RegisterPrivateComponentFactory(x => x.PrivateFactory().RecursiveFactory1()).Construct<MainComponent1>();
        }

        public void RegisterPublicComponentAsPrivateComponent()
        {
            this.RegisterPrivateComponent(x => x.PublicComponent).Construct<MainComponent1>();
        }

        public void RegisterPublicFactoryAsPrivateFactory()
        {
            this.RegisterPrivateComponentFactory(x => x.PublicFactory()).Construct<MainComponent1>();
        }

        public void RegisterPrivateComponentWithoutAttribute()
        {
            this.RegisterPrivateComponent(x => x.PrivateComponentWithoutAttribute).Construct<MainComponent1>();
        }

        public void RegisterPrivateFactoryWithoutAttribute()
        {
            this.RegisterPrivateComponentFactory(x => x.PrivateFactoryWithoutAttribute()).Construct<MainComponent1>();
        }

        public void RegisterWithFancyExpression1()
        {
            this.RegisterPublicComponent(x => new ModuleWithRegistrationErrors().PublicComponent)
                .Construct<MainComponent1>();
        }

        public void RegisterWithCastToNonImplementedInterface()
        {
            this.RegisterPublicComponent(x => x.PublicComponent2)
                .Construct<MainComponent2>()
                .Inject((c, m) => c.Initialize((IMainComponent2SubInterface)m.PublicComponent));
        }
    }
}
