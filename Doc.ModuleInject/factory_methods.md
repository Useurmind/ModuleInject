Factory methods
---------------

Until now you probably only saw injection of components via properties of the same or a different module (in unity this is 
equivalent to creating object with a ContainerControlledLifetimeManager). This would mean you would be required to create 
properties for all instances you want to create. In some cases this is cumbersome or even impossible. For such cases you 
can use factory methods. They create a new instance every time they are called (in unity equivalent to creating objects 
without the ContainerControlledLifetimeManager).

Let's see how you can do that. For this example we have the following components:

    public class Component1 : IComponent1 {
    }

    public class Component2 : IComponent2 {
        public IComponent1 Comp1 { get; private set; }
    }

Given we want to create a new instance of Component1 each time we need an instance of it, we could write the following 
module:

    public class FactoryModule : ... {
        public IComponent2 Component2No1 { get; private set; }
        public IComponent2 Component2No2 { get; private set; }

        [PrivateFactory]
        private IComponent1 CreateComponent1() {
            return CreateInstance(x => x.CreateComponent1());
        }

        public FactoryModule() {
            RegisterPrivateComponentFactory<IComponent1, Component1>(x => x.CreateComponent1());

            RegisterPublicComponent<IComponent2, Component2>(x => x.Component2No1)
                .Inject(x => x.CreateComponent1()).IntoProperty(x => x.Comp1);
            RegisterPublicComponent<IComponent2, Component2>(x => x.Component2No2)
                .Inject(x => x.CreateComponent1()).IntoProperty(x => x.Comp1);
        }
    }

This results in Comp1 of the two Component2 instances to be different instances of the Component1 type.

###Current limitations
No build in support for arguments.