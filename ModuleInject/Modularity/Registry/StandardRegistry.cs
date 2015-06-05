using System;
using System.Collections.Generic;
using System.Linq;

using ModuleInject.Container;
using ModuleInject.Container.Interface;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Injection.Hooks;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class StandardRegistry : RegistryBase, IAddRegistrationHooksMixin
    {
        private const string componentName = "a";
        private ISet<IRegistrationHook> registrationHooks;

        protected IDependencyContainer Container { get; private set; }

        public StandardRegistry()
        {
            this.Container = new DependencyContainer();
            this.registrationHooks = new HashSet<IRegistrationHook>();
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

        /// <summary>
        /// Adds a registration hools to the registry.
        /// </summary>
        /// <param name="registrationHook">The registration hook to add.</param>
        public virtual void AddRegistrationHook(IRegistrationHook registrationHook)
        {
            this.registrationHooks.Add(registrationHook);
        }

        public override IEnumerable<IRegistrationHook> GetRegistrationHooks()
        {
            return this.registrationHooks;
        }
    }

   
}
