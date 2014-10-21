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
            var injectionModuleType = typeof(IInjectionModule);
            var searchedInterface = propertyInfo.PropertyType.GetInterface(injectionModuleType.Name, false);
            bool isModule = searchedInterface != null;
            return isModule;
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

            if (type == typeof(object) || type == typeof(IInjectionModule) || type.Name.StartsWith("InjectionModule", StringComparison.Ordinal))
            {
                return new List<PropertyInfo>();
            }

            IEnumerable<PropertyInfo> properties = bindingOptions == null ? type.GetProperties() : type.GetProperties(BindingFlags.DeclaredOnly|bindingOptions.Value);

            properties =
                properties.Where(p => p.GetCustomAttributes(typeof(NonModulePropertyAttribute), false).Count() == 0);

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
                properties = properties.Union(subType.GetModuleComponentPropertiesRecursive(bindingOptions));
            }

            return properties;
        }
    }
}
