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
        /// <summary>
        /// Start extracting properties and methods from a given instance as service sources.
        /// </summary>
        /// <param name="serviceProvider">The service provider to add the service sources to.</param>
        /// <param name="instance">The instance from which the service sources should be extracted.</param>
        /// <returns>A context to further specify what to add.</returns>
        public static IFromInstanceContext FromInstance(this ServiceProvider serviceProvider, object instance)
        {
            return new FromInstanceContext(serviceProvider, instance, instance.GetType());
        }

        /// <summary>
        /// Start extracting properties and methods from a specific interface of the given instance as service sources.
        /// </summary>
        /// <param name="serviceProvider">The service provider to add the service sources to.</param>
        /// <param name="instance">The instance from which the service sources should be extracted.</param>
        /// <typeparam name="TInstanceInterface">The interface of the instance which should be available for extraction.</typeparam>
        /// <returns>A context to further specify what to add.</returns>
        public static IFromInstanceContext FromInstance<TInstanceInterface>(this ServiceProvider serviceProvider, object instance)
        {
            return new FromInstanceContext(serviceProvider, instance, typeof(TInstanceInterface));
        }

        /// <summary>
        /// Start extracting properties and methods from a specific interface of the given instance as service sources.
        /// </summary>
        /// <param name="serviceProvider">The service provider to add the service sources to.</param>
        /// <param name="instance">The instance from which the service sources should be extracted.</param>
        /// <param name="instanceInterface">The interface of the instance which should be available for extraction.</param>
        /// <returns>A context to further specify what to add.</returns>
        public static IFromInstanceContext FromInstance(this ServiceProvider serviceProvider, object instance, Type instanceInterface)
        {
            return new FromInstanceContext(serviceProvider, instance, instanceInterface);
        }

        /// <summary>
        /// Add a func that returns a service as service source to a service provider.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="serviceProvider">The service provider to add the service source to.</param>
        /// <param name="createService">The func that will return the service when requested.</param>
        /// <returns>The service provider itself.</returns>
        public static ServiceProvider AddServiceSource<TService>(this ServiceProvider serviceProvider, Func<TService> createService)
        {
            serviceProvider.AddServiceSource(new LambdaServiceSource<TService>(createService));

            return serviceProvider;
        }
    }
}
