using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Common.Linq;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    public interface IAllPropertiesContext
    {
        /// <param name="filter">Should return true for all properties that should be included.</param>
        IAllPropertiesContext Where(Func<PropertyInfo, bool> filter);

        IAllPropertiesContext ExceptFrom(Type type, bool recursive = false);

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

        public IAllPropertiesContext ExceptFrom(Type type, bool recursive = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if (!recursive)
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
        public static IAllPropertiesContext ExceptFrom<TExcept>(this IAllPropertiesContext context, bool recursive = false)
        {
            return context.ExceptFrom(typeof(TExcept), recursive);
        }
    }
}
