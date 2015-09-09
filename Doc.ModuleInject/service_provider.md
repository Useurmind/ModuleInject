Service Provider
----------------

In some situations you need an implementation of `IServiceProvider` for your dependency injection framework.
This is for example the case in ASP.Net vNext applications that work with that interface.
As it is not straigth forward to provide such an implementation, ModuleInject does so for you.

	var serviceProvider = new ServiceProvider();

	serviceProvider.AddServiceSource(() => new MyService());

	var myService = (MyService)serviceProvider.GetService(typeof(MyService));

This example is very poor because it shows you nothing. Except that the `ServiceProvider` class implements the interface `IServiceProvider`.

### Filling a service provider from arbitrary instances

But we now get to the important stuff. You can construct a `ServiceProvider` from the properties and methods of any given object instance.

	serviceProvider.FromInstance(new MyServiceFactory())
				   .AllProperties()
				   .Extract()
				   .AllGetMethods()
				   .Extract();

This code adds all properties and get methods as services to the service provider (Get methods are methods with no parameters and a non void return type).

When a service is requested the available properties and methods are called to retrieve an instance of the service.

### Filling a service provider from module instances

The above API is used to define the following method:

	serviceProvider.FromModuleExtractAll(new MyModule());

This code extracts all properties and get methods from the given module (excluding the properties and methods on the types `InjectionModule<MyModule>` and above).