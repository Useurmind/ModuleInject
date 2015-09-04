using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ModuleInject.Common.Linq;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    public interface IFromInstanceContext
    {
        IFromInstanceContext ExceptProperty(string propertyName);

        IFromInstanceContext ExceptPropertiesFrom(Type type, bool recursive = false);

        IFromInstanceContext AddAllProperties();

        void Extract();

        //IFromInstanceContext AddPropertiesOf(Type type);
    }

    public class FromInstanceContext : IFromInstanceContext
    {
        private readonly object instance;
        private readonly ServiceProvider serviceProvider;

        private IEnumerable<PropertyInfo> properties;

        public object Instance { get { return instance; } }
        public ServiceProvider ServiceProvider { get { return serviceProvider; } }

        public FromInstanceContext(ServiceProvider serviceProvider, object instance)
        {
            this.serviceProvider = serviceProvider;
            this.instance = instance;
        }

        public IFromInstanceContext AddAllProperties()
        {
            properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return this;
        }
        public IFromInstanceContext ExceptProperty(string propertyName)
        {
            FilterProperties(props => props.Where(p => p.Name != propertyName));
            return this;
        }

        public IFromInstanceContext ExceptPropertiesFrom(Type type, bool recursive = false)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            if(!recursive)
            {
                bindingFlags |= BindingFlags.DeclaredOnly;
            }

            var exceptProperties = type.GetProperties(bindingFlags);

            FilterProperties(props => props.Except(exceptProperties, new PropertyNameComparer()));

            return this;
        }

        public void FilterProperties(Func<IEnumerable<PropertyInfo>, IEnumerable<PropertyInfo>> filterProperties)
        {
            properties = filterProperties(properties);
        }

        public void Extract()
        {
            foreach (var property in properties)
            {
                serviceProvider.AddServiceSource(new PropertyServiceSource(instance, property));
            }
        }
    }

    public static class ServiceProviderFactoryExtensions
    {
        public static IFromInstanceContext FromInstance(this ServiceProvider serviceProvider, object instance)
        {
            return new FromInstanceContext(serviceProvider, instance);
        }
    }

    public static class AllPropertiesContextExtensions
    {
        public static IFromInstanceContext ExceptProperty<TInstance, TProperty>(
            this IFromInstanceContext context,
            Expression<Func<TInstance, TProperty>> propertyExpression)
        {
            return context.ExceptProperty((string)Property.Get(propertyExpression));
        }

        public static IFromInstanceContext ExceptPropertiesFrom<TExcept>(this IFromInstanceContext context, bool recursive=false)
        {
            return context.ExceptPropertiesFrom(typeof(TExcept), recursive);
        }
    }
}
