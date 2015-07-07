Unit Testing
------------

In ModuleInject every class that fullfills the `IModule` interface can be used as a submodule of other modules.

This also leads to the fact that you can mock submodules if you keep the invariants of the `IModule` interface in mind:

 - If `IsResolved` is true, you can request any components or factories of the module.
 - Calling `Resolve` leads to `IsResolved` being true.

You can for example use Moq to create a mock for the following module interface:

    public interface IMockedModule : IModule
    {
        IMockedComponent MockedComponent { get; set; }

        IMockedComponent CreateMockedComponent();
    }

The creation of the mock would look like this:

	// create module mock
	var moduleMock = new Mock<IMockedModule>();

	// setup component getter
	moduleMock.SetupGet(x => x.MockedComponent).Returns(Mock.Of<IMockedComponent>());

	// setup component factory
    moduleMock.Setup(x => x.CreateMockedComponent()).Returns(Mock.Of<IMockedComponent>());

How you introduce the mock into the tested module can vary depending on the use case.

 - If the mocked module comes from a registry, you can set one in the test that contains the mocked module.
 - Else you have to provide some means to inject it in the tests.

Example code for introduction via a registry:

    // create registry containing the mock created above
	mockRegistry = new StandardRegistry();
	mockRegistry.Register<IMockedModule>(() => moduleMock.Object);

	// apply the registry to the tested module
	testedModule.Registry = mockRegistry;