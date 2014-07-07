Method injection
----------------

In the tutorial you learned how property injection works. But there are other kinds of injection that ModuleInject provides. One is called method injection.

There are two ways to implement method injection.

###IInitializable

Method injection in ModuleInject is strongly typed. The first way requires you to implement a special interface of the component that you want to inject into through method injection. 

This interface is called IInitializable. It is generic  and it's type parameters are the types of the parameters that go into the function that is used for method injection. Here is the version with one parameter:

    public interface IInitializable<TArgument>
    {
        void Initialize(TArgument dependency1);
    }

It is available with a range of numbers of arguments. But keep in mind that in a "good" class design only a limited number of dependencies are required per class.

So, let's dive into implementing and using the IInitializable interface. First we define our components interface and implementation:

    public interface IInitializableComponent {
        ISomeUndefinedComponent InjectedComponent { get; }
    }

    public class InitializableComponent : 
        IInitializableComponent, IInitializable<ISomeUndefinedComponent> {

        public ISomeUndefinedComponent InjectedComponent { get; private set; }

        public void Initialize(ISomeUndefinedComponent injectedComponent) {
            InjectedComponent = injectedComponent;
        }
    }

Pretty simple functionality which only servers to show what you need to do and what you are able to do with ModuleInject. There is only one thing I want to mention here. As you can see, the IInitializable interface is only used in the component implementation itself. That is because the initialization process is not necessarily stuff that belongs into the behavior contract of the component. After the instance is initialized by the IoC framework which knows the exact implementation, the initialization functionality normally isn't interesting anymore. Therefore, I left it out of the interface here.

Finally, lets use the method injection to inject something into our class. I will only show the registration here and leave the rest of the module out:

    public IInitializableComponent Component { get; set; }

    [PrivateComponent]
    private ISomeUndefinedComponent ComponentToInject { get; set; }

    public ModuleWithMethodInjectionComponent() {
        RegisterPrivateComponent<ISomeUndefinedComponent, SomeUndefinedComponent>(x => x.ComponentToInject);

        RegisterPublicComponent<IInitializableComponent, InitializableComponent>(x => x.Component)
            .InitializeWith(x => x.ComponentToInject);
    }

So we just register the initializable component and its dependency. Afterwards, we use the dependency in the InitializeWith call. This results in the components Initialize method being called after construction with the injected component as an argument.

###Current limitations
Their are currently only InitializeWith calls for up to 3 arguments and no overloads for using constant values. Can be easily fixed by creating the overloads. But time is so scarce.

###CallMethod
The second way to implement method injection is the function CallMethod which can be used like in the following example:

    RegisterPublicComponent<IInitializableComponent, InitializableComponent>(x => x.Component)
        .CallMethod((comp, module) => comp.Initialize(module.ComponentToInject));

This works pretty much the same as calling InitializeWith but is much more flexible. This is because you can call any method.
Note that you can only inject components of the module specified in the parameter list and constant values. Both types of parameters can be casted to get the correct type.