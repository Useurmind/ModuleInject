using Microsoft.Practices.Unity;
using ModuleInject.Interfaces;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject
{
    internal static class ModuleResolver
    {
        public static void Resolve<IModule, TModule>(TModule module, IUnityContainer container)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            Type moduleInterfaceType = typeof(IModule);

            if (!moduleInterfaceType.IsInterface)
            {
                throw new ModuleInjectException("Modules must always have an Interface");
            }

            var submodulePropertyInfos = GetModuleProperties<IModule, TModule>(true);
            foreach (var submodulePropInfo in submodulePropertyInfos)
            {
                TryResolveComponent<IModule, TModule>(module, container, submodulePropInfo);

                TryResolveSubmodule<IModule, TModule>(module, container, submodulePropInfo);
            }

            foreach (var propInfo in GetModuleProperties<IModule, TModule>(false))
            {
                if (!propInfo.PropertyType.IsInterface)
                {
                    CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_PropertyIsNoInterface, propInfo.Name);
                }

                TryResolveComponent<IModule, TModule>(module, container, propInfo);
            }
        }

        private static IEnumerable<PropertyInfo> GetModuleProperties<IModule, TModule>(bool thatAreModules)
            where TModule : IModule
        {
            Type moduleType = typeof(TModule);
            Type moduleInterface = typeof(IModule);
            Type injectionModuleType = typeof(IInjectionModule);

            var interfaceProperties = moduleInterface.GetProperties()
                                                     .Where(p =>
                                                      {
                                                          var searchedInterface = p.PropertyType.GetInterface(injectionModuleType.Name, false);
                                                          bool isModule = searchedInterface != null;
                                                          return thatAreModules ? isModule : !isModule;
                                                      })
                                                     .Select(p => moduleType.GetProperty(p.Name));

            var privateProperties = moduleType.GetProperties(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic)
                                              .Where(p =>
                                              {
                                                  bool isPrivate = p.GetCustomAttributes(typeof(PrivateComponentAttribute), false).Length > 0;
                                                  var searchedInterface = p.PropertyType.GetInterface(injectionModuleType.Name, false);
                                                  bool isModule = searchedInterface != null;

                                                  return isPrivate && (thatAreModules ? isModule : !isModule);
                                              });

            return privateProperties.Union(interfaceProperties);
        }

        private static bool TryResolveComponent<IModule, TModule>(TModule module, IUnityContainer container, PropertyInfo propInfo)
            where IModule : IInjectionModule
            where TModule : IModule
        {
            bool resolved = true;

            object currentValue = propInfo.GetValue(module, null);
            if (currentValue != null)
            {
                resolved = false;
            }
            else
            {
                ResolveComponent<IModule, TModule>(module, container, propInfo);
            }

            return resolved;
        }

        private static void ResolveComponent<IModule, TModule>(TModule module, IUnityContainer container, PropertyInfo propInfo)
            where IModule : IInjectionModule
            where TModule : IModule
        {
            Type propType = propInfo.PropertyType;
            string propName = propInfo.Name;
            if (!container.IsRegistered(propType, propName))
            {
                CommonFunctions.ThrowPropertyAndTypeException<TModule>(Errors.ModuleResolver_MissingPropertyRegistration, propName);
            }
            var component = container.Resolve(propInfo.PropertyType, propInfo.Name);
            propInfo.SetValue(module, component, BindingFlags.NonPublic, null, null, null);
        }

        private static void TryResolveSubmodule<IModule, TModule>(TModule module, IUnityContainer container, PropertyInfo subModulePropInfo)
            where TModule : IModule
            where IModule : IInjectionModule
        {
            string submoduleName = subModulePropInfo.Name;
            IInjectionModule submodule = (IInjectionModule)subModulePropInfo.GetValue(module, null);

            if (!submodule.IsResolved)
            {
                submodule.Resolve();
            }

            foreach (var propInfo in subModulePropInfo.PropertyType.GetProperties())
            {
                object subComponent = propInfo.GetValue(submodule, null);
                string subComponentName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", submoduleName, propInfo.Name);
                container.RegisterInstance(propInfo.PropertyType, subComponentName, subComponent);
            }
        }
    }
}
