using System.Linq;

namespace ModuleInject.Container
{
    using System;
    using System.Collections.Generic;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;
    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;
    using ModuleInject.Container.Resolving;

    public class DependencyContainer : IDependencyContainer
    {
        private DoubleKeyDictionary<Type, string, ContainerRegistration> registrations;

        public IEnumerable<IContainerRegistration> Registrations
        {
            get
            {
                return this.registrations.GetAll();
            }
        }

        public DependencyContainer()
        {
            this.registrations = new DoubleKeyDictionary<Type, string, ContainerRegistration>();
        }

        public void Register(string name, Type registeredType, Type actualType)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, registeredType);
            registration.ActualType = actualType;
        }

        public void SetLifetime(string name, Type type, ILifetime lifetime)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);
            registration.Lifetime = lifetime;
        }

        public void InjectProperty(string name, Type type, string propertyName, IResolvedValue value)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);
            registration.AddDependencyInjection(new PropertyDependencyInjection()
                                                    {
                                                        PropertyName = propertyName,
                                                        ResolvedValue = value
                                                    });
        }

        private ContainerRegistration GetOrCreateRegistration(string name, Type registeredType)
        {
            ContainerRegistration registration = this.GetRegistration(name, registeredType);
            if (registration == null)
            {
                registration = new ContainerRegistration()
                    {
                        RegisteredType = registeredType,
                        Name = name,
                        Lifetime = new SingletonLifetime(),
                        ConstructorDependencyInjection = new ConstructorDependencyInjection()
                    };

                this.registrations.Add(registeredType, name, registration);
            }
            return registration;
        }

        private ContainerRegistration GetRegistration(string name, Type registeredType)
        {
            ContainerRegistration registration = null;
            this.registrations.TryGetValue(registeredType, name, out registration);
            return registration;
        }


        public object Resolve(string name, Type type)
        {
            IContainerRegistration registration = this.GetRegistration(name, type);
            if (registration == null)
            {
                ExceptionHelper.ThrowFormatException(Errors.DependencyContainer_RegistrationNotFound, type.Name, name);
            }
            return registration.Resolve();
        }

        public void InjectMethod(string name, Type type, string methodName, IEnumerable<IResolvedValue> resolvedValues)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);
            var methodDependencyInjection = new MethodDependencyInjection()
                                                {
                                                    MethodName = methodName
                                                };

            foreach (var resolvedValue in resolvedValues)
            {
                methodDependencyInjection.AddParameterValue(resolvedValue);
            }

            registration.AddDependencyInjection(methodDependencyInjection);
        }

        public void InjectConstructor(string name, Type type, IEnumerable<IResolvedValue> resolvedValue)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);

            var constructorDependencyInjection = new ConstructorDependencyInjection();

            foreach (var resolvedParameter in resolvedValue)
            {
                constructorDependencyInjection.AddParameter(resolvedParameter);
            }

            registration.ConstructorDependencyInjection=constructorDependencyInjection;
        }
    }
}
