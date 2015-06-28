Injectors
---------

Sometimes you want to define common injection patterns for a set of components of a module. In ModuleInject you can use injectors for this purpose. Per component you can use several injectors that can inject different sets of dependencies into your component. This composition approach easily enables you to inject dependencies that come from different base classes or the composition of the class from different other classes/interface (though in the case of class composition you should then think about injecting the classes that your class is composed of).

But now let us have a look at how an injector is implemented. So, assume that we have the following components:

    public interface ILog 
    {
        void Write(string message);
    }

    public interface ILogging 
    {
        ILog Log { get; }
    }

    public class DebugLog :  ILog 
    {
        public Write(string message) { Debug.Write(message); }
    }

    public interface IInjectorComponent : ILogging 
    {
    }

    public class InjectorComponent : IInjectorComponent 
    {
        public ILog Log { get; private set; }
    }

In this example the `ILog` interface is used for logging messages. All classes that want to use logging implement the `ILogging` interface, which contains a property with the log component required for logging. Our component `InjectorComponent` implements the `ILogging` interface and needs to be injected with a proper log component.

First, we will have a look at how to do this without injectors.

    public class InjectorModule : ... 
    {
        public ILog LogComponent { get { return GetSingleInstance<DebugLog>(); } }
    
        public IInjectorComponent InjectorComponent 
        { 
            get 
            {
                return GetSingleInstanceWithConstruct<IInjectorComponent>(m => new InjectorComponent() {
                    Log = m.LogComponent
                });
            }
        }
    }

That is how to do it. Pretty straight forward. But now let's assume we have several of these InjectorComponents:

    public class InjectorModule : ... 
    {
        public ILog LogComponent { get { return GetSingleInstance<DebugLog>(); } }
    
        public IInjectorComponent InjectorComponent1
        { 
            get 
            {
                return GetSingleInstanceWithConstruct<IInjectorComponent>(m => new InjectorComponent() {
                    Log = m.LogComponent
                });
            }
        }

        public IInjectorComponent InjectorComponent2
        { 
            get 
            {
                return GetSingleInstanceWithConstruct<IInjectorComponent>(m => new InjectorComponent() {
                    Log = m.LogComponent
                });
            }
        }

        public IInjectorComponent InjectorComponent3
        { 
            get 
            {
                return GetSingleInstanceWithConstruct<IInjectorComponent>(m => new InjectorComponent() {
                    Log = m.LogComponent
                });
            }
        }
    }

Looks pretty much the same for all three of them. So how do we solve this duplication of code to adhere to the DRY principle?

Well, let's try using an injector:

    public class DebugLogInjector : InterfaceInjector<IHaveLogComponent, IInjectorComponent> 
    {
        public void InjectInto(IInterfaceModificationContext<IHaveLogComponent, IInjectorComponent> context)
		{
			context.Inject((m, c) => c.Log = m.LogComponent);
		}
    }

You can then apply it by writing the following.

    // this interface is required for the injector to know where the logging component is available
    public interface IHaveLogComponent 
    {
          ILog LogComponent { get; } 
    }

    // the interface is applied to the module
    public class InjectorModule : ..., IHaveLogComponent 
    {
        public IInjectorComponent InjectorComponent1
        { 
            get 
            {
                return GetSingleInstance<IInjectorComponent>(cc => 
                    cc.Construct<InjectorComponent>()
                      .AddInjector(new DebugLogInjector())
                );
            }
        }

        public IInjectorComponent InjectorComponent2
        { 
            get 
            {
                return GetSingleInstance<IInjectorComponent>(cc => 
                    cc.Construct<InjectorComponent>()
                      .AddInjector(new DebugLogInjector())
                );
            }
        }

        public IInjectorComponent InjectorComponent3
        { 
            get 
            {
                return GetSingleInstance<IInjectorComponent>(cc => 
                    cc.Construct<InjectorComponent>()
                      .AddInjector(new DebugLogInjector())
                );
            }
        }
    }

What we just did is that we defined an injector. An injector encapsulates an injection pattern for a certain range of types.
When defining such injection patterns inside an injector it works exactly the same as registering injections the usual way.

In this case we used the most abstract injector that is available, an `InterfaceInjector`. It works on any interfaces of a component and a module.
In that way you can use it in a lot of places, given you reuse the interfaces for modules and components.

### Exact interface injectors

One additional hint. There is another type of injector which limits the interface to inject into to the exact interface with which a component is registered. This injector also provides the possibility to change the instance that is returned. It is called `IExactInterfaceInjector`.

This is not possible with the `IInterfaceInjector` because the compiler can not verify that you return the correct type. As the presented injectors are a type save approach to injection patterns, I decided to separate the interface injector in two different interfaces.