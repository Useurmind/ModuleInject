using ModuleInject.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces
{
    /// <summary>
    /// Interface for an injector that works on a specific interface which is injected as a component.
    /// </summary>
    /// With this injector you can inject on the basis of the interfaces properties and methods.
    /// <typeparam name="IComponent">The interface of the class in which the injector injects.</typeparam>
    /// <typeparam name="IModule">The interface of the module from which components can be selected.</typeparam>
    /// <typeparam name="TModule">The class of the module from which components can be selected.</typeparam>
    public interface IInterfaceInjector<IComponent, IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        /// <summary>
        /// This function is executed to perform the injection into the component.
        /// </summary>
        /// <remarks>Only the component interface is available here, so you can only use that one.</remarks>
        /// <param name="context">The context which can be used to inject other components into this one.</param>
        void InjectInto(InterfaceRegistrationContext<IComponent, IModule, TModule> context);
    }
}
