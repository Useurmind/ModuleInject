Method injection
----------------

In the tutorial you learned how property injection works. But there are other kinds of injection that ModuleInject provides. One is the method injection.

It is performed by using the `Inject` function.

###Inject function
So to implement method injection you call an overload of the function `Inject` which can be used like in the following example:

    RegisterPublicComponent(x => x.Component)
        .Construct<InitializableComponent>()
        .Inject((comp, module) => comp.Initialize(module.ComponentToInject));

This is a very flexible approach because you can define any valid lambda EXPRESSION that uses either the module and the component to do something.
You can even call a method repeatedly to e.g. fill a list via the add method.

