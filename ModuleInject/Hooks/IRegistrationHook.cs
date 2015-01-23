using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Interfaces.Fluent;

namespace ModuleInject.Hooks
{
    /// <summary>
    /// Flags to desribe to which components a <see cref="IRegistrationHook" /> should apply.
    /// </summary>
    [Flags]
    public enum RegistrationHookComponentFilterFlags
    {
        /// <summary>
        /// Hook applies to nothing.
        /// </summary>
        None = 0,

        /// <summary>
        /// Hook applies to public components.
        /// </summary>
        PublicComponent = 2,

        /// <summary>
        /// Hook applies to private components.
        /// </summary>
        PrivateComponent = 4,

        /// <summary>
        /// Hook applies to public factories.
        /// </summary>
        PublicFactory = 8,

        /// <summary>
        /// Hook applies to private factories.
        /// </summary>
        PrivateFactory = 16,

        Public = PublicComponent | PublicFactory,
        Private = PrivateComponent | PrivateFactory,
        Component = PublicComponent | PrivateComponent,
        Factory = PublicFactory | PrivateFactory,
        All = PublicComponent | PrivateComponent | PublicFactory | PrivateFactory
    }

    /// <summary>
    /// Interface for hooks that are executed when a component/factory is registered.
    /// </summary>
    public interface IRegistrationHook
    {
        /// <summary>
        /// Should the hook be used inside the given module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns>True if the hook should be applied to the module, else false.</returns>
        bool AppliesToModule(object module);

        /// <summary>
        /// Should the hook be executed for a given registration.
        /// </summary>
        /// <remarks>
        /// If a hook applies to all modules, this method is performed for each and every registration
        /// that you do in your dependency injection process.
        /// Therefore, be cautios about using hooks and make this method as performant as possible.
        /// </remarks>
        /// <param name="registrationContext">The registration context of the registration.</param>
        /// <returns>True if the hook should be executed.</returns>
        bool AppliesToComponent(IRegistrationContext registrationContext);

        /// <summary>
        /// Executes the hook for the registration of a component/factory.
        /// </summary>
        /// <param name="registrationContext">The registration context of the registration.</param>
        void Execute(IRegistrationContext registrationContext);
    }
}
