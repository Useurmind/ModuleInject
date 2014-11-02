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

    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the properties of the all types in the _module chain.
        /// </summary>
        /// <typeparam name="IModule">The interface of the _module.</typeparam>
        /// <typeparam name="TModule">The type of the _module.</typeparam>
        /// <param name="thatAreModules">if set to <c>true</c> only properties that are modules themselves are returned, else all other properties.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetModuleProperties<IModule, TModule>(bool thatAreModules)
            where TModule : IModule
        {
            return TypeExtensions.GetModuleProperties(typeof(IModule), typeof(TModule), thatAreModules);
        }

        /// <summary>
        /// Gets the properties of the all types in the module inheritance chain.
        /// </summary>
        /// <param name="moduleInterface">The interface of the module.</param>
        /// <param name="moduleType">The type of the module.</param>
        /// <param name="thatAreModules">if set to <c>true</c> only properties that are modules themselves are returned, else all other properties.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetModuleProperties(Type moduleInterface, Type moduleType, bool thatAreModules)
        {
            var interfaceProperties = moduleInterface.GetModuleComponentPropertiesRecursive()
                                                     .Where(p =>
                                                     {
                                                         bool isModule = p.IsInjectionModuleType();
                                                         return thatAreModules ? isModule : !isModule;
                                                     })
                                                     .Select(p => moduleType.GetProperty(p.Name));

            var privateProperties = moduleType.GetModuleComponentPropertiesRecursive(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                              .Where(p =>
                                              {
                                                  bool isPrivate = p.HasCustomAttribute<PrivateComponentAttribute>();
                                                  bool isRegistry = p.HasCustomAttribute<RegistryComponentAttribute>();
                                                  bool isModule = p.IsInjectionModuleType();

                                                  return (isRegistry || isPrivate) && (thatAreModules ? isModule : !isModule);
                                              });

            return privateProperties.Union(interfaceProperties);
        }

        public static bool IsInjectionModuleType(this PropertyInfo propertyInfo)
        {
            CommonFunctions.CheckNullArgument("propertyInfo", propertyInfo);

            var injectionModuleType = typeof(IInjectionModule);
            var searchedInterface = propertyInfo.PropertyType.GetInterface(injectionModuleType.Name, false);
            bool isModule = searchedInterface != null;
            return isModule;
        }

        public static void SetPropertyRecursive(this Type type, object instance, string name, object value)
        {
            var propertySetter = type.GetPropertySetterRecursive(name, BindingFlags.NonPublic | BindingFlags.Public);
            if (propertySetter == null)
            {
                ExceptionHelper.ThrowFormatException(Errors.TypeExtensions_NoPropertySetterFound, name, type.Name);
            }
            propertySetter.Invoke(
                instance,
                BindingFlags.NonPublic | BindingFlags.Public,
                null,
                new object[] { value },
                CultureInfo.InvariantCulture);
        }

        public static MethodInfo GetPropertySetterRecursive(this Type type, string name, BindingFlags? bindingOptions = null)
        {
            CommonFunctions.CheckNullArgument("type", type);

            if (type.ShouldStopModuleTypeRecursion())
            {
                return null;
            }

            var propertyInfo = GetPropertyInfo(type, name, bindingOptions);
            var setMethod = propertyInfo == null ? null : propertyInfo.GetSetMethod(true);
            if (setMethod == null)
            {
                type.ForEachBaseTypeInModuleHierarchy(
                    subType =>
                    {
                        setMethod = subType.GetPropertySetterRecursive(name, bindingOptions);
                        if (setMethod != null)
                        {
                            return true;
                        }
                        return false;
                    });
            }

            return setMethod;
        }

        public static PropertyInfo GetPropertyRecursive(this Type type, string name, BindingFlags? bindingOptions = null)
        {
            CommonFunctions.CheckNullArgument("type", type);

            if (type.ShouldStopModuleTypeRecursion())
            {
                return null;
            }

            var propertyInfo = GetPropertyInfo(type, name, bindingOptions);

            if (propertyInfo == null)
            {
                type.ForEachBaseTypeInModuleHierarchy(
                    subType =>
                    {
                        propertyInfo = GetPropertyRecursive(subType, name, bindingOptions);
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

            IEnumerable<PropertyInfo> properties = bindingOptions == null ? type.GetProperties() : type.GetProperties(BindingFlags.DeclaredOnly | bindingOptions.Value);

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
