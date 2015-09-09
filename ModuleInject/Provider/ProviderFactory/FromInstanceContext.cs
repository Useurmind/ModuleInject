using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    /// <summary>
    /// This context is used to start adding properties or methods as service sources to a service provider.
    /// </summary>
    public interface IFromInstanceContext
    {
        /// <summary>
        /// Choose all properties for addition as service sources.
        /// </summary>
        /// <returns>A context narrow down the set of added properties.</returns>
        IAllPropertiesContext AllProperties();

        /// <summary>
        /// Choose all get methods for addition as service sources.
        /// </summary>
        /// <returns>A context narrow down the set of added methods.</returns>
        IAllGetMethodsContext AllGetMethods();
    }

    public class FromInstanceContext : IFromInstanceContext
    {
        private readonly object instance;
        private readonly ServiceProvider serviceProvider;
        
        public object Instance { get { return instance; } }
        public ServiceProvider ServiceProvider { get { return serviceProvider; } }

        public FromInstanceContext(ServiceProvider serviceProvider, object instance)
        {
            this.serviceProvider = serviceProvider;
            this.instance = instance;
        }

        public IAllPropertiesContext AllProperties()
        {
            return new AllPropertiesContext(this);
        }

        public IAllGetMethodsContext AllGetMethods()
        {
            return new AllGetMethodsContext(this);
        }
    }
}
