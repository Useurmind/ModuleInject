using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
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
