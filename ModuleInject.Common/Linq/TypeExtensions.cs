using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ModuleInject.Common.Linq
{
    public static class TypeExtensions
    {
        public static void SetPropertyRecursive(this Type type, object instance, string name, object value, bool onlyWhenNotNull=false)
        {
            CommonFunctions.CheckNullArgument("type", type);

            var propertyWithSetterRecursive = type.GetPropertyWithSetterRecursive(name, BindingFlags.NonPublic | BindingFlags.Public);
            if (propertyWithSetterRecursive == null)
            {
                ExceptionHelper.ThrowFormatException(Errors.TypeExtensions_NoPropertySetterFound, name, type.Name);
            }

            if(onlyWhenNotNull)
            {
                if(propertyWithSetterRecursive.GetValue(instance, null) != null)
                {
                    return;
                }
            }

            propertyWithSetterRecursive.SetValue(
                instance,
                value,
                BindingFlags.NonPublic | BindingFlags.Public,
                null,
                null,
                CultureInfo.InvariantCulture);
        }

        public static PropertyInfo GetPropertyWithSetterRecursive(this Type type, string name, BindingFlags? bindingOptions = null)
        {
            CommonFunctions.CheckNullArgument("type", type);

            var propertyInfo = GetPropertyInfo(type, name, bindingOptions);
            bool hasSetMethod = propertyInfo == null ? false : propertyInfo.GetSetMethod(true) != null;
            if (!hasSetMethod)
            {
                propertyInfo = null;

                type.ForEachBaseType(
                    subType =>
                    {
                        propertyInfo = subType.GetPropertyWithSetterRecursive(name, bindingOptions);
                        if (propertyInfo != null)
                        {
                            return true;
                        }
                        return false;
                    });
            }

            return propertyInfo;
        }

        private static PropertyInfo GetPropertyInfo(Type type, string name, BindingFlags? bindingOptions)
        {
            PropertyInfo propertyInfo = bindingOptions.HasValue
                                            ? type.GetProperty(name, BindingFlags.Instance | bindingOptions.Value)
                                            : type.GetProperty(name);
            return propertyInfo;
        }

        private static void ForEachBaseType(this Type type, Func<Type, bool> action)
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
