No Container Injection
----------------------

So far you have seen modules that work similar to a small DI container. And you also saw that plain code is the simplest and most efficient code to perform component setup through dependency injection. Plain code only becomes an issue if you do many setups in one stroke of DI code where the setups are heavily intertwined.

###The `Module` class

In many cases you could create modules that are not that complicated. In such cases one could argue that it is overkill to use such a complicated module with integrated container as provided by the `InjectionModule` class.

For example, assume the case where you want to create a model-view-controller triple and inject a business service into the controller.

    var model = new MyModel();
    var view = new MyView();
    var controller = new MyController(model, view, service); 

Easy enough I would say. The model and view are created and injected into the controller. The service was already created elsewhere and is injected.

With the `InjectionModule` class you would arguably have some overhead when implementing this logic. All the used LINQ expressions must be compiled and analyzed to guarantee the correct order of execution.

Specifically for such cases, the modularity features of ModuleInject are separated from the `InjectionModule` implementation. You can create a module without the features of the fluent registration language and implement the DI code completely by hand.

For the example above it could look like this:

    public interface IMyMvcModule : IModule 
    {
        // Only the controller must be made public in this example
        // because it is used by other controllers
        MyController Controller { get; }
    }

    public class MyMvcModule : Module 
    {
        public MyController Controller { get; set; }

        // there is a module for the business layer which holds the service
        // it is resolved from the registry
        [RegistryComponent]
        public IBllModule BllModule { get; set; }

        // called when after the components/modules from the registry
        // are resolved. The BllModule is now available and resolved.
        protected override void OnRegistryResolved(IRegistry usedRegistry)
        {
            // model and view can simply be injected they are only used by 
            // this controller
            var model = new MyModel();
            var view = new MyView();
            Controller = new MyController(model, view, BllModule.Service);         
        }
    }

So we just define a public interface as before. But now the module looks a little more straight forward. The injection code is plain C# code. 

The things the module does for you is resolving modules by using the registry. Here we assume the registry is set by a parent module. The `IBllModule` is resolved and then used to inject the service into the controller.

The method `OnRegistryResolved` is called during the resolution process of this module as soon as all properties with the `RegistryComponentAttribute` are resolved by the base class.

###Combining `Module` class with a DI container

Using the `Module` base class you could even apply a DI container from a completely different library.

    protected override void OnRegistryResolved(IRegistry usedRegistry)
    {
        var container = new UnityContainer();

        container.RegisterInstance<IBllService>(BllModule.Service);
        container.RegisterType<MyModel>();
        container.RegisterType<MyView>();
        container.RegisterType<MyController>();

        Controller = container.Resolve<MyController>();
    }

Crazy stuff and I don't know if you would want to use it that way. But it is possible. Everything is with plain C# code. :)

###Summary

The `Module` class allows you to write your DI code in any form you want without sacrificing the big advantage of modularity that ModuleInject provides to you.