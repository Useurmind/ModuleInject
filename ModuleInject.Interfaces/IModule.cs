using System.Linq;

namespace ModuleInject.Interfaces
{
    using System;

    /// <summary>
    /// Interface for a module which is part of the injection process.
    /// 
    /// Invariants:
    /// - If <see cref="IsResolved"/> is true, all component properties of the public interface are filled and all factories return the correct components.
    /// - Calling <see cref="Resolve"/> leads to <see cref="IsResolved"/> being true.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Is the module already resolved.
        /// </summary>
        bool IsResolved { get; }

        /// <summary>
        /// Resolve the module and all its submodules.
        /// </summary>
        void Resolve();


        /// <summary>
        /// Resolve the module and all its submodules using the given registry.
        /// </summary>
        void Resolve(IRegistry parentRegistry);
    }
}
