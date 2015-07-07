The registration API
--------------------

Module inject provides a (partly fluent) API that tries to make the process of defining registrations as painless and safe as possible.

The API has several phases:

 - Instantiation
 - Disposing
 - Construction
 - Modification

### Instantiation

In this phase you define which kind of strategy should be used to create instances of the component, e.g. single instance or factory. You can define arbitrary strategies but for single instances and factories the InjectionModule class provides shortcut functions that skip this step.

### Disposing

In this phase you define which kind of dispose strategy should be used to dispose instances of the component. You can skip this step if you are satisfied with no dispose strategy or the defaults that are applied.
The defaults are:

 - SingleInstance: Instances are remembered and disposed with the module.
 - Factory: Instances are forgot and never disposed (consumer is repsonsible for disposing).
 - Pure SourceOf: No default dispose strategy (means null).

### Construction

In the construction phase you define how an instance of the component is created. You can specify different construction processes which give you some flexibility when creating components.

 - Creation from type.
 - Creation from func.

Creation from a type is the shortest possible notation that can be used when a default constructor should be used. The creation from a func can be used for constructor injection scenarios with a non-default constructor or for applying property initializer syntax. You can inject fixed instances by using a closure in the construct func.

### Modification phase

This phase allows you to perform further modifications of the registered component. You can:

 - Execute actions that are performed when constructing an instance (e.g. for method injection).
 - Change the instance that is returned (e.g. for decoration).
 - Add meta data to the component (e.g. for easy recognition in registration hooks).
 
Also, there can be further extensions depending on the loaded extensions. For example, you can add interceptors when including ModuleInject.Interception.Castle.