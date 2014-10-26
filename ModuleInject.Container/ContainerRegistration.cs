using System.Linq;

namespace ModuleInject.Container
{
    using System;
    using System.Collections;

    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.InstanceCreation;
    using ModuleInject.Container.Interface;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class ContainerRegistration : IContainerRegistration
    {
        private IList<IDependencyInjection> dependencyInjections;

        private IList<IInterceptionBehavior> behaviours; 

        public Type ActualType { get; set; }

        public Type RegisteredType { get; set; }

        public string Name { get; set; }

        public ILifetime Lifetime { get; set; }

        public IEnumerable<IDependencyInjection> DependencyInjections
        {
            get
            {
                return dependencyInjections;
            }
        }

        public ContainerRegistration()
        {
            dependencyInjections = new List<IDependencyInjection>();
            behaviours = new List<IInterceptionBehavior>();
        }

        public void AddDependencyInjection(IDependencyInjection dependencyInjection)
        {
            dependencyInjections.Add(dependencyInjection);
        }

        public object Resolve()
        {
            object instance = null;
            if (Lifetime.OnObjectResolving())
            {
                instance = this.InstanceCreation.Resolve(ActualType);
                ResolveDependencies(instance);

                instance = ApplyBehaviours(instance);

                Lifetime.OnObjectResolved(new ObjectResolvedContext()
                                              {
                                                  ActualType = ActualType,
                                                  RegisteredType = RegisteredType,
                                                  Name = Name,
                                                  Instance = instance
                                              });
            }
            else
            {
                instance = Lifetime.OnObjectNotResolved();
            }
            return instance;
        }

        private object ApplyBehaviours(object instance)
        {
            if (behaviours.Count == 0)
            {
                return instance;
            }

            InterfaceInterceptor interfaceInterceptor = new InterfaceInterceptor();
            var proxy = interfaceInterceptor.CreateProxy(RegisteredType, instance);
            foreach (var behaviour in behaviours)
            {
                proxy.AddInterceptionBehavior(behaviour);
            }
            return proxy;
        }

        private void ResolveDependencies(object instance)
        {
            foreach (var dependencyInjection in dependencyInjections)
            {
                dependencyInjection.Resolve(instance);
            }
        }
        public IInstanceCreation InstanceCreation { get; set; }

        public void AddBehaviour(IInterceptionBehavior behaviour)
        {
            behaviours.Add(behaviour);
        }
    }
}
