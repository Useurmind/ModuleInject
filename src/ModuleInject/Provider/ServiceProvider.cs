using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider
{
    /// <summary>
    /// Allows to register different sources of services that this service provider calls to return services.
    /// Each service type can only be registered once.
    /// </summary>
    public class ServiceProvider : IServiceProvider
    {
        private Dictionary<Type, ISourceOfService> serviceSources;

        public ServiceProvider()
        {
            serviceSources = new Dictionary<Type, ISourceOfService>();
        }

        /// <summary>
        /// Returns the number of registered services.
        /// </summary>
        public int NumberOfServices
        {
            get
            {
                return serviceSources.Count;
            }
        }

        /// <summary>
        /// Add a source for a service.
        /// </summary>
        /// <param name="serviceSource">The source of the service.</param>
        public ServiceProvider AddServiceSource(ISourceOfService serviceSource)
        {
            if(HasService(serviceSource.Type))
            {
                ExceptionHelper.ThrowFormatException(Errors.ServiceProvider_AlreadyContainsThisServiceType, serviceSource.Type);
            }

            serviceSources.Add(serviceSource.Type, serviceSource);

            return this;
        }

        /// <summary>
        /// Is a service of the given type registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to check for.</typeparam>
        /// <returns>True if the service is registered, else false.</returns>
        public bool HasService<TService>()
        {
            return HasService(typeof(TService));
        }

        /// <summary>
        /// Is a service of the given type registered.
        /// </summary>
        /// <param name="type">The type of the service to check for.</param>
        /// <returns>True if the service is registered, else false.</returns>
        public bool HasService(Type type)
        {
            return serviceSources.ContainsKey(type);
        }

        /// <summary>
        /// Get a service of the given type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }

        /// <summary>
        /// Get a service of the given type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType)
        {
            ISourceOfService sourceOfService = null;
            if(!serviceSources.TryGetValue(serviceType, out sourceOfService))
            {
                // service provider must return null if no service is registered
                return null;                
                //ExceptionHelper.ThrowFormatException(Errors.ServiceProvider_DoesNotContainThisServiceType, serviceType.Name);
            }

            return sourceOfService.Get();
        }
    }
}
