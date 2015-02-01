using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    using System.Globalization;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Common.Utility;
    using ModuleInject.Decoration;
    using ModuleInject.Interfaces;

    public static class ModuleTypeExtensions
    {
        /// <summary>
        /// Gets the properties of the all types in the _module chain.
        /// </summary>
        /// <typeparam name="IModule">The interface of the _module.</typeparam>
        /// <typeparam name="TModule">The type of the _module.</typeparam>
        /// <param name="thatAreModules">if set to <c>true</c> only properties that are modules themselves are returned, else all other properties.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetModuleProperties<IModule, TModule>(bool? thatAreModules)
            where TModule : IModule
        {
            return ModuleTypeExtensions.GetModuleProperties(typeof(IModule), typeof(TModule), thatAreModules);
        }

        /// <summary>
        /// Gets the properties of the all types in the module inheritance chain.
        /// </summary>
        /// <param name="moduleInterface">The interface of the module.</param>
        /// <param name="moduleType">The type of the module.</param>
        /// <param name="thatAreModules">if set to <c>true</c> only properties that are modules themselves are returned, else all other properties.</param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetModuleProperties(Type moduleInterface, Type moduleType, bool? thatAreModules)
        {
            var interfaceProperties = moduleInterface.GetModuleComponentPropertiesRecursive();
            if (thatAreModules.HasValue)
            {
                interfaceProperties = interfaceProperties.Where(p =>
                                                         {
                                                             bool isModule = p.IsInjectionModuleType();
                                                             return thatAreModules == true ? isModule : !isModule;
                                                         });
            }
            interfaceProperties = interfaceProperties.Select(p => moduleType.GetProperty(p.Name));

            var privateProperties = moduleType.GetModuleComponentPropertiesRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                              .Where(p =>
                                              {
                                                  bool shouldBeReturned = p.HasCustomAttribute<PrivateComponentAttribute>();

                                                  if (thatAreModules.HasValue)
                                                  {
                                                      bool isModule = p.IsInjectionModuleType();

                                                      shouldBeReturned = shouldBeReturned && (thatAreModules == true ? isModule : !isModule);
                                                  }

                                                  return shouldBeReturned;
                                              });

            return privateProperties.Union(interfaceProperties);
        }

        public static bool IsInjectionModuleType(this PropertyInfo propertyInfo)
        {
            CommonFunctions.CheckNullArgument("propertyInfo", propertyInfo);

            var injectionModuleType = typeof(IModule);
            var searchedInterface = propertyInfo.PropertyType.GetInterface(injectionModuleType.Name, false);
            bool isModule = searchedInterface != null;
            return isModule;
        }

        public static bool ShouldStopModuleTypeRecursion(this Type type)
        {
            return type == typeof(object) || type == typeof(IModule) || type.Name.StartsWith("InjectionModule", StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the properties that represent module components of the given type.
        /// </summary>
        /// <remarks>
        /// Stops property search at Object, <see cref="IModule"/> and InjectionModule types, so no properties of these types
        /// are included.
        /// Also note that properties with the <see cref="NonModulePropertyAttribute"/> are excluded from the search.
        /// </remarks>
        /// <param name="type">The type.</param>
        /// <param name="bindingOptions">The binding flags.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetModuleComponentPropertiesRecursive(this Type type, BindingFlags? bindingOptions = null, bool excludeNonModuleProperties = true)
        {
            CommonFunctions.CheckNullArgument("type", type);

            if (type.ShouldStopModuleTypeRecursion())
            {
                return new List<PropertyInfo>();
            }

            IEnumerable<PropertyInfo> properties = bindingOptions == null ? type.GetProperties() : type.GetProperties(BindingFlags.DeclaredOnly | bindingOptions.Value);

            if (excludeNonModuleProperties)
            {
                properties =
                    properties.Where(p => p.GetCustomAttributes(typeof(NonModulePropertyAttribute), false).Count() == 0);
            }

            type.ForEachBaseTypeInModuleHierarchy(
                subType =>
                {
                    properties = properties.Union(subType.GetModuleComponentPropertiesRecursive(bindingOptions));
                    return false;
                });


            return properties;
        }

        private static void ForEachBaseTypeInModuleHierarchy(this Type type, Func<Type, bool> action)
        {
            Type[] checkedSubTypes = null;
            if (type.IsInterface)
            {
                checkedSubTypes = type.GetInterfaces();
            }
            else
            {
                checkedSubTypes = new Type[] { type.BaseType };
            }

            foreach (var subType in checkedSubTypes)
            {
                if (action(subType))
                {
                    break;
                }
            }
        }
    }
}
