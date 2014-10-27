using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Module
{
    using System.Runtime.InteropServices;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Container.Interface;
    using ModuleInject.Decoration;
    using ModuleInject.Registry;

    internal class ModuleResolver<IModule, TModule>
        where TModule : IModule
        where IModule : IInjectionModule
    {
        private TModule _module;
        private IDependencyContainer _container;
        private IRegistry _registry;

        public ModuleResolver(TModule module, IDependencyContainer container, IRegistry registry)
        {
            _module = module;
            _container = container;
            _registry = registry;
        }

        public void Resolve()
        {
            Type moduleInterfaceType = typeof(IModule);

            if (!moduleInterfaceType.IsInterface)
            {
                throw new ModuleInjectException("Modules must always have an Interface");
            }

            var submodulePropertyInfos = TypeExtensions.GetModuleProperties<IModule, TModule>(true);
            foreach (var submodulePropInfo in submodulePropertyInfos)
            {
                TryResolveComponent(submodulePropInfo);

                TryResolveSubmodule(submodulePropInfo);
            }

            foreach (var propInfo in TypeExtensions.GetModuleProperties<IModule, TModule>(false))
            {
                //if (!propInfo.PropertyType.IsInterface)
                //{
                //    CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_PropertyIsNoInterface, propInfo.Name);
                //}

                TryResolveComponent(propInfo);
            }
        }

        private bool TryResolveComponent(PropertyInfo propInfo)
        {
            bool resolved = true;

            object currentValue = propInfo.GetValue(_module, null);
            if (currentValue != null)
            {
                // does not work anymore because the module is connected with the container
                // the properties are now set when resolved in the container
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

            if (!resolved && propInfo.HasCustomAttribute<RegistryComponentAttribute>())
            {
                resolved = this.TryResolveComponentFromRegistry(propInfo);
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
            // unecessary because set by ComponentLifetime, but used for unit test
            propInfo.SetValue(_module, component, BindingFlags.NonPublic, null, null, null);
            return true;
        }

        private bool TryResolveComponentFromRegistry(PropertyInfo propInfo)
        {
            Type propType = propInfo.PropertyType;

            if (_registry == null)
            {
                return false;
            }

            if (!_registry.IsRegistered(propType))
            {
                return false;
            }
            var component = _registry.GetComponent(propType);
            propInfo.SetValue(_module, component, BindingFlags.NonPublic, null, null, null);
            return true;
        }

        private void TryResolveSubmodule(PropertyInfo subModulePropInfo)
        {
            InjectionModule submodule = (InjectionModule)subModulePropInfo.GetValue(_module, null);

            if (!submodule.IsResolved)
            {
                submodule.Resolve(_registry);
            }
        }
    }
}
