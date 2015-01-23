using ModuleInject.Interfaces.Fluent;
using System.Linq;

namespace ModuleInject.Interfaces
{
    /// <summary>
    /// Interface for an injector that works on a specific class which is injected as a component.
    /// </summary>
    /// With this injector you can inject on the basis of the classes properties and methods.
    /// <typeparam name="IComponent">The interface of the class in which the injector injects.</typeparam>
    /// <typeparam name="TComponent">The class in which the injector injects.</typeparam>
    /// <typeparam name="IModule">The interface of the module from which components can be selected.</typeparam>
    /// <typeparam name="TModule">The class of the module from which components can be selected.</typeparam>
    public interface IClassInjector<IComponent, TComponent, IModule, TModule>
        where TComponent : IComponent
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        /// <summary>
        /// This function is executed to perform the injection into the component.
        /// </summary>
        /// <param name="context">The context which can be used to inject other components into this one.</param>
        void InjectInto(IComponentRegistrationContext<IComponent, TComponent, IModule, TModule> context);
    }
}
