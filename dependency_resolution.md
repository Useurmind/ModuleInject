Dependency Resolution
---------------------

###Automatic Dependency Injection

Many DI containers out there support one or all of the following features:
 
 - Automatic constructor injection: Automatic resolution of constructor arguments based on the type of the argument.
 - Semiautomatic attribute based injection: one or several attributes which enable you to specify which dependencies to inject.

As of now, ModuleInject does not support any of these features. In my opinion these features disperse the dependency injection code. 

The automatic constructor injection hides the DI structure completely. It may be nice to have some automatic support for small to medium applications but in large applications this can become problematic when many developers need to understand the structure when working with the code.

The attribute based injection is just to focused on a special use case of a class. Only because many classes are used in a single scenario does not mean that it should be bound to that scenario (unit tests are always a second one). And usually you want the flexibility to use classes in several circumstances where you inject different dependencies to alter their behavior. Attribute based injection can limit your flexibility because you can only define one set of attributes on a given class.

###Explicit Dependency Injection

Many of the containers that are in common use today are just to fat for their purpose. In most cases you want to statically inject some dependencies into some class instance. For example:

    ServiceClass service = new ServiceClass();
    instance1.Repository = new RepositoryClass();

So why the hell do we need a DI container which hides this nice code that is completely clear to everyone and does not obstruct the meaning from the developer by some hocus-pocus API.

The first rather marginal problem occurs when you have chains of resolutions.

    ServiceClass1 service1 = new ServiceClass1(service2);

    ServiceClass2 service2 = new ServiceClass2();

Oh wait this code does not work because `service2` is not known when used as dependency of `service1`. Well, that is the problem when using plain code to define dependencies. You have to pay attention to the resolution order. Keeping resolution order valid however is a very cumbersome task, especially if you want to modularize your code.

That is why most DI containers hide your instances behind names and types, e.g.:

    RegisterType<ServiceClass1>("service1",
        new ConstructorInjection<ServiceClass2>("service2"));
    RegisterType<ServiceClass2>("service2");

Completely equivalent code that abstracts the order in the resolution process away from the order in the registration process (I ignored interface and IoC on purpose to make it simpler). Only problem now is that you have to deal with the strings.

Interestingly, the next step most container implementers take here is to make everything automatic as explained above. By doing that you take away most of the string handling which can be very cumbersome. For example:

    // ServiceClass1 has only a constructor with an argument 
    // of type ServiceClass2
    RegisterType<ServiceClass1>("service1");
    RegisterType<ServiceClass2>("service2");

You can now see what is hidden in the comment I inserted. The constructor of `ServiceClass1` fulfills the invariant that its signature matches the types of the registrations in the container. You could also say that the invariant is fulfilled by the container registrations. 

However it is, you just gave up some of your flexibility just to get rid of some strings that weren't there when you started implementing your container. And you wanted to implement your container to gain some flexibility. Weird...

####Being explicit in ModuleInject

So where does ModuleInject come into play here. In the end you don't gain anything for free. You always have to pay a price. With the standard DI container you get automatic features that will reduce your implemented code but will hide some of your logic and reduce your flexibility compared to plain old C# code (POCC).

So to be honest ModuleInject will increase the amount of code you write to perform a very simple injection. You need a module, you need a module interface, you need properties and factory methods that will be connected to the registrations. In most cases just a few more line. But only if you have all of this you can finally register and connect the components of your application.

The following code could then be written, which does the same as the code in the examples above:

    RegisterPublicProperty(x => x.Service1)
        .Construct(m => new ServiceClass1(m.Service2));

    RegisterPublicProperty(x => x.Service2)
        .Construct<ServiceClass2>();

Some things to notice:

 - Everything is explicitly stated, no hidden assumptions.
 - No strings anywhere that need to be managed.
 - All registrations look similar/same.

The strings are replaced by properties and methods which provide a completely different amount of integrated support by the compiler and refactoring tools (renaming, types, usages, etc.).

Because all registrations look similar by using the same keywords. Developers know exactly what happens in the code once they are accustomed to ModuleInject. The limited and explicit registration syntax provides a structure which can be searched for and understood very fast, once you understand ModuleInject. At the same time the syntax tries to give you all the freedom and flexibility you need.

####Expressions vs. Actions

Look at the code from the example above:

    RegisterPublicProperty(x => x.Service1)
        .Construct(m => new ServiceClass1(m.Service2));

The construct call has a signature with the following argument type.

    Expression<Func<TModule, TComponent>>

In that expression `TModule` is the type of your module and `TComponent` is the type of the registered component.

So why is this an expression and not a simple `Func` that just returns the correct component? The answer is simple: you cannot analyze the content of a `Func`. 

In the expression you have access to the module in which the component is registered (here via the argument `m`). ModuleInject will look at the expression and find all members of the module that are used in it. These members will be marked as prerequisites of the registered component. All prerequisites of a component are resolved before the component is resolved. (here `m.Service2` is the only prerequisite).

Truth be told, there are alternative possibilities for the syntax, e.g.:

    // ModuleInject
    .Construct(m => new ServiceClass1(m.Service2));

    // plain func with ugly strings
    .Construct(m =>  new ServiceClass1(m.Resolve<ServiceClass2>("Service2")));

    // automatic without strings, non explicit, less flexible
    .Construct(m =>  new ServiceClass1(m.Resolve<ServiceClass2>()));

    // we just moved the expression into 
    // the resolve call
    // task: find an even longer syntax...
    .Construct(m =>  new ServiceClass1(m.Resolve<ServiceClass2>(m2 => m2.Service2)));

To summarize, expressions seem like a good compromise.

####Module Resolution Process

In ModuleInject a module is always resolved as a whole. You call the method `Resolve` on a module and it will automatically resolve all its components and all submodules (modules used inside the module).

The steps are as follows:

 - Create and resolve instances of submodules with the `PrivateComponentAttribute` or the `PublicComponentAttribute`.
 - Get from the registry and resolve instances of submodules with the `RegistryComponentAttribute`.
 - Resolve all components of the module.

Resolving a component looks as follows:

 - Cancel if component is already resolved.
 - Resolve all prerequisites of the component.
 - Resolve the component itself.

This process assures that all dependencies from submodules and the module itself are fully resolved when they are used inside an expression for injection into a component.

####Summary

So what does ModuleInject provide?

 - Decouple registration and resolution.
 - Modularize the registration code.
 - Structure the registration code.
 - Keep the registration code flexible.
 - Keep the registration code very explicit.
 - Order dependencies in the resolution process.

And it does NOT offer:

 - (Semi)Automatic resolution of dependencies.
 - Hiding the structure of your application components in not-so-default rules.
 - Limiting your flexibility so that you don't need to manage a lot of strings that you normally do not need to manage.

Note: This text was written very offensive with the purpose of bringing out the differences between ModuleInject and other containers. Please do not take it personally, if you just implemented one of these containers. :)