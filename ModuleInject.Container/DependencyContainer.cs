using System.Linq;

namespace ModuleInject.Container
{
    using System;
    using System.Collections.Generic;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Utility;
    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.InstanceCreation;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;
    using ModuleInject.Container.Resolving;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using ModuleInject.Container.Interface;

    public class ComponentResolvedEventArgs : EventArgs
    {
        public ComponentResolvedEventArgs(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; private set; }
        public string Name { get; private set; }
    }

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

        public event EventHandler<ComponentResolvedEventArgs> ComponentResolved;

        public DependencyContainer()
        {
            this.registrations = new DoubleKeyDictionary<Type, string, ContainerRegistration>();
        }

        public bool IsRegistered(string name, Type type)
        {
            return registrations.Contains(type, name);
        }

        public void Register(string name, Type registeredType, Type actualType)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, registeredType);
            registration.ActualType = actualType;
        }

        public void Register(string name, Type registeredType, object instance)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, registeredType);
            registration.ActualType = instance.GetType();
            registration.InstanceCreation = new ExistingInstance(instance);
        }

        public void Register(string name, Type registeredType, Func<IDependencyContainer, object> createInstance)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, registeredType);
            registration.InstanceCreation = new FactoryInstanceCreation(this, createInstance);
        }

        public void DefinePrerequisite(string name, Type registeredType, IResolvedValue prerequisite)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, registeredType);
            registration.AddPrerequisite(prerequisite);
        }

        public void SetLifetime(string name, Type type, ILifetime lifetime)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);
            registration.Lifetime = lifetime;
        }

        public void InjectProperty(string name, Type type, string propertyName, IResolvedValue value)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);

            this.Inject(name, type, new PropertyDependencyInjection()
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
                        InstanceCreation = new ConstructorDependencyInjection()
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
            object result = registration.Resolve();

            FireComponentResolved(name, type);

            return result;
        }

        private void FireComponentResolved(string name, Type type)
        {
            if (ComponentResolved != null)
            {
                ComponentResolved(this, new ComponentResolvedEventArgs(type, name));
            }
        }

        public void InjectMethod(string name, Type type, string methodName, IEnumerable<IResolvedValue> resolvedValues)
        {
            var methodDependencyInjection = new MethodDependencyInjection()
                                                {
                                                    MethodName = methodName
                                                };

            foreach (var resolvedValue in resolvedValues)
            {
                methodDependencyInjection.AddParameterValue(resolvedValue);
            }

            this.Inject(name, type, methodDependencyInjection);
        }

        public void Inject(string name, Type type, IDependencyInjection dependencyInjection)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);

            registration.AddDependencyInjection(dependencyInjection);
        }

        public void InjectConstructor(string name, Type type, IEnumerable<IResolvedValue> resolvedValue)
        {
            var constructorDependencyInjection = new ConstructorDependencyInjection();

            foreach (var resolvedParameter in resolvedValue)
            {
                constructorDependencyInjection.AddParameter(resolvedParameter);
            }

            SetInstanceCreation(name, type, constructorDependencyInjection);
        }

        public void SetInstanceCreation(string name, Type type, IInstanceCreation instanceCreation)
        {
            ContainerRegistration registration = this.GetOrCreateRegistration(name, type);
            if (registration.InstanceCreation as ExistingInstance != null)
            {
                ThrowRegistrationError(registration, Errors.DependencyContainer_ConstructorCannotBeConfiguredForExistingInstance);
            }

            registration.InstanceCreation = instanceCreation;
        }

        private static void ThrowRegistrationError(IContainerRegistration registration, string errorMsgFormat)
        {
            ExceptionHelper.ThrowFormatException(errorMsgFormat, registration.RegisteredType, registration.Name);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (var registration in registrations)
            {
                registration.Value.Lifetime.Dispose();
            }
        }

        public void AddBehaviour(string name, Type type, IInterceptionBehavior behaviour)
        {
            var registration = this.GetOrCreateRegistration(name, type);
            registration.AddBehaviour(behaviour);
        }
    }
}
