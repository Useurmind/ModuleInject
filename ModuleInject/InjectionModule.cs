using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using ModuleInject.Fluent;
using ModuleInject.Module;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject
{
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Common.Utility;
    using ModuleInject.Container;
    using ModuleInject.Container.Interface;
    using ModuleInject.Container.Lifetime;
    using ModuleInject.Decoration;
    using ModuleInject.Interfaces;
    using ModuleInject.Interfaces.Fluent;
    using ModuleInject.Registry;

    public abstract class InjectionModule : IInjectionModule, IDisposable
    {
        internal abstract Type ModuleInterface { get; }
        internal abstract Type ModuleType { get; }

        internal abstract IDependencyContainer Container { get; }

        public abstract void Resolve(IRegistry registry);

        public abstract bool IsResolved { get; }

        public abstract void Resolve();

        public abstract object GetComponent(Type componentType, string componentName);

        public abstract IComponent GetComponent<IComponent>(string componentName);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);

        internal abstract void OnComponentResolved(ObjectResolvedContext context);
    }
}
