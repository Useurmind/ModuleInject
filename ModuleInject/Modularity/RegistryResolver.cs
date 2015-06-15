using System.Reflection;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using ModuleInject.Decoration;
using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Utility;

namespace ModuleInject.Modularity
{
    /// <summary>
    /// Finds all properties of the module marked with the <see cref="FromRegistryAttribute"/>
    /// and tries to retrieve them from the registry given to it.
    /// If the properties contain modules they are resolved too with the given registry.
    /// </summary>
    public class RegistryResolver
    {
        private Module module;
        private IRegistry usedRegistry;
        private Type moduleType;

        public RegistryResolver(Module module, IRegistry usedRegistry)
        {
            this.module = module;
            this.moduleType = module.GetType();
            this.usedRegistry = usedRegistry;
        }

        /// <summary>
        /// Resolves the registry components in the module.
        /// </summary>
        public void Resolve()
        {
            var submodulePropertyInfos =
                this.moduleType.GetModuleComponentPropertiesRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, false)
                               .Where(
                                    p =>
                                    {
                                        bool isRegistry = p.HasCustomAttribute<FromRegistryAttribute>();
                                        return isRegistry;
                                    });

            foreach (var submodulePropInfo in submodulePropertyInfos)
            {
                TryResolveComponent(submodulePropInfo);

                if (submodulePropInfo.IsInjectionModuleType())
                {
                    ResolverUtility.TryResolveSubmodule(submodulePropInfo, this.usedRegistry, this.module);
                }
            }
        }

        private bool TryResolveComponent(PropertyInfo propInfo)
        {
            bool resolved = true;

            object currentValue = propInfo.GetValue(this.module, null);
            if (currentValue != null)
            {
                resolved = false;
            }
            else
            {
                ResolveComponent(propInfo);
            }

            return resolved;
        }

        private void ResolveComponent(PropertyInfo propInfo)
        {
            string propName = propInfo.Name;

            bool resolved = TryResolveComponentFromRegistry(propInfo);
            if (!resolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException(this.module.GetType(), Errors.ModuleResolver_MissingPropertyRegistration, propName);
            }
        }

        private bool TryResolveComponentFromRegistry(PropertyInfo propInfo)
        {
            Type propType = propInfo.PropertyType;

            if (this.usedRegistry == null)
            {
                return false;
            }

            if (!this.usedRegistry.IsRegistered(propType))
            {
                return false;
            }
            var component = this.usedRegistry.GetComponent(propType);
            this.moduleType.SetPropertyRecursive(this.module, propInfo.Name, component);
            return true;
        }
    }
}
