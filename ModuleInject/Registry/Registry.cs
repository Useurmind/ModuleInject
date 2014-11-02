﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Registry
{
    using System.Dynamic;

    using ModuleInject.Common.Exceptions;
    using ModuleInject.Utility;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container;

    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class Registry : IRegistry
    {
        private const string componentName = "a";

        protected IDependencyContainer container;

        public Registry()
        {
            container = new DependencyContainer();
        }

        internal override  bool IsRegistered(Type type)
        {
            return container.IsRegistered(componentName, type);
        }

        internal override object GetComponent(Type type)
        {
            return container.Resolve(componentName, type);
        }

        public virtual void Register<T>(Func<T> factoryFunc)
        {
            container.Register(componentName, typeof(T), depCont => (object)factoryFunc());
        }

        protected virtual void Register(Type type, object instance)
        {
            container.Register(componentName, type, instance);
        }
    }
}
