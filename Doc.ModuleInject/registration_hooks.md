Registration Hooks
------------------

Although, ModuleInject is very explicit in that you must specify each injection there may be cases where you want to apply
an injection pattern over a whole module or even a set of modules. For this purpose, ModuleInject offers so called registration
hooks.

###The registration hook interface

The registration hook interface consists of three functions:

        bool AppliesToModule(object module);

        bool AppliesToRegistration(IRegistrationContext registrationContext);

        void Execute(IRegistrationContext registrationContext);

`AppliesToModule` states if the hook should be applied to components inside a given module. If this method returns false
the hook is completely ignored for the given module.

`AppliesToRegistration` states if the hook should apply to registration, e.g. a component. The parameter is a registration
context that can be evaluated.

Finally, the `Execute` method will be executed for all matching registrations. The given registration context can be used
to modify the registration in a manner that fits your needs.

By filtering out modules and registrations you can pinpoint only the registrations you want to modify. Additonally, ingoring modules
altogether will improve the performance impact a hook has on your registration process.

###Applying registration hooks

Registration hooks can be applied in two ways.

* Putting them in a registry
* Putting them in a module

The standard registry class provides you with a method to put registration hooks into it as well as the module class.

    registry.AddRegistrationHook(someRegistrationHook);

    ...

    // inside some module registration process
    this.AddRegistrationHook(someRegistrationHook);

It is not very hard to apply them. The important part is to understand what happens if a registration hook is applied
in a certain way.

When adding a registration hook to a module the hook is applied to that module only and only if `AppliesToModule` returns
true for that module.

When adding a registration hook to a registry the hook is applied to all modules that use this registry (either direct
or over aggregation with the parents registry). But it is only applied to a module if `AppliesToModule` returns true for the 
given module.

###Interface injector registration hook

Interface injectors can be reused efficiently for implementing a simple registration hook. This registration hook applies
to modules that implement a given interface and to registrations whose constructed type implements a given interface.

In this way, you can easily grab out a lot of registrations and apply an injection pattern to them. And because an interface
injector is used you can define the pattern on the same generic API that the injector uses.

There are shortcuts for adding such hooks:

    registry.AddRegistrationHook<IHookedComponent, IHookedModule>(ctx => 
    {
        // registration pattern here
        ctx.Inject(...).IntoProperty(...);
    });

###Registration hooks and performance

Be wary of the performance impact that hooks imply. The method `AppliesToModule` is possibly executed for each module in your 
program. The method `AppliesToRegistration` is executed for each registration in the modules to which the hook applies.

Therefore, you should make sure that these methods are rather fast and that hooks only apply to modules in which they should
do some work.