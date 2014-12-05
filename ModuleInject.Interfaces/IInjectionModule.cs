using System.Linq;

namespace ModuleInject.Interfaces
{
    using System;

    /// <summary>
    /// Interface for a module which is part of the injection process.
    /// 
    /// Invariants:
    /// - If <see cref="IsResolved"/> is true, all component properties of the public interface are filled.
    /// - Calling <see cref="Resolve"/> leads to <see cref="IsResolved"/> being true.
    /// - Component factories of the public interface, <see cref="GetComponent"/> and <see cref="GetComponent{T}"/>
    ///   always return instances according to the registrations in the module.
    /// </summary>
    public interface IInjectionModule
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
        void Resolve(IRegistry registry);

        /// <summary>
        /// Get a component of the specified type with the specified name.
        /// </summary>
        /// <param name="componentType">The type/interface with which the component is registered in the module.</param>
        /// <param name="componentName">The name of the component.</param>
        /// <returns>If the component is found it is returned, else an exception is thrown.</returns>
        object GetComponent(Type componentType, string componentName);

        /// <summary>
        /// Generic version of <see cref="GetComponent"/>.
        /// </summary>
        /// <typeparam name="IComponent">The type/interface with which the component is registered in the module.</typeparam>
        /// <param name="componentName">The name of the component.</param>
        /// <returns>If the component is found it is returned, else an exception is thrown.</returns>
        IComponent GetComponent<IComponent>(string componentName);
    }
}
