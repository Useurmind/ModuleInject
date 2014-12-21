Constructor Injection
---------------------

In the tutorial you learned how property injection works. But there are other kinds of injection that ModuleInject provides. One is the constructor injection.

Constructor injection is part of the construction phase and rather simple. You can define a constructor on your component that takes some arguments which should be injected by your IoC container. Then you use the Construct method when registering the component to tell the module that it should not use the default constructor.

```csharp
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
```

So here we defined a component with a dependency that is injected via the constructor of the component. Now that we have this component we can register it in a module like so:

    public IConstructorComponent Component { get; set; }

    [PrivateComponent]
    private ISomeUndefinedComponent ComponentToInject { get; set; }

    public ModuleWithConstructorInjectionComponent() {
        // use default constructor of type SomeUndefinedComponent
        RegisterPrivateComponent(x => x.ComponentToInject)
            .Construct<SomeUndefinedComponent>();

        // use non-default constructor and inject module component into it
        RegisterPublicComponent(x => x.Component)
            .Construct(module => new ConstructorComponent(module.ComponentToInject));
    }

In this example you first register a private component which should be injected. After that the public component is registered which requires the private component as a dependency. By calling the proper overload of `Construct` the module is told to not use the default constructor but instead use the given lambda expression for construction. The arguments of the constructor are given as member expressions describing other components of the module (which is the only parameter to the lambda expression).

Note that every valid lambda EXPRESSION can be used for defining the construction process. For example, you can also use an initializer list for property injection in the same lambda expression as the constructor.