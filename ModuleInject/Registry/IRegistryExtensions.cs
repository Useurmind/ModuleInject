using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    using ModuleInject.Common.Utility;
    using ModuleInject.Interfaces;
    using ModuleInject.Utility;

    public static class IRegistryExtensions
    {
        /// <summary>
        /// Registers a given module in the registry.
        /// </summary>
        /// <typeparam name="IModule">The interface of the module.</typeparam>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="registry">The registry module in which the module is registered.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Explicit generic interface")]
        public static void RegisterModule<IModule, TModule>(this StandardRegistry registry)
            where TModule : IModule, new()
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("Registry", registry);

            registry.Register(() => (IModule)(new TModule()));
        }

        /// <summary>
        /// Merges this registry module with the specified other registry module.
        /// </summary>
        /// <remarks>
        /// The order is important because values of the other registry module are only overtaken
        /// if they do not override registrations in this registry module.
        /// </remarks>
        /// <param name="otherRegistry">The other registry module.</param>
        /// <returns></returns>
        internal static IRegistry Merge(this IRegistry registry, IRegistry otherRegistry)
        {
            var emptyRegistry1 = registry as EmptyRegistry;
            var emptyRegistry2 = otherRegistry as EmptyRegistry;

            if (emptyRegistry1 != null)
            {
                return otherRegistry;
            } 
            else if (emptyRegistry2 != null)
            {
                return registry;
            }

            var newRegistry = new AggregateRegistry();
            newRegistry.AddRegistry(registry);
            newRegistry.AddRegistry(otherRegistry);
            return newRegistry;
        }
    }
}
