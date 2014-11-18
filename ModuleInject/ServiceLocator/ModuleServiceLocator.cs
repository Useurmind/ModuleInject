using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.ServiceLocator
{
    using Microsoft.Practices.ServiceLocation;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;

    public class ModuleServiceLocator : IServiceLocator
    {
        private class Registration
        {
            public Type Type { get; set; }
            public string Key { get; set; }
            public Func<object> GetterFunc { get; set; }
        }

        private DoubleKeyDictionary<Type, string, Registration> registrations; 

        public ModuleServiceLocator()
        {
            this.registrations = new DoubleKeyDictionary<Type, string, Registration>();
        }

        public void AddRegistration(Type type, string key, Func<object> getterFunc)
        {
            this.registrations.Add(type, key, new Registration()
                                                  {
                                                      Type = type,
                                                      Key =  key,
                                                      GetterFunc = getterFunc
                                                  });
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            Type serviceType = typeof(TService);
            return GetAllInstances(serviceType).Select(x => (TService)x);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return this.registrations.GetAll(serviceType).Select(x => x.GetterFunc());
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService));
        }

        public object GetInstance(Type serviceType, string key)
        {
            Registration registration = null;
            if(!registrations.TryGetValue(serviceType, key, out registration))
            {
                ExceptionHelper.ThrowFormatException(Errors.ServiceLocator_CouldNotFindInstanceRegistration, key, serviceType);
            }

            return registration.GetterFunc();
        }

        public object GetInstance(Type serviceType)
        {
            var registrations = this.registrations.GetAll(serviceType);

            if (registrations.Count() > 1)
            {
                ExceptionHelper.ThrowFormatException(Errors.ServiceLocator_MultipleRegistrationsFound, serviceType);
            }

            return registrations.First().GetterFunc();
        }

        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }
    }
}
