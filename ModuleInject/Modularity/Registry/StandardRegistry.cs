using System;
using System.Collections.Generic;
using System.Linq;

using ModuleInject.Container;
using ModuleInject.Container.Interface;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces;
using ModuleInject.Hooks;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Modules.Fluent;

namespace ModuleInject.Modularity.Registry
{
    /// <summary>
    /// A simple registry module that implements type based resolution of entries.
    /// </summary>
    public class StandardRegistry : RegistryBase
    {
        private const string componentName = "a";
        private IList<IRegistrationHook> registrationHooks;

        protected IDependencyContainer Container { get; private set; }

        public StandardRegistry()
        {
            this.Container = new DependencyContainer();
            this.registrationHooks = new List<IRegistrationHook>();
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

    public static class StandardRegistryExtensions
    {
        /// <summary>
        /// Adds an interface injector registration hook to the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface that a component must implement so that the hook is executed on it.</typeparam>
        /// <typeparam name="IModule">The interface that a module must implement so that the hook is executed on it.</typeparam>
        /// <param name="registry">The registy to add the hook to.</param>
        /// <param name="injectInto">The action that contains the injection pattern for the component.</param>
        public static void AddRegistrationHook<IComponent, IModule>(
            this StandardRegistry registry,
            Action<IInterfaceRegistrationContext<IComponent, IModule>> injectInto)
            where IComponent : class
            where IModule : class
        {
            registry.AddRegistrationHook(new InterfaceInjector<IComponent, IModule>(injectInto));
        }

        /// <summary>
        /// Adds an interface injector registration hook to the module.
        /// </summary>
        /// <typeparam name="IComponent">The interface that a component must implement so that the hook is executed on it.</typeparam>
        /// <typeparam name="IModule">The interface that a module must implement so that the hook is executed on it.</typeparam>
        /// <param name="registry">The registy to add the hook to.</param>
        /// <param name="interfaceInjector">The injector that should be executed for matching components.</param>
        public static void AddRegistrationHook<IComponent, IModule>(
            this StandardRegistry registry,
            IInterfaceInjector<IComponent, IModule> interfaceInjector)
            where IComponent : class
            where IModule : class
        {
            var hook = new InterfaceInjectorRegistrationHook<IComponent, IModule>(interfaceInjector);
            registry.AddRegistrationHook(hook);
        }
    }
}
