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
    public static class ServiceProviderFactoryExtensions
    {
        public static IFromInstanceContext FromInstance(this ServiceProvider serviceProvider, object instance)
        {
            return new FromInstanceContext(serviceProvider, instance);
        }

        public static ServiceProvider AddServiceSource<TService>(this ServiceProvider serviceProvider, Func<TService> createService)
        {
            serviceProvider.AddServiceSource(new LambdaServiceSource<TService>(createService));

            return serviceProvider;
        }
    }
}
