using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    /// <summary>
    /// First encountered interface when working with the fluent API.
    /// An instance that implements it is returned by the module when a component/factory is registered.
    /// </summary>
    /// <typeparam name="IComponent">Interface of the component.</typeparam>
    /// <typeparam name="IModule">Interface of the module.</typeparam>
    /// <typeparam name="TModule">Type of the module.</typeparam>
    public interface IRegistrationContext<IComponent, IModule, TModule> : IRegistrationContextT
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        /// <summary>
        /// Construct the component from a given type.
        /// </summary>
        /// <typeparam name="TComponent">The class that should be instantiated for the component that is registered.</typeparam>
        /// <returns>A context for fluent injection into the component.</returns>
        IComponentRegistrationContext<IComponent, TComponent, IModule, TModule>
            Construct<TComponent>()
            where TComponent : IComponent;
    }
}
