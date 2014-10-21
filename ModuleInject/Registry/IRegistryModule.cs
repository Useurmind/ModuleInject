using System.Linq;

namespace ModuleInject.Registry
{
    using System;
    using System.Collections.Generic;

    using ModuleInject.Common.Disposing;
    using ModuleInject.Interfaces;
    using ModuleInject.Utility;

    /// <summary>
    /// Internal interface for a <see cref="RegistryModule"/>.
    /// </summary>
    /// <remarks>
    /// Registry modules are meant to distribute top level modules whose interface is unique but which are required 
    /// in a lot of other modules to resolve dependencies.
    /// </remarks>
    public abstract class IRegistryModule : DisposableExtBase
    {
        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>True</c> if yes, else <c>False</c>.</returns>
        internal abstract bool IsRegistered(Type type);

        /// <summary>
        /// Gets the component for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The component for the type</returns>
        internal abstract object GetComponent(Type type);

        /// <summary>
        /// Merges this registry module with the specified other registry module.
        /// </summary>
        /// <remarks>
        /// The order is important because values of the other registry module are only overtaken
        /// if they do not override registrations in this registry module.
        /// </remarks>
        /// <param name="otherRegistryModule">The other registry module.</param>
        /// <returns></returns>
        internal abstract IRegistryModule Merge(IRegistryModule otherRegistryModule);

        /// <summary>
        /// Gets all registration entries of the current registry.
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<RegistrationEntry> GetRegistrationEntries();
    }
}