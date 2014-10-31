Registries
----------

###Registry resolution vs. module tree resolution

There are two principal approaches to build up the module graph that will create the runtime tree of your application.
In both scenarios a module will use another module, which I will call submodule.
From these submodules the module will inject components into components that itself declares.
That is how modules are connected in ModuleInject and how dependencies of components between different modules are resolved.

In and by itself this is a pretty simple setup. Take some components from a submodule and put them in some components that I create.
The interesting problem is now how to provide modules with submodules.

####Module Tree

The simplest approach is to declare a submodule as a private or public component of the module. 
That means the submodule is created by the module itself. 
No problem and no headache at all. 

It only becomes a problem when you want to use the submodule in different parts of your application. In that situation you would have
to distribute it over complex injection between modules. And you probably don't want that.

Submodules can be private and public components or they can be resolved from a registry.
The first one is to explicitely use modules as private components in another modules. I will call these modules submodules.
When a module declares a (private or public) property that contains a module interface, the created instance is a submodule thatstry.