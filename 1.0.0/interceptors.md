Interceptors
------------

Interceptors are a very strong pattern that allows you to apply AOP like constructs. You can intercept method calls of 
certain classes to do things that are not related to the core functionality of the class. They are similar to decorators 
but more flexible and easier to apply.

They can be used for anything but will be exceptionally useful for cross cutting concerns like logging.

In ModuleInject you can use the interceptors that are part of the Unity container. Unity provides so called behaviours
that you can add to types. ModuleInject provides you with the possibility to add such behaviours to components
registered in your modules.

    public class LoggingBehaviour : IInterceptionBehaviour 
    {
        //...
    }

    //...

    public void RegisterBehaviour()
    {
        RegisterPublicComponent(x => x.Component)
            .Construct<Component>()
            .AddBehaviour<LoggingBehaviour>();
    }

This registers your logging behaviour with a component. The behaviours work exactly as in the Unity container. The only
limitation is that you can only use interface interceptors in ModuleInject.