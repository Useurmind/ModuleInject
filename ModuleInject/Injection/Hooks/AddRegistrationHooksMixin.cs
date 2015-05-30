using ModuleInject.Interfaces;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Interfaces.Hooks;
using ModuleInject.Interfaces.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModuleInject.Injection.Hooks
{
    /// <summary>
    /// Interface for classes that should allow the addition of registration hooks.
    /// </summary>
    public interface IAddRegistrationHooksMixin
    {
        /// <summary>
        /// Adds a registration hook to the instance.
        /// </summary>
        /// <param name="registrationHook">The registration hook to add.</param>
        void AddRegistrationHook(IRegistrationHook registrationHook);
    }

    public static class AddRegistrationHooksMixin
    {
        /// <summary>
        /// Adds an interface injector registration hook to the instance.
        /// </summary>
        /// <typeparam name="TIComponent">The interface that a component must implement so that the hook is executed on it.</typeparam>
        /// <typeparam name="TIModule">The interface that a module must implement so that the hook is executed on it.</typeparam>
        /// <param name="hookedInstance">The instance to add the hook to.</param>
        /// <param name="injectInto">The action that contains the injection pattern for the component.</param>
        public static void AddRegistrationHook<TIModule, TIComponent>(
            this IAddRegistrationHooksMixin hookedInstance,
            Action<IInterfaceInjectionRegister<TIModule, TIComponent>> injectInto)
            where TIComponent : class
            where TIModule : class
        {
            hookedInstance.AddRegistrationHook(new InterfaceInjector<TIModule, TIComponent>(injectInto));
        }

        /// <summary>
        /// Adds an interface injector registration hook to the instance.
        /// </summary>
        /// <typeparam name="IComponent">The interface that a component must implement so that the hook is executed on it.</typeparam>
        /// <typeparam name="IModule">The interface that a module must implement so that the hook is executed on it.</typeparam>
        /// <param name="hookedInstance">The instance to add the hook to.</param>
        /// <param name="interfaceInjector">The injector that should be executed for matching components.</param>
        public static void AddRegistrationHook<IComponent, IModule>(
            this IAddRegistrationHooksMixin hookedInstance,
            IInterfaceInjector<IComponent, IModule> interfaceInjector)
            where IComponent : class
            where IModule : class
        {
            var hook = new InterfaceInjectorRegistrationHook<IComponent, IModule>(interfaceInjector);
            hookedInstance.AddRegistrationHook(hook);
        }
    }
}
