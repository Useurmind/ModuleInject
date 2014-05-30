ModuleInject
=============

Modules & Components
--------------------
In ModuleInject everything is centered around modules and components. 

Components are the working units in your application, the classes that provide your functionality.

On the other hand, modules are purely made to divide your application in logical sections. They provide an interface that other modules can rely on and initialize the components that are managed by them. The interface of a module is standard C# interface with properties representing the components that the module should provide to other modules.

	public interface IMainModule : IInjectionModule
	{
		IPrintComponent PrintComponent { get; }
	}

This is how a very basic interface for module named IMainModule could look like. Other modules will consume this module purely by working with this interface.
This gives you nice boundaries that won't be crossed so easily. Also notice that components are offered in the form of interfaces. This is also important because else you would reveal the inner workings of your module to other modules. 
You can see that ModuleInject is all about contracting between different parts of your application and separating them clearly through these contracts. This also allows to exchange modules relatively easy when you see the need to do so.

Your first module
-----------------

###The modules interface is its contract

Let's take a leap at implementing the module interface given above. First we define concrete class of our module:

	public class MainModule : InjectionModule<IMainModule, MainModule>, IMainModule {
		public IPrintComponent PrintComponent { get; private set; }
	}

Looks easy enough. You define the class MainModule which implements the interface IMainModule that we defined above. Additionally, it derives from the InjectionModule class which is generic and takes the interface of the module and the type of the module itself. Also we need to implement the single public component that the module should offer.

###The modules first component

Obviously, there is no information yet how the component should look like and how it will be resolved. We will fix that just now by defining the interface of the component:

	public interface IPrintComponent {
		void PrintOutput();
	}

Can you imagine what it will do? So lets see how the first draft of our component should look like.

	public class PrintComponent : IPrintComponent {
		public void PrintOutput() {
			Console.WriteLine("Hello World");
		}
	}
	
###Registering the component in the module

Finally, we have a component to print a Hello World greeting for us. But how do we employ it in the module? First we need to register the component, for example in the constructor of our module:

	public MainModule() {
		RegisterPublicComponent<IPrintComponent, PrintComponent>(x => x.PrintComponent);
	}

This registers the PrintComponent property of the MainModule to be a PrintComponent instance. 

###Resolving the module

On calling the Resolve method of the module the property will be filled with the specified instance. After this we can use the module as follows to print our greeting:

	static void Main(string[] args)
	{
		IMainModule module = new MainModule();
		module.Resolve();
		
		IPrintComponent component = module.PrintComponent;
		component.PrintOutput();
	}
	
This will result in the following output:
	"Hello World"
	
What happened is that in the Resolve call the Property PrintComponent of the MainModule was filled with an instance of the PrintComponent class. We used this instance to print the greeting.

###Property injection and private components
So we have our first module up and running. But now lets make it a bit more complicated to show off some of the features of ModuleInject.

	public interface INameComponent {
		string NameToGreet { get; }
	}
	
	public class NameComponent {
		public string NameToGreet { get; set; }
	}
	
	public class PrintComponent : IPrintComponent {
		public INameComponent NameProvider { get; set; }
	
		public void PrintOutput() {
			Console.WriteLine("Hello {0}", NameProvider.NameToGreet);
		}
	}
	
Basically, we make the greeting string configurable via dependency injection. With these components we extend our MainModule like this:

	public class MainModule : ... 
	{
		...
		
		[PrivateComponent]
		private INameComponent NameComponent { get; set; }
		
		public MainModule() {
			RegisterPrivateComponent<INameComponent, NameComponent>(x => x.NameComponent)
				.Inject("ModuleInject").IntoProperty(x => x.NameToGreet);
		
			RegisterPublicComponent<IPrintComponent, PrintComponent>(x => x.PrintComponent)
				.Inject(x => x.NameComponent).IntoProperty(x => x.NameProvider);
		}
	}

A lot of stuff just happened. The components are relatively straight forward. But in the registration process we now can see that there a two kinds of components in a module.
First the public components which are available through the interface of the module and second the private components which are only available inside the module. The private components must be decorated with the PrivateComponentAttribute.
But for now let's concentrate on the registration process. As before we register both components and specify the their actual implementations. But in addition we now inject values into properties of the components. This is done with the fluent API via the Inject and IntoProperty methods. The Inject method describes the property in our module from where to take the injected value. And the IntoProperty call identifies the property of the component in which we want to inject the dependency. 
You can inject properties of the module, of submodules and constant values also.

If we now execute our application (which we didn't need to change), we get the following output:
	"Hello ModuleInject"
