using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ModuleInject.Provider.ServiceSources;

namespace ModuleInject.Provider.ProviderFactory
{
    public interface IFromInstanceContext
    {
        IAllPropertiesContext AddAllProperties();

        IAllGetMethodsContext AddAllGetMethods();
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

        public IAllPropertiesContext AddAllProperties()
        {
            return new AllPropertiesContext(this);
        }

        public IAllGetMethodsContext AddAllGetMethods()
        {
            return new AllGetMethodsContext(this);
        }
    }
}
