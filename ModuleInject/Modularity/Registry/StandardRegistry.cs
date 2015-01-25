﻿using System;
using System.Linq;

using ModuleInject.Container;
using ModuleInject.Container.Interface;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class StandardRegistry : RegistryBase
    {
        private const string componentName = "a";

        protected IDependencyContainer Container { get; private set; }

        public StandardRegistry()
        {
            this.Container = new DependencyContainer();
        }

        public override bool IsRegistered(Type type)
        {
            return this.Container.IsRegistered(componentName, type);
        }

        public override object GetComponent(Type type)
        {
            return this.Container.Resolve(componentName, type);
        }

        public virtual void Register<T>(Func<T> factoryFunc)
        {
            this.Container.Register(componentName, typeof(T), depCont => (object)factoryFunc());
        }

        protected virtual void Register(Type type, object instance)
        {
            this.Container.Register(componentName, type, instance);
        }
    }
}