﻿using System.Linq;

using System;

using ModuleInject.Interfaces.Disposing;
using System.Collections.Generic;
using ModuleInject.Interfaces.Hooks;

namespace ModuleInject.Interfaces
{
    /// <summary>
    /// Interface for a registry that is used to distribute components over
    /// a tree of modules.
    /// </summary>
    public interface IRegistry : IDisposableExt
    {
        /// <summary>
        /// Is the given type registered in the registry.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if yes, else false</returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Get a component of the given type from the registry.
        /// </summary>
        /// <param name="type">The type of the component.</param>
        /// <returns>The component.</returns>
        object GetComponent(Type type);

        /// <summary>
        /// Gets all registration hooks that are present in this registry.
        /// </summary>
        /// <returns>The registration hooks.</returns>
        IEnumerable<IRegistrationHook> GetRegistrationHooks();
    }
}
