using System.Linq;

namespace ModuleInject.Container
{
    using System;
    using System.Collections;

    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.InstanceCreation;
    using ModuleInject.Container.Interface;
    using System.Collections.Generic;

    public class ContainerRegistration : IContainerRegistration
    {
        private IList<IDependencyInjection> dependencyInjections;

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
                Lifetime.OnObjectResolved(instance);
            }
            else
            {
                instance = Lifetime.OnObjectNotResolved();
            }
            return instance;
        }

        private void ResolveDependencies(object instance)
        {
            foreach (var dependencyInjection in dependencyInjections)
            {
                dependencyInjection.Resolve(instance);
            }
        }
        public IInstanceCreation InstanceCreation { get; set; }
    }
}
