using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Injection;
using ModuleInject.Interfaces;
using ModuleInject.Modularity;

namespace ModuleInject.Provider.ProviderFactory
{
    public static class ModuleServiceProviderExtensions
    {
        /// <summary>
        /// This methods extracts all properties and get methods from a module and adds them as service sources
        /// to a service provider.
        /// The following properties and methods are excluded:
        /// - Properties filled from the registry (adorned with the <see cref="FromRegistryAttribute"/>).
        /// - Ones starting from type <see cref="InjectionModule{T}" />.
        /// </summary>
        /// <typeparam name="TModule">The type of the module whose properties and methods should be added.</typeparam>
        /// <param name="serviceProvider">The service provider to add the properties and methods as service sources to.</param>
        /// <param name="module">The module from which the properties and methods should be extracted.</param>
        public static void FromModuleExtractAll<TModule>(this ServiceProvider serviceProvider, TModule module)
            where TModule : InjectionModule<TModule>
        {
            serviceProvider.FromInstance(module)
                        .AllProperties()
                        .ExceptFrom<InjectionModule<TModule>>(true)
                        .Where(x => x.GetCustomAttribute<FromRegistryAttribute>() == null)
                        .Extract()
                        .AllGetMethods()
                        .ExceptFrom<InjectionModule<TModule>>(true)
                        .Extract();
        }
    }
}
