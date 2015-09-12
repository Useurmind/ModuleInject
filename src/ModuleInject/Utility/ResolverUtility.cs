using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using ModuleInject.Interfaces;

namespace ModuleInject.Utility
{
    /// <summary>
    /// Common functions for <see cref="ModuleResolver"/> and <see cref="RegistryResolver"/>.
    /// </summary>
    public static class ResolverUtility
    {
        /// <summary>
        /// Tries to resolve a submodule.
        /// It the submodule is not yet resolved it will now be resolved with the given registry.
        /// </summary>
        /// <param name="subModulePropInfo">The sub module property information.</param>
        /// <param name="registry">The registry.</param>
        /// <param name="module">The module.</param>
        public static void TryResolveSubmodule(PropertyInfo subModulePropInfo, IRegistry registry, IModule module)
        {
            IModule submodule = (IModule)subModulePropInfo.GetValue(module, null);

            if (!submodule.IsResolved)
            {
                submodule.Resolve(registry);
            }
        }
    }
}
