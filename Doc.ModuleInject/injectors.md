Injectors
---------

Sometimes you want to define common injection patterns for a set of components of a module. In ModuleInject you can use injectors for this purpose. Per component you can use several injectors that can inject different sets of dependencies into your component. This composition approach easily enables you to inject dependencies that come from different base classes or the composition of the class from different other classes/interface (though in the case of class composition you should then think about injecting the classes that your class is composed of).

But now let us have a look at how an injector is implemented. So, assume that we have the following components:

    public interface ILog {
        void Write(string message);
    }

    public interface ILogging {
        ILog Log { get; }
    }

    public class DebugLog :  ILog {
        public Write(string message) { Debug.Write(message); }
    }

    public interface IInjectorComponent : ILogging {
    }

    public class InjectorComponent : IInjectorComponent {
        public ILog Log { get; private set; }
    }

In this example the ILog interface is used for logging messages. All classes that want to use logging implement the ILogging interface, which contains a property with the log component required for logging. Our component InjectorComponent implements the ILogging interface and needs to injected with a proper log component.

First, we will have a look at how to do this without injectors.

    public class InjectorModule : ... {
        public ILog LogComponent { get; private set; }
    
        public IInjectorComponent InjectorComponent { get; private set; }

        public InjectorModule() {
            RegisterPublicComponent<ILog, DebugLog>(x => x.LogComponent);
            RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent)
                .Injcet(x => x.LogComponent).IntoProperty(x.Log);
        }
    }

That is how to do it. Pretty straight forward. But now let's assume we have several of these InjectorComponents:

    public class InjectorModule : ... {
        public ILog LogComponent { get; private set; }
    
        public IInjectorComponent InjectorComponent1 { get; private set; }
        public IInjectorComponent InjectorComponent2 { get; private set; }
        public IInjectorComponent InjectorComponent3 { get; private set; }

        public InjectorModule() {
            RegisterPublicComponent<ILog, DebugLog>(x => x.LogComponent);
            RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent1)
                .Inject(x => x.LogComponent).IntoProperty(x.Log);
            RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent2)
                .Inject(x => x.LogComponent).IntoProperty(x.Log);
            RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent3)
                .Inject(x => x.LogComponent).IntoProperty(x.Log);
        }
    }

Looks pretty much the same for all three of them. So how do we solve this duplication of code to adhere to the DRY principle?

Well, let's try using an injector:

    public class DebugLogInjector :  
        ClassInjector<IInjectorComponent, InjectorComponent, IInjectorModule, InjectorModule> 
    {
        public DebugLogInjector() : base(context => {
            context.Inject(x => x.LogComponent).IntoProperty(x.Log);
        })
        {}
    }

You can then apply it by writing the following.

    public InjectorModule() {
        ...
        RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent1)
            .AddInjector(new DebugLogInjector());
        RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent2)
            .AddInjector(new DebugLogInjector());
        RegisterPublicComponent<IInjectorComponent, InjectorComponent>(x => x.InjectorComponent3)
            .AddInjector(new DebugLogInjector());
    }

What we just did is that we defined an injector. An injector encapsulates an injection pattern for a certain range of types. When defining such injection patterns inside an injector it works exactly the same as registering injections the usual way.

### Class injectors vs. interface injectors
One additional hint. We need different types of injectors for different scenarios. 

Just now we used a ClassInjector. This injector can only be used inside a special module on a special concrete type. You can easily see this as it requires the interface and type of both module and component for which the injection pattern is defined.

In comparison the InterfaceInjector requires the interface and type of the module as well as only the interface of the component. This allows you to define injectors for a wider range of types by targeting an interface with it.