using Microsoft.Practices.Unity;

using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Modules
{
    using System.Runtime.InteropServices;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Container.Interface;
    using ModuleInject.Decoration;
    using ModuleInject.Interfaces;

    internal class ModuleResolver<IModule, TModule>
        where TModule : IModule
        where IModule : Interfaces.IModule
    {
        private TModule module;
        private IDependencyContainer _container;
        private IRegistry registry;

        public ModuleResolver(TModule module, IDependencyContainer container, IRegistry registry)
        {
            this.module = module;
            _container = container;
            this.registry = registry;
        }

        public void CheckBeforeResolve()
        {
            Type moduleInterfaceType = typeof(IModule);

            if (!moduleInterfaceType.IsInterface)
            {
                throw new ModuleInjectException("Modules must always have an Interface");
            }
        }

        public void ResolveComponents()
        {
            foreach (var propInfo in ModuleTypeExtensions.GetModuleProperties<IModule, TModule>(false))
            {
                //if (!propInfo.PropertyType.IsInterface)
                //{
                //    CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_PropertyIsNoInterface, propInfo.Name);
                //}

                this.TryResolveComponent(propInfo);
            }
        }

        public void ResolveSubmodules()
        {
            var submodulePropertyInfos = ModuleTypeExtensions.GetModuleProperties<IModule, TModule>(true);
            foreach (var submodulePropInfo in submodulePropertyInfos)
            {
                this.TryResolveComponent(submodulePropInfo);

                ResolverUtility.TryResolveSubmodule(submodulePropInfo, this.registry, this.module);
            }
        }

        private bool TryResolveComponent(PropertyInfo propInfo)
        {
            bool resolved = true;

            object currentValue = propInfo.GetValue(this.module, null);
            if (currentValue != null)
            {
                // does not work anymore because the module is connected with the Container
                // the properties are now set when resolved in the Container
                //if (!propInfo.HasCustomAttribute<ExternalComponentAttribute>())
                //{
                //    ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_PropertyWithoutExternalAttribute, propInfo.Name);
                //}
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

            bool resolved = false;

            if (!resolved && propInfo.HasCustomAttribute<PrivateComponentAttribute>())
            {
                // private components take priority over registry components
                resolved = this.TryResolveComponentFromContainer(propInfo);
            }

            if (!resolved)
            {
                // public components
                resolved = this.TryResolveComponentFromContainer(propInfo);
            }

            if (!resolved)
            {
                ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_MissingPropertyRegistration, propName);
            }
        }

        private bool TryResolveComponentFromContainer(PropertyInfo propInfo)
        {
            Type propType = propInfo.PropertyType;
            string propName = propInfo.Name;

            if (!_container.IsRegistered(propName, propType))
            {
                return false;
            }
            var component = _container.Resolve(propInfo.Name, propInfo.PropertyType);
            // unnecessary because set by ComponentLifetime, but used for unit test
            typeof(TModule).SetPropertyRecursive(this.module, propInfo.Name, component);
            return true;
        }
    }
}
