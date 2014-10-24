using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    using ModuleInject.Common.Utility;
    using ModuleInject.Decoration;

    public static class TypeExtensions
    {
        public static bool IsInjectionModuleType(this PropertyInfo propertyInfo)
        {
            CommonFunctions.CheckNullArgument("propertyInfo", propertyInfo);

            var injectionModuleType = typeof(IInjectionModule);
            var searchedInterface = propertyInfo.PropertyType.GetInterface(injectionModuleType.Name, false);
            bool isModule = searchedInterface != null;
            return isModule;
        }

        public static PropertyInfo GetPropertyRecursive(this Type type, string name, BindingFlags? bindingOptions = null)
        {
            CommonFunctions.CheckNullArgument("type", type);

            if (type.ShouldStopModuleTypeRecursion())
            {
                return null;
            }

            PropertyInfo propertyInfo = bindingOptions.HasValue ? type.GetProperty(name, BindingFlags.Instance|bindingOptions.Value) : type.GetProperty(name);

            if (propertyInfo == null)
            {
                type.ForEachBaseTypeInModuleHierarchy(
                    subType =>
                        {
                            propertyInfo = bindingOptions.HasValue ? subType.GetProperty(name, BindingFlags.Instance|bindingOptions.Value) : subType.GetProperty(name);
                            if (propertyInfo != null)
                            {
                                return true;
                            }
                            return false;
                        });
            }

            return propertyInfo;
        }

        private static bool ShouldStopModuleTypeRecursion(this Type type)
        {
            return type == typeof(object) || type == typeof(IInjectionModule) || type.Name.StartsWith("InjectionModule", StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets the properties that represent module components of the given type.
        /// </summary>
        /// <remarks>
        /// Stops property search at Object, <see cref="IInjectionModule"/> and InjectionModule types, so no properties of these types
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

            IEnumerable<PropertyInfo> properties = bindingOptions == null ? type.GetProperties() : type.GetProperties(BindingFlags.DeclaredOnly|bindingOptions.Value);

            properties =
                properties.Where(p => p.GetCustomAttributes(typeof(NonModulePropertyAttribute), false).Count() == 0);

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
