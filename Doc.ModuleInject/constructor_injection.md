Constructor Injection
---------------------

In the tutorial you learned how property injection works. But there are other kinds of injection that ModuleInject provides. One is called constructor injection.

Constructor injection is rather simple. You can define a constructor on your component that takes some arguments which should be injected by your IoC container. Then you use the CallConstructor method when registering the component to tell the module that it should not use the default constructor.

    public interface IConstructorComponent {
        ISomeUndefinedComponent InjectedComponent { get; }
    }

    public class ConstructorComponent : IConstructorComponent
    {
        public ISomeUndefinedComponent InjectedComponent { get; private set; }

        public ConstructorComponent(ISomeUndefinedComponent injectedComponent) {
            InjectedComponent = injectedComponent;
        }
    }

So here we defined a component with a dependency that is injected via the constructor of the component. Now that we have this component we can register it in a module like so:

    public IConstructorComponent Component { get; set; }

    [PrivateComponent]
    private ISomeUndefinedComponent ComponentToInject { get; set; }

    public ModuleWithConstructorInjectionComponent() {
        RegisterPrivateComponent<ISomeUndefinedComponent, SomeUndefinedComponent>(x => x.ComponentToInject);

        RegisterPublicComponent<IConstructorComponent, ConstructorComponent>(x => x.Component)
            .CallConstructor(module => new ConstructorComponent(module.ComponentToInject));
    }

In this example you first register a private component which should be injected. After that the public component is registered which requires the private component as a dependency. By calling CallConstructor the module is told to not use the default constructor but instead look for the given constructor. The arguments of the constructor are given as member expressions describing other components of the module (which is the only parameter to the lambda expression).