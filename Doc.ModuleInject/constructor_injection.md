Constructor Injection
---------------------

In the tutorial you learned how property injection works. But there are other kinds of injection that ModuleInject provides. One is the constructor injection.

Constructor injection is part of the construction phase and rather simple. You can define a constructor on your component that takes some arguments which should be injected by your IoC container. Then you use the Construct method when registering the component to tell the module that it should not use the default constructor.

    public interface IConstructorComponent 
    {
        ISomeUndefinedComponent InjectedComponent { get; }
    }

    public class ConstructorComponent : IConstructorComponent
    {
        public ISomeUndefinedComponent InjectedComponent { get; private set; }

        public ConstructorComponent(ISomeUndefinedComponent injectedComponent) 
        {
            InjectedComponent = injectedComponent;
        }
    }

So here we defined a component with a dependency that is injected via the constructor of the component. Now that we have this component we can register it in a module like so:

    public IConstructorComponent Component 
    { 
        get 
        { 
            // use non-default constructor and inject module component into it
            return GetSingleInstance<IConstructorComponent>(module => 
            {
                return new ConstructorComponent(module.ComponentToInject)
            }); 
        }
    }

    private ISomeUndefinedComponent ComponentToInject 
    { 
        get 
        { 
            // use default constructor of type SomeUndefinedComponent
            return GetSingleInstance<SomeUndefinedComponent>(); 
        }
    }

In this example you first register a private component which should be injected. After that the public component is registered which requires the private component as a dependency. By calling the proper overload of `GetSingleInstance` the module is told to not use the default constructor but instead use the given lambda expression for construction. The input for the provided lambda is the module itself. You can use this reference to the module to feed the dependencies into the constructor of the component.

Note that every valid lambda can be used for defining the construction process. For example, you can also use an initializer list for property injection in the same lambda expression as the constructor.

### Complex form

The syntax shown above is a short form for the following syntax:

    public IConstructorComponent Component 
    { 
        get 
        { 
            // use non-default constructor and inject module component into it
            return GetSingleInstance<IConstructorComponent>(cc => 
            {
                cc.Construct(module => new ConstructorComponent(module.ComponentToInject));
            }); 
        }
    }

Using this form you also have access to other parts of the fluent API (e.g. Inject, Change, etc.). In this form the argument of the lambda expression given to the `GetSingleInstance` method is not the module. The argument `cc` is a so called `IConstructionContext` which is part of the fluent syntax. It offers the `Construct` method which offers several overloads. Here an overload is shown that takes a lambda expression taking the module itself as argument and returning the constructed instance.