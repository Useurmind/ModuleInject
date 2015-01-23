using System;
using System.Linq;

using ModuleInject.Common.Disposing;
using ModuleInject.Interfaces;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// Internal interface for a <see cref="StandardRegistry"/>.
    /// </summary>
    /// <remarks>
    /// Registries are meant to distribute top level modules whose interface is unique but which are required 
    /// in a lot of other modules to resolve dependencies.
    /// </remarks>
    public abstract class RegistryBase : DisposableExtBase, IRegistry
    {

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>True</c> if yes, else <c>False</c>.</returns>
        public abstract bool IsRegistered(Type type);

        /// <summary>
        /// Gets the component for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The component for the type</returns>
        public abstract object GetComponent(Type type);
    }
}