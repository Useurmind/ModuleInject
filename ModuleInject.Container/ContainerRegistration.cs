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
    using ModuleInject.Container.Interface;

    public class ContainerRegistration : IContainerRegistration
    {
        private IList<IDependencyInjection> dependencyInjections;

        private IList<IInterceptionBehavior> behaviours;
        private IList<IResolvedValue> dependencies; 

        public Type ActualType { get; set; }

        public Type RegisteredType { get; set; }

        public string Name { get; set; }

        public ILifetime Lifetime { get; set; }

        public IEnumerable<IResolvedValue> Prerequisite
        {
            get
            {
                return dependencies;
            }
        }

        public IEnumerable<IDependencyInjection> DependencyInjections
        {
            get
            {
                return dependencyInjections;
            }
        }

        public IInstanceCreation InstanceCreation { get; set; }

        public ContainerRegistration()
        {
            dependencyInjections = new List<IDependencyInjection>();
            behaviours = new List<IInterceptionBehavior>();
            dependencies = new List<IResolvedValue>();
        }

        public void AddDependencyInjection(IDependencyInjection dependencyInjection)
        {
            dependencyInjections.Add(dependencyInjection);
        }

        public void AddPrerequisite(IResolvedValue dependency)
        {
            dependencies.Add(dependency);
        }

        public void AddBehaviour(IInterceptionBehavior behaviour)
        {
            behaviours.Add(behaviour);
        }

        public object Resolve()
        {
            object instance = null;
            if (Lifetime.OnObjectResolving())
            {
                // resolve dependencies used in lambda expressions
                ResolvePrerequisite();

                instance = this.InstanceCreation.Resolve(ActualType);

                InjectDependencies(instance);

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

        private void ResolvePrerequisite()
        {
            foreach (var prerequisite in Prerequisite)
            {
                prerequisite.Resolve();
            }
        }

        private void InjectDependencies(object instance)
        {
            foreach (var dependencyInjection in dependencyInjections)
            {
                dependencyInjection.Resolve(instance);
            }
        }
    }
}
