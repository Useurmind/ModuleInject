using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    using ModuleInject.Interfaces;
    using ModuleInject.Utility;

    public static class RegistryModuleExtensions
    {
        /// <summary>
        /// Registers a given module in the registry.
        /// </summary>
        /// <typeparam name="IModule">The interface of the module.</typeparam>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="registryModule">The registry module in which the module is registered.</param>
        public static void RegisterModule<IModule, TModule>(this RegistryModule registryModule)
            where TModule : IModule, new()
            where IModule : IInjectionModule
        {
            CommonFunctions.CheckNullArgument("registryModule", registryModule);

            registryModule.Register(() => (IModule)(new TModule()));
        }
    }
}
