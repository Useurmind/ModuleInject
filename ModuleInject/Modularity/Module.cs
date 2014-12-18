using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces;
using ModuleInject.Modularity.Registry;
using ModuleInject.Modules;

namespace ModuleInject.Modularity
{
    /// <summary>
    /// Base class for all modules
    /// </summary>
    public abstract class Module : IModule, IDisposable
    {
        private IRegistry registry;

        private bool isResolved;

        /// <inheritdoc />
        public bool IsResolved
        {
            get
            {
                return this.isResolved;
            }
        }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        /// <value>
        /// The registry.
        /// </value>
        public IRegistry Registry
        {
            get
            {
                if (this.registry as EmptyRegistry != null)
                {
                    return null;
                }

                return this.registry;
            }
            set
            {
                if (value == null)
                {
                    this.registry = new EmptyRegistry();
                }
                this.registry = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        public Module()
        {
            this.registry = new EmptyRegistry();
        }

        /// <inheritdoc />
        public void Resolve()
        {
            this.Resolve(null);
        }

        /// <inheritdoc />
        public void Resolve(IRegistry parentRegistry)
        {
            if (this.IsResolved)
            {
                ExceptionHelper.ThrowTypeException(this.GetType(), Errors.InjectionModule_AlreadyResolved);
            }

            this.OnResolving();

            var usedRegistry = this.GetUsedRegistry(parentRegistry);

            RegistryResolver resolver = new RegistryResolver(this, usedRegistry);

            resolver.Resolve();

            this.OnRegistryResolved(usedRegistry);

            this.isResolved = true;

            this.OnResolved();
        }

        /// <summary>
        /// Called when all registry components are resolved and in place.
        /// Override it to implement the resolution logic for the module.
        /// </summary>
        protected abstract void OnRegistryResolved(IRegistry usedRegistry);

        /// <summary>
        /// Called before the <see cref="Resolve"/> method is executed.
        /// </summary>
        protected virtual void OnResolving() { }

        /// <summary>
        /// Called after the <see cref="Resolve"/> method was executed.
        /// </summary>
        protected virtual void OnResolved() { }

        protected IRegistry GetUsedRegistry(IRegistry parentRegistry)
        {
            var usedRegistry = this.registry;
            if (parentRegistry != null)
            {
                usedRegistry = this.registry.Merge(parentRegistry);
            }
            return usedRegistry;
        }

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
        protected virtual void Dispose(bool disposing)
        {
            this.registry.Dispose();
        }
    }
}
