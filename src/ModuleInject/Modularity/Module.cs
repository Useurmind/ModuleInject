using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ModuleInject.Common.Exceptions;
using ModuleInject.Interfaces;
using ModuleInject.Modularity.Registry;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Common.Disposing;

namespace ModuleInject.Modularity
{
    /// <summary>
    /// Base class for all modules
    /// </summary>
    public abstract class Module : DisposableExtBase, IModule, IDisposable, IAddRegistrationHooksMixin
    {
        private IRegistry registry;

        private bool isResolved;
        private bool isResolving;
        private HashSet<IRegistrationHook> registrationHooks;

        /// <summary>
        /// Gets the registrations hooks that are registered for application in this module only.
        /// </summary>
        public IEnumerable<IRegistrationHook> RegistrationHooks { get { return this.registrationHooks; } }

        /// <summary>
        /// Gets a value indicating whether this instance is currently resolving.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is resolving; otherwise, <c>false</c>.
        /// </value>
        protected bool IsResolving
        {
            get
            {
                return this.isResolving;
            }
        }

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
            this.registrationHooks = new HashSet<IRegistrationHook>();
            this.isResolving = false;
            this.isResolved = false;
        }

        /// <summary>
        /// Adds a registration hook to the module.
        /// </summary>
        /// <param name="registrationHook">The registration hook to add.</param>
        public virtual void AddRegistrationHook(IRegistrationHook registrationHook)
        {
            if(!registrationHook.AppliesToModule(this))
            {
                ExceptionHelper.ThrowTypeException(this.GetType(), Errors.Module_RegistrationHookDoesNotApply, registrationHook.GetType());
            }

            this.registrationHooks.Add(registrationHook);
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

            try
            {
                this.isResolving = true;

                this.OnResolving();

                var usedRegistry = this.GetUsedRegistry(parentRegistry);

                this.OnRegistryResolving(usedRegistry);

                RegistryResolver resolver = new RegistryResolver(this, usedRegistry);

                resolver.Resolve();

                this.OnRegistryResolved(usedRegistry);

                this.isResolved = true;
            }
            finally
            {
                this.isResolving = false;
            }

            this.OnResolved();
        }

        /// <summary>
        /// Called before the registry components are resolved.
        /// </summary>
        /// <param name="usedRegistry">The used registry.</param>
        protected virtual void OnRegistryResolving(IRegistry usedRegistry) { }

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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            this.registry.Dispose();

            base.Dispose(disposing);
        }
    }
}
