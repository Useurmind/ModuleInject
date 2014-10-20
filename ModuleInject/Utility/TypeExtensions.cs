using ModuleInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
    public static class TypeExtensions
    {
        public static bool IsInjectionModuleType(this PropertyInfo propertyInfo)
        {
            var injectionModuleType = typeof(IInjectionModule);
            var searchedInterface = propertyInfo.PropertyType.GetInterface(injectionModuleType.Name, false);
            bool isModule = searchedInterface != null;
            return isModule;
        }

        public static bool HasCustomAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return memberInfo.HasCustomAttribute(typeof(TAttribute));
        }

        public static bool HasCustomAttribute(this MemberInfo memberInfo, Type attributeType)
        {
            return memberInfo.GetCustomAttributes(attributeType, false).Length > 0;
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
