The registration API
--------------------

Module inject provides a (partly fluent) API that tries to make the process of defining registrations as painless and safe as possible.

The API has several phases:

 - Registration
 - Construction
 - Modification

### Registration phase

In this phase a property or method of the module is registered as a component (factory).

#### Components vs. component factories

Mo

Components are singletons in the scope of a module and only constructed once. That is why they are stored inside of properties of the module which reflect the idea behind the components best. By default, their lifetime is managed by the module. Therefore, they are disposed together with the module they are contained in. 

Factories create components when requested. To reflect this behaviour they are implemented via methods of a module. By default, the lifetime of components that are created through factories lies in the hands of the requester of the component. The module will forget the created component after it was created.

#### Public vs. private

Public components and factories are part of the interface of a module which makes them available to other modules. In contrast a private component or factory is not part of the interface and therefore is usually not available to other modules. The public and private scheme of ModuleInject does not correspond to the access modifier scheme of C#. Private components can have a public access modifier.

### Construction phase

In the construction phase you define how an instance of the component is created. You can specify different construction processes which give you some flexibility when creating components.

 - Creation from type.
 - Creation from expression.
 - Setting a fixed instance.

Creation from a type is the shortest possible notation that can be used when a default constructor should be used. The creation from an expression can be used for constructor injection scenarios with a non-default constructor. When setting a fixed instance you can construct an object as you like and give it to the container for dependency injection.

### Modification phase

This phase is the most complex and gives you several possibilities to modify the constructed instance. Among other things you can:

 - Call methods on the instance.
 - Set properties on the instance.
 - Perform custom actions on the instance.
 - Use standard injection code in the form of so called injectors.
 - Apply a dynamic proxy in the form of a behaviour.