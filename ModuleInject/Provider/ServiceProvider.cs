using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces.Provider;

namespace ModuleInject.Provider
{
    /// <summary>
    /// Allows to register different sources that this service provider returns.
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
        /// Add a source for a service.
        /// </summary>
        /// <param name="serviceSource">The source of the service.</param>
        public void AddServiceSource(ISourceOfService serviceSource)
        {
            if(HasService(serviceSource.Type))
            {
                ExceptionHelper.ThrowFormatException(Errors.ServiceProvider_AlreadyContainsThisServiceType, serviceSource.Type);
            }

            serviceSources.Add(serviceSource.Type, serviceSource);
        }

        public bool HasService<T>()
        {
            return HasService(typeof(T));
        }

        public bool HasService(Type type)
        {
            return serviceSources.ContainsKey(type);
        }

        /// <summary>
        /// Get a service of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
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
                ExceptionHelper.ThrowFormatException(Errors.ServiceProvider_DoesNotContainThisServiceType, serviceType.Name);
            }

            return sourceOfService.Get();
        }
    }
}
