using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Common.Linq;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    /// <summary>
    /// This context is used to filter and add the properties of an instance as a service source to a service provider.
    /// </summary>
    public interface IAllPropertiesContext
    {
        /// <summary>
        /// From all remaining properties only keep the ones that fullfill the given predicate.
        /// </summary>
        /// <param name="filter">Should return true for all properties that should be included.</param>
        IAllPropertiesContext Where(Func<PropertyInfo, bool> filter);

        /// <summary>
        /// Exclude the properties that are declared on the given type (or further up the inheritance chain).
        /// </summary>
        /// <param name="type">The type from which properties should be excluded.</param>
        /// <param name="inherited">If set to true all properties up the inheritance chain starting from the given type are excluded.</param>
        IAllPropertiesContext ExceptFrom(Type type, bool inherited = false);

        /// <summary>
        /// Confirm that the chosen set of properties should be added to the service provider.
        /// </summary>
        /// <returns></returns>
        IFromInstanceContext Extract();
    }

    public class AllPropertiesContext : IAllPropertiesContext
    {
        private readonly FromInstanceContext fromInstance;
        private IEnumerable<PropertyInfo> properties;

        public AllPropertiesContext(FromInstanceContext fromInstance)
        {
            this.fromInstance = fromInstance;
            properties = fromInstance.Instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">Should return true for all properties that should be included.</param>
        /// <returns></returns>
        public IAllPropertiesContext Where(Func<PropertyInfo, bool> filter)
        {
            properties = properties.Where(filter);
            return this;
        }

        public IAllPropertiesContext ExceptFrom(Type type, bool inherited = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (!inherited)
            {
                bindingFlags |= BindingFlags.DeclaredOnly;
            }

            var exceptProperties = type.GetProperties(bindingFlags);

            properties = properties.Except(exceptProperties, new PropertyNameComparer());

            return this;
        }

        public IFromInstanceContext Extract()
        {
            foreach (var property in properties)
            {
                fromInstance.ServiceProvider.AddServiceSource(new PropertyServiceSource(fromInstance.Instance, property));
            }
            return fromInstance;
        }
    }

    public static class AllPropertiesContextExtensions
    {
        /// <summary>
        /// Exclude the properties that are declared on the given type (or further up the inheritance chain).
        /// </summary>
        /// <typeparam name="TExcept">The type from which properties should be excluded.</typeparam>
        /// <param name="inherited">If set to true all properties up the inheritance chain starting from the given type are excluded.</param>
        public static IAllPropertiesContext ExceptFrom<TExcept>(this IAllPropertiesContext context, bool inherited = false)
        {
            return context.ExceptFrom(typeof(TExcept), inherited);
        }
    }
}
