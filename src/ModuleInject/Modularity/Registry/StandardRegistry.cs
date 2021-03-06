﻿using System;
using System.Collections.Generic;
using System.Linq;

using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Injection.Hooks;
using ModuleInject.Interfaces.Injection;
using ModuleInject.Injection;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class StandardRegistry : RegistryBase, IAddRegistrationHooksMixin
    {
        private const string componentName = "a";
        private ISet<IRegistrationHook> registrationHooks;
        private ISimpleInjectionContainer container;

        public StandardRegistry()
        {
            this.container = new InjectionContainer();
            this.registrationHooks = new HashSet<IRegistrationHook>();
        }

        /// <inheritdoc />
        public override bool IsRegistered(Type type)
        {
            return this.container.IsRegistered(type);
        }

        /// <inheritdoc />
        public override object GetComponent(Type type)
        {
            return this.container.GetComponent(type);
        }

        /// <summary>
        /// Register an instance under the given type.
        /// </summary>
        /// <typeparam name="T">The type which serves as key for the registration.</typeparam>
        /// <param name="factoryFunc">Func that returns the instance on resolution.</param>
        public virtual void Register<T>(Func<T> factoryFunc)
        {
            this.container.Register(typeof(T), () => factoryFunc());
        }

        /// <summary>
        /// Register an instance under the given type.
        /// </summary>
        /// <param name="type">The type which serves as key for the registration.</param>
        /// <param name="instance">The instance to register.</param>
        protected virtual void Register(Type type, object instance)
        {
            this.container.Register(type, () => instance);
        }

        /// <summary>
        /// Adds a registration hools to the registry.
        /// </summary>
        /// <param name="registrationHook">The registration hook to add.</param>
        public virtual void AddRegistrationHook(IRegistrationHook registrationHook)
        {
            this.registrationHooks.Add(registrationHook);
        }

        /// <inheritdoc />
        public override IEnumerable<IRegistrationHook> GetRegistrationHooks()
        {
            return this.registrationHooks;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.container.Dispose();
            }

            base.Dispose(disposing);
        }
    }


}
