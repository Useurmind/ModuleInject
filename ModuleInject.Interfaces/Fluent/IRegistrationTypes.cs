using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Interfaces.Fluent
{
    /// <summary>
    /// Interface for describing the types that are important during a registration process.
    /// </summary>
    public interface IRegistrationTypes
    {
        /// <summary>
        /// The interface of the component that is registered (could also be a class).
        /// This is the type part of the key under which the component is stored.
        /// </summary>
        Type IComponent { get; }

        /// <summary>
        /// The actual type of the component that is constructed (only available after constructor was called).
        /// </summary>
        Type TComponent { get; }

        /// <summary>
        /// The interface of the module in which the component is registered.
        /// </summary>
        Type IModule { get; }

        /// <summary>
        /// The actual type of the module in which the component is registered.
        /// </summary>
        Type TModule { get; }
    }
}
