using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using ModuleInject.Common.Utility;
using ModuleInject.Interfaces;

namespace ModuleInject.Utility
{
    public static class ModuleTypeExtensions
    {
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
        public static IEnumerable<PropertyInfo> GetModuleComponentPropertiesRecursive(this Type type, BindingFlags? bindingOptions = null)
        {
            CommonFunctions.CheckNullArgument("type", type);

            if (type.ShouldStopModuleTypeRecursion())
            {
                return new List<PropertyInfo>();
            }

            IEnumerable<PropertyInfo> properties = bindingOptions == null ? type.GetProperties() : type.GetProperties(BindingFlags.DeclaredOnly | bindingOptions.Value);

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
