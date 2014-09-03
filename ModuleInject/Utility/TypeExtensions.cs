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
        public static IEnumerable<PropertyInfo> GetPropertiesRecursive(this Type type, BindingFlags? bindingFlags = null)
        {
            if (type == typeof(object) || type == typeof(IInjectionModule) || type.Name == "InjectionModule")
            {
                return new List<PropertyInfo>();
            }

            IEnumerable<PropertyInfo> properties = bindingFlags == null ? type.GetProperties() : type.GetProperties(bindingFlags.Value);

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
                properties = properties.Union(subType.GetPropertiesRecursive(bindingFlags));
            }

            return properties;
        }
    }
}
